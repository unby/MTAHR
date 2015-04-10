 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 using System.Net.Pop3;
 using BaseType.Common;
using BaseType.Utils;
 using LumiSoft.Net;
 using LumiSoft.Net.DNS.Client;
 using LumiSoft.Net.POP3.Client;
 using Validator = System.ComponentModel.DataAnnotations.Validator;

namespace test
{
    [Flags]
    public enum Test
    {
        Noll=0,
        OneP = 1,
        TwoO=-4,       
        TwoP=4
    }

    internal class Program
    {
        public static string template = @"    <html>
    <body style=”color:grey; font-size:15px;”>
    <font face=”Helvetica, Arial, sans-serif”>

    <div style=”position:absolute; height:100px; width:600px; background-color:0d1d36; padding:30px;”>
    <img src=”logo” />
    </div>

    <br/>
    <br/>

    <div style=”background-color: #ece8d4; width:600px; height:200px; padding:30px; margin-top:30px;”>

    <p>Dear {0},<p>

    <p>Please click the link below to login and get started.</p>
    <p>
    <a href=””>Login Here</a>

    Username: {1}<br>
    Password: {2}<br>

     

    <br/>
    <p>Thank you</p>
    </div>
    </body>
    </html>";


        private static void Main(string[] args)
        {
            try
            {
                // 192.168.222.17
                var client = new Pop3Client();
                Dns_Client s = new Dns_Client();
                string adress = "study\natalya.tolstova";
                string password = "NataT2013";
                string hosts = "mail.study.rsvpu.ru";
                var host = s.GetEmailHosts(hosts)[0];
                string h = host.Addresses[0].ToString();
                //client.Server = "mail.study.rsvpu.ru";
                //client.Port = 110;
                //client.ReadTimeout = 10000;
                //client.Connect();
                //client.Login("natalya.tolstova@rsvpu.ru", "tyujyu", AuthenticationType.Ntlm);
                //Console.WriteLine();
                POP3_Client cl = new POP3_Client();
                cl.Timeout = 40000;
                Console.WriteLine(WellKnownPorts.POP3);
                cl.Connect("mail.study.rsvpu.ru", WellKnownPorts.POP3, false);
                // cl.Auth(new AUTH_SASL_Client_Ntlm("study", "natalya.tolstova@rsvpu.ru", password));
                cl.Authenticate("natalya.tolstova", password, false);
                Console.WriteLine(cl.IsAuthenticated);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                Console.ReadLine();
            }
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
