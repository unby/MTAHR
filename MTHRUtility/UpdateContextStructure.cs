using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseType;

namespace MTHRUtility
{
    class UpdateContextStructure:IUtility
    {
        private KeyAndValues keyAndValues;
        ApplicationDbContext _context=new ApplicationDbContext("MTHRData");
        public UpdateContextStructure()
        {
        }

        public UpdateContextStructure(KeyAndValues keyAndValues)
        {
            // TODO: Complete member initialization
            this.keyAndValues = keyAndValues;
        }

        public void Execute()
        {
            try
            {
                var mess = new AppJurnal()
                {
                    DateEntry = DateTime.Now,
                    IdEntry = Guid.NewGuid(),
                    MessageType = MessageType.System,
                    MessageCode = 2001,
                    Message =
                        string.Format("Обновление структуры ApplicationDbContext до версии {0}", _context.InfoContext)
                };
                DbMigrationsConfiguration configuration = new DbMigrationsConfiguration()
                {
                    MigrationsAssembly = typeof(ApplicationDbContext).Assembly,
                    ContextType = typeof(ApplicationDbContext),
                    AutomaticMigrationsEnabled = true,
                };

                DbMigrator dbMigrator = new DbMigrator(configuration);
                dbMigrator.Update(null);      
                
                _context.AppJurnal.Add(mess);
                _context.SaveChanges();
                Console.WriteLine(mess.Message);
                Console.WriteLine("The utility {0} is executed with parameters {1}", keyAndValues.Key, keyAndValues.Values.Aggregate((current, next) => current + ", " + next));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка в операции обновления структуры {0}",_context.GetType());
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

    }
}
