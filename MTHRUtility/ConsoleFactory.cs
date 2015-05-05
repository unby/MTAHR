using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTHRUtility
{
    class ConsoleFactory
    {
        public List<KeyAndValues> Params { get; private set; }
        public bool Init(string[] args)
        {
            if (args == null || args.Any()||args[0].ToLower()==@"/?")
            {
                Console.WriteLine("mthrutility -updpass -updcontext -getdb" +
                                  "1. -updpass обновить пароли пользователей {0}" +
                                  "2. -updcontext обновить структуру БД {0} " +
                                  "3. -getdb получить информацию о БД" ,Environment.NewLine);
                return false;
            }
            else
            {
                Params=new List<KeyAndValues>();
                KeyAndValues currentParamsGroup=null;
                for (var i = 0; i < args.Length; i++)
                {
                   if (args[i].StartsWith("-"))
                    {
                        currentParamsGroup = new KeyAndValues(args[i]);
                        Params.Add(currentParamsGroup);
                    }
                    if (currentParamsGroup != null)
                    {
                        currentParamsGroup.Values.Add(args[i]);
                    }
                }
                return true;
            }
        }

        internal void FactoryExecute()
        {
            if (Params.Any(a => a.Key == "-updpass"))
            {
                new PasswordUserUpdate(Params.First(f => f.Key == "-updpass")).Execute();
            }
            if (Params.Any(a => a.Key == "-updcontext"))
            {               
                new UpdateContextStructure(Params.First(f => f.Key == "-updcontext")).Execute();               
            }
            if (Params.Any(a => a.Key == "-getdb"))
            {
                new DbInfo(Params.First(f => f.Key == "-getdb")).Execute();
            }
        }

        internal void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
