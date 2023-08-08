using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MailController : Controller
    {
        /// <summary>
        /// MailKit paketi kurulmalıdır
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(MailRequest mailRequest)
        {
            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "jarvis.tony.34@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", mailRequest.ReceiverMail);
            mimeMessage.To.Add(mailboxAddressTo);

            mimeMessage.Subject = mailRequest.Subject;
            //mimeMessage.Body = mailRequest.Body;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = mailRequest.Body;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("jarvis.tony.34@gmail.com", "hwrerqhbddsqyuqz");//  "jarvis.tony.34@gmail.com" mail adresi için şifre pqabajejwxfnsiau
            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);


            return Redirect("/Admin/Dashboard/Index");
        }
    }
}
