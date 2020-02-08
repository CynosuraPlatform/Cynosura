using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Infrastructure
{
    public interface IConfigurationModule<T>
    {
        void Configure(T configuration);
    }
}
