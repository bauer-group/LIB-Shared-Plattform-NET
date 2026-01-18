using System;

namespace BAUERGROUP.Shared.Data.Application.Data
{
    /// <summary>
    /// Defines the contract for entries that can be stored in an <see cref="Interface.IApplicationDataStore"/>.
    /// </summary>
    public interface IApplicationDataStoreEntry
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entry.
        /// </summary>
        Guid UID { get; set; }
    }
}