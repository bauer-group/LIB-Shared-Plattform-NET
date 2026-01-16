using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Exceptions
{
    public class ChromeException : Exception
    {
        public ChromeException() 
            : base()
        {

        }

        public ChromeException(String sMessage)
            : base(sMessage)
        {

        }

        public ChromeException(String sMessage, Exception oInnerException)
            : base(sMessage, oInnerException)
        {

        }
    }
}
