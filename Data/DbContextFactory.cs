using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<Data.DbContext>
    {
        public Data.DbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot Configuration;

            var bilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = bilder.Build();

            var option = new DbContextOptionsBuilder<Data.DbContext>();
            option.UseSqlServer(Configuration["ConnectionStrings:Base"]);

            return new Data.DbContext(option.Options);
        }
    }
}
