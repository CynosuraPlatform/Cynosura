using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.EF
{
    public interface IDbContextModule
    {
        void CreateModel(ModelBuilder builder);
    }
}
