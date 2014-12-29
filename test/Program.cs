using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using BaseType;


namespace test
{
    internal class Program
    {
   private static void Main(string[] args)
        {
            var user=new User(){IdUser = Guid.NewGuid(),Email = "ee@e1.ru",Name = "g",Surname = "sdfsd",Password = "mypass"};
            Console.WriteLine(user.Password);

            Console.WriteLine(CostumValidator(user));

      


            Console.ReadLine();
        }

        class Person
        {
            public string Name { get; set; }
            public bool IsWork { get; set; }

            public string Post { get; set; }
        }


        public static bool CostumValidator(object obj)
        {
            var context = new ValidationContext(obj, null, null);
            var results = new List<ValidationResult>();
            return  Validator.TryValidateObject(obj, context, results, true);
        }
    }
}
