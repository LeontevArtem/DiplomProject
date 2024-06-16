using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common
{
    public static class SMTPMail
    {
        public static void SendMail(string Subject,string Body,string ReceiverMail)
        {
            SmtpClient Smtp = new SmtpClient("smtp.yandex.ru", 587);
            Smtp.EnableSsl = true;
            Smtp.Credentials = new NetworkCredential("artemleont03@yandex.ru", "vnuwgyukmojigyst");
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("artemleont03@yandex.ru", "Artemiy Leontyev");
            Message.To.Add(new MailAddress(ReceiverMail));
            Message.Subject = Subject;
            string code = new Random().Next(1000, 9999).ToString();
            Message.Body = Body;
            Smtp.Send(Message);
        }
    }
}
