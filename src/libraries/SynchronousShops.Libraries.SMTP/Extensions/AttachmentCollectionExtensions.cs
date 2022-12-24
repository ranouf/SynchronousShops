using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SynchronousShops.Libraries.SMTP.Extensions
{
    public static class AttachmentCollectionExtensions
    {
        public static AttachmentCollection AddRange(this AttachmentCollection collection, IEnumerable<Attachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                collection.Add(attachment);
            }
            return collection;
        }
    }
}
