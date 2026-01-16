using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class EmailHelper
    {
        public static Boolean IsEmailAddressValid(this String sEmail)
        {
            if (String.IsNullOrWhiteSpace(sEmail))
                return false;

            try
            {
                new MailAddress(sEmail);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
