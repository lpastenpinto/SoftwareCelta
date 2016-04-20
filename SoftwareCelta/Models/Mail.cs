using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime; 

namespace SoftwareCelta.Models
{
    public class Mail
    {
        public static void send(string emailTo, string subject, string body)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFrom = "kathyleonetflix@gmail.com";
            string password = "federico88";
            //string emailTo = "norum19@gmail.com";
            //string subject = "";
            //string body = "";


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);                    
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
            
        }
    }
}