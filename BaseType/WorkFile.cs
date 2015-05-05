using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BaseType
{
    public class WorkFile
    {
        public WorkFile()
        {
        }

        public WorkFile(Guid id, FileInfo fileInfo)
        {
            FileId = id;
            FileInfo = fileInfo;
        }

        [Key]
        public Guid FileId { get; set; }

        [Required]
        [StringLength(254, ErrorMessage = @"Превышена длина имени файла")]
        // [RegularExpression(@"<>:""/\|?\*", ErrorMessage = @"'<>:""/\|?*' не доступные символы")]
        public string FileName { get; set; }

        [StringLength(60, ErrorMessage = "Длина комментария превышена (60)")]
        public string Comment { get; set; }

        public DateTime DateCreate { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual Task Catalog { get; set; }
        public int Size { get; set; }

        [NotMapped]
        public FileInfo FileInfo { get; set; }

        private bool ChekedConnectionString(string connStr)
        {
            var list = connStr.ToLower().Split(';');
            foreach (var param in list.Where(param => param.Contains("integrated") && param.Contains("security")))
            {
                if (param.Contains("true")) return true;
                else
                    throw new ArgumentException("Bad connection string", param);
            }
            return false;
        }

        public void LoadToDb(string connStr)
        {
            if (ChekedConnectionString(connStr))
                LoadToDbSqlFileS(connStr);
            else
                LoadToDbInsertFile(connStr);
        }

        public void LoadToDbInsertFile(string connStr)
        {
            if (FileInfo == null)
                throw new ArgumentNullException(@"FileInfo");

            using (var fileStream = FileInfo.Open(FileMode.Open, FileAccess.Read))
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var trn = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmdInsert = new SqlCommand(
                                @"insert into WorkFiles 
   ([FileId]
   ,[FileName]
   ,[DateCreate]
   ,[Size]
   ,[Author_Id]
   ,[Catalog_IdTask]
   ,[Data])
values 
    (@idFile,@fileName,@dateCreate,@size,@author_Id,@catalog_IdTask,@data)", conn, trn);
                            cmdInsert.Parameters.Add(new SqlParameter("@idFile", SqlDbType.UniqueIdentifier)
                            {
                                Value = this.FileId
                            });
                            cmdInsert.Parameters.Add(new SqlParameter("@fileName", SqlDbType.VarChar, 254)
                            {
                                Value = FileInfo.Name
                            });

                            cmdInsert.Parameters.Add(new SqlParameter("@dateCreate", SqlDbType.DateTime)
                            {
                                Value = FileInfo.LastWriteTime
                            });
                            cmdInsert.Parameters.Add(new SqlParameter("@size", SqlDbType.Int) {Value = FileInfo.Length});
                            cmdInsert.Parameters.Add(new SqlParameter("@author_Id", SqlDbType.UniqueIdentifier)
                            {
                                Value = this.Author.Id
                            });
                            cmdInsert.Parameters.Add(new SqlParameter("@catalog_IdTask", SqlDbType.UniqueIdentifier)
                            {
                                Value = this.Catalog.IdTask
                            });
                            var data = new byte[fileStream.Length];
                            fileStream.Read(data, 0, data.Length);
                            cmdInsert.Parameters.Add(new SqlParameter("@data", SqlDbType.Image)
                            {
                                Value = data
                            });
                            cmdInsert.ExecuteNonQuery();
                            trn.Commit();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            trn.Rollback();
                            conn.Close();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void LoadToDbSqlFileS(string connStr)
        {
            if (FileInfo == null)
                throw new ArgumentNullException(@"FileInfo");

            using (var fileStream = FileInfo.Open(FileMode.Open, FileAccess.Read))
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var trn = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmdInsert = new SqlCommand(
                                @"insert into WorkFiles 
   ([FileId]
   ,[FileName]
   ,[DateCreate]
   ,[Size]
   ,[Author_Id]
   ,[Catalog_IdTask]
   ,[Data])
output 
	INSERTED.Data.PathName(),    
    GET_FILESTREAM_TRANSACTION_CONTEXT ()
values 
    (@idFile, @fileName,  @dateCreate,@size,@author_Id,@catalog_IdTask, 0x)", conn, trn);
                            cmdInsert.Parameters.Add(new SqlParameter("@idFile", SqlDbType.UniqueIdentifier)
                            {
                                Value = this.FileId
                            });
                            cmdInsert.Parameters.Add(new SqlParameter("@fileName", SqlDbType.VarChar, 254)
                            {
                                Value = FileInfo.Name
                            });

                            cmdInsert.Parameters.Add(new SqlParameter("@dateCreate", SqlDbType.DateTime)
                            {
                                Value = FileInfo.LastWriteTime
                            });
                            cmdInsert.Parameters.Add(new SqlParameter("@size", SqlDbType.Int)
                            {Value = FileInfo.Length});
                            cmdInsert.Parameters.Add(new SqlParameter("@author_Id", SqlDbType.UniqueIdentifier)
                            {
                                Value = this.Author.Id
                            });
                            cmdInsert.Parameters.Add(new SqlParameter("@catalog_IdTask", SqlDbType.UniqueIdentifier)
                            {
                                Value = this.Catalog.IdTask
                            });

                            string path = null;
                            byte[] context = null;

                            using (var rdr = cmdInsert.ExecuteReader())
                            {
                                rdr.Read();
                                path = rdr.GetString(0);
                                context = rdr.GetSqlBytes(1).Buffer;
                            }

                            using (var sfs = new SqlFileStream(path, context, FileAccess.Write))
                            {
                                fileStream.CopyTo(sfs);
                            }
                            trn.Commit();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            trn.Rollback();
                            conn.Close();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
        }


        private static string CheckExistFile(string path, string nameFile)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return Path.Combine(path,nameFile);
            }          
            var name = Path.GetFileNameWithoutExtension(nameFile);
            var ext = Path.GetExtension(nameFile);
            var freeFile = Path.Combine(path, nameFile);
            var num = 1;
            var search = true;
            do
            {
                if (File.Exists(freeFile))
                    freeFile = Path.Combine(path, string.Format("{0}({1}){2}", name, num++, ext));
                else
                    search = false;
            } while (search);
            return freeFile;
        }


        public void UnloadingOfFileInFolder(string pathOut, string connStr)
        {
            if (ChekedConnectionString(connStr))
                UnloadingOfFileInFolderSqlFileS(pathOut, connStr);
            else
                UnloadingOfFileInFolderFile(pathOut, connStr);
        }

        public void UnloadingOfFileInFolderFile(string pathOut, string connStr)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction trn = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
@"SELECT 
    Data
FROM WorkFiles
WHERE FileId = @fileId;", conn, trn);
                        var paramFilename = new SqlParameter(
                            "@fileId", SqlDbType.UniqueIdentifier);
                        paramFilename.Value = this.FileId;
                        cmd.Parameters.Add(paramFilename);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (false == reader.Read())
                            {
                                reader.Close();
                                trn.Dispose();
                                conn.Dispose();

                                throw new EntitySqlException("Bad entity");
                            }
                            byte[] context = new byte[this.Size];
                            reader.GetBytes(0, 0, context, 0, Size);


                            using (
                                var fileStream = new FileStream(CheckExistFile(pathOut, FileName), FileMode.Create,
                                    FileAccess.Write))
                            {
                                fileStream.Write(context, 0, context.Length);
                            }
                        }
                        trn.Commit();
                    }
                    catch (Exception)
                    {
                        trn.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UnloadingOfFileInFolderSqlFileS(string pathOut, string connStr)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction trn = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand(
@"SELECT 
Data.PathName() as path
GET_FILESTREAM_TRANSACTION_CONTEXT ()
FROM WorkFiles
WHERE FileId = @fileId;", conn, trn);
                    var paramFilename = new SqlParameter(
                        "@fileId", SqlDbType.UniqueIdentifier);
                    paramFilename.Value = this.FileId;
                    cmd.Parameters.Add(paramFilename);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (false == reader.Read())
                        {
                            reader.Close();
                            trn.Dispose();
                            conn.Dispose();

                            throw new EntitySqlException("Bad entity");
                        }

                        string path = reader.GetString(0);
                        byte[] context = reader.GetSqlBytes(1).Buffer;

                        using (
                            var fileStream = new FileStream(CheckExistFile(pathOut, FileName), FileMode.Create,
                                FileAccess.Write))
                        {
                            using (var sqlStream = new SqlFileStream(path, context, FileAccess.Read))
                            {
                                fileStream.CopyTo(sqlStream);
                            }
                        }
                    }
                }
            }
        }

        public byte[] GetByte(string connStr)
        {
            return ChekedConnectionString(connStr) ? GetByteFile(connStr) : GetByteFile(connStr);
        }

        private byte[] GetByteFile(string connStr)
        {
            byte[] context = new byte[this.Size];
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction trn = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = new SqlCommand(
@"SELECT 
    Data
FROM WorkFiles
WHERE FileId = @fileId;", conn, trn);
                        var paramFilename = new SqlParameter(
                            "@fileId", SqlDbType.UniqueIdentifier);
                        paramFilename.Value = this.FileId;
                        cmd.Parameters.Add(paramFilename);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (false == reader.Read())
                            {
                                reader.Close();
                                trn.Dispose();
                                conn.Dispose();

                                throw new EntitySqlException("Bad entity");
                            }
                           
                            reader.GetBytes(0, 0, context, 0, Size);
                        }
                        trn.Commit();
                    }
                    catch (Exception)
                    {
                        trn.Rollback();
                        throw;
                    }
                }
            }
            return context;
        }
    }
}

