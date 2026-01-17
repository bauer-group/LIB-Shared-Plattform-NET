using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Core.Resilience
{
    public class ResilienceHelper
    {
        public ResilienceHelper()
        {

        }

        private void Debug()
        {
            //Documentation: https://github.com/App-vNext/Polly
            //Polly is a .NET resilience and transient-fault-handling library that allows developers
            //to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation,
            //and Fallback in a fluent and thread-safe manner.

            var p1 = Policy
                .Handle<Exception>()
                .Retry(2);

            var result = p1.ExecuteAndCapture(() => { });
        }
    }
}
