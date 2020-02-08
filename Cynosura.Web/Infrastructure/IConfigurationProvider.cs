using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Infrastructure
{
    public interface IConfigurationProvider<T>
    {
        void Configure(T configuration);
    }
}
