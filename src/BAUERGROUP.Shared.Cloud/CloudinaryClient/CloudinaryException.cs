using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.CloudinaryClient
{
    public class CloudinaryException : Exception
    {
        public CloudinaryException()
            : base()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public CloudinaryException(String sMessage)
            : base(sMessage)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public CloudinaryException(String sMessage, Exception eInnerException)
            : base(sMessage, eInnerException)
        {

        }

        public CloudinaryException(HttpStatusCode eStatusCode, String sMessage)
            : base(sMessage)
        {
            StatusCode = eStatusCode;
        }

        public CloudinaryException(HttpStatusCode eStatusCode)
            : base()
        {
            StatusCode = eStatusCode;
        }

        public CloudinaryException(HttpStatusCode eStatusCode, String sMessage, Exception eInnerException)
            : base(sMessage, eInnerException)
        {
            StatusCode = eStatusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
