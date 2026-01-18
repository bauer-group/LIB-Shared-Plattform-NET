using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Core.Application
{
    /// <summary>
    /// Base class for application lifecycle management.
    /// </summary>
    public class ApplicationController: IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// </summary>
        public ApplicationController()
        {

        }

        /// <summary>
        /// Releases all resources used by the <see cref="ApplicationController"/>.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
