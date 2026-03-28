using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FifoApi.Data
{
    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(
            string[] args
        )
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<ApplicationDBContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=fifo-db-dev;Username=postgres;Password=W!nnerX2"
            );

            return new ApplicationDBContext(
                optionsBuilder.Options
            );
        }
    }
}