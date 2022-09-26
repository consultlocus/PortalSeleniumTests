using System;
using System.Collections.Generic;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace ConsultlocusSelenium.Helpers
{
    internal class Emailer
    {
        public static string Receiver = "srz@consultlocus.com";

        public static void SendEmail(string title, string body)
        {
            try
            {
                Outlook._Application app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = Receiver;
                mail.Subject = title;
                mail.Body = body;
                mail.Importance = Outlook.OlImportance.olImportanceNormal;

                mail.Send();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}