using System;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.EF
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContext Get();
    }
}
