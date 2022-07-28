using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Models
{
    public class AdminDBContext:DbContext
    {
        public AdminDBContext()
        {
        }

        public AdminDBContext(DbContextOptions<AdminDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminCredentials> AdminCredentials { get; set; }
        public virtual DbSet<AirlineAddBlock> AirlineaddBlock { get; set; }
        public virtual DbSet<Flights> Flights { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();
            var connectionString = configuration.GetConnectionString("AdminDatabase");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
