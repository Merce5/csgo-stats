using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Clients
{
    public class CsgoDbContext : DbContext
    {
        public CsgoDbContext(DbContextOptions<CsgoDbContext> options) : base(options)
        {

        }
    }
}