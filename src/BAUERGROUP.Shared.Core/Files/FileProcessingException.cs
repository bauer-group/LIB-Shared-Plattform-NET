using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Files
{  
    public class FileProcessingException : Exception
    {
        public FileProcessingException() :
            base()
        {

        }

        public FileProcessingException(String sMessage) :
            base(sMessage)
        {

        }

        public FileProcessingException(String sMessage, Exception eInnerException) :
            base(sMessage, eInnerException)
        {

        }
    }
}
