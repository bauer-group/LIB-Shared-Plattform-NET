using System;

namespace BAUERGROUP.Shared.Data.EmbeddedDatabase.Internal
{
    internal static class ConcurrentPersistentDictionaryShared
    {        
        internal static Object GlobalLock = new Object();
    }
}
