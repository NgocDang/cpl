using CPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public static class EmailHelper
    {
        public static bool Send(TemplateViewModel template, string toEmails, string ccEmails = "", string bccEmails = "", bool isBulked = false)
        {
            var enqueueResult = ServiceClient.EmailClient.EnqueueAsync(Authentication.Token, template.Subject, template.Body, toEmails, ccEmails, bccEmails, isBulked);
            enqueueResult.Wait();
            return enqueueResult.Result.Status.Code == 0;
        }
    }
}
