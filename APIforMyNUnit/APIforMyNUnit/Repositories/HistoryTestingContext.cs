using APIforMyNUnit.Models;
using Microsoft.EntityFrameworkCore;

namespace APIforMyNUnit.Repositories
{
    /// <summary>
    /// Class for creating and working with the database
    /// </summary>
    public class HistoryTestingContext : DbContext
    {
        public DbSet<TestedAssemblyModel> Assemblys { get; set; }

        /// <summary>
        /// Сreates a database if it has not been created yet
        /// </summary>
        public HistoryTestingContext(DbContextOptions<HistoryTestingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
