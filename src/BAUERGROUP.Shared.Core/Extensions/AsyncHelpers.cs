using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Core.Extensions
{
    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="oTask">Task<T> method to execute</param>
        public static void RunSync(Func<Task> oTask)
        {
            var oExistingContext = SynchronizationContext.Current;
            var oExclusiveSynchronisation = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(oExclusiveSynchronisation);

            oExclusiveSynchronisation.Post(async _ =>
            {
                try
                {
                    await oTask();
                }
                catch (Exception e)
                {
                    oExclusiveSynchronisation.InnerException = e;
                    throw;
                }
                finally
                {
                    oExclusiveSynchronisation.EndMessageLoop();
                }
            }, null);

            oExclusiveSynchronisation.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oExistingContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="oTask">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> oTask)
        {
            var oExistingContext = SynchronizationContext.Current;
            var oExclusiveSynchronisation = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(oExclusiveSynchronisation);

            T oReturn = default(T);
            oExclusiveSynchronisation.Post(async _ =>
            {
                try
                {
                    oReturn = await oTask();
                }
                catch (Exception e)
                {
                    oExclusiveSynchronisation.InnerException = e;
                    throw;
                }
                finally
                {
                    oExclusiveSynchronisation.EndMessageLoop();
                }
            }, null);

            oExclusiveSynchronisation.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oExistingContext);

            return oReturn;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private Boolean bCompleted;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent oWorkItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> oItems = new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback oPostCallback, Object oState)
            {
                throw new NotSupportedException("Unable to send on same Thread.");
            }

            public override void Post(SendOrPostCallback oPostCallback, Object oState)
            {
                lock (oItems)
                {
                    oItems.Enqueue(Tuple.Create(oPostCallback, oState));
                }

                oWorkItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => bCompleted = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!bCompleted)
                {
                    Tuple<SendOrPostCallback, object> oTask = null;

                    lock (oItems)
                    {
                        if (oItems.Count > 0)
                        {
                            oTask = oItems.Dequeue();
                        }
                    }

                    if (oTask != null)
                    {
                        oTask.Item1(oTask.Item2);

                        if (InnerException != null)
                        {
                            throw new AggregateException("AsyncHelpers.Run Method Exception.", InnerException);
                        }
                    }
                    else
                    {
                        oWorkItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}
