using APIforMyNUnit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIforMyNUnit.Repositories
{
    public class HistoryTestingContext : DbContext
    {
        public DbSet<TestedAssemblyModel> Assemblys { get; set; }

        public HistoryTestingContext(DbContextOptions<HistoryTestingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
