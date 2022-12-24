using SynchronousShops.Libraries.Extensions;
using MimeKit;
using SynchronousShops.Libraries.SMTP.Extensions;

namespace SynchronousShops.Libraries.SMTP.Extensions
{
    public static class MimeMessageExtensions
    {
        public static string ToJson(this MimeMessage email)
        {
            var shortEmail = new
            {
                email.From,
                email.To,
                email.Cc,
                email.Bcc,
                email.Subject,
                Body = email.Body.ToString()
            };
            return shortEmail.ToJson();
        }
    }
}
