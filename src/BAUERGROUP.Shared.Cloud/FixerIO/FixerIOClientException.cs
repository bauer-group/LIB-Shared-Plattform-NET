using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    public class FixerIOClientException : Exception
    {
        public FixerIOClientException()
            : base()
        {

        }

        public FixerIOClientException(String sMessage)
            : base(sMessage)
        {

        }

        public FixerIOClientException(String sMessage, Exception eInnerException)
            : base(sMessage, eInnerException)
        {

        }
    }
}
