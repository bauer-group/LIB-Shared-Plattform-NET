using System;

namespace BAUERGROUP.Shared.Core.Application.Interface
{
    public interface IApplicationConfigurationStore<T> where T : new()
    {
        T Configuration { get; set; }

        void Save();
    }
}