using Microsoft.EntityFrameworkCore;
using TimesheetParser.DataLayer.Entities;

namespace TimesheetParser.DataLayer
{
    /// <summary>
    /// Main app DB context.
    /// </summary>
    internal class AppDbContext : DbContext
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext()
        {
        }

        /// <summary>
        /// Constructor with options.
        /// </summary>
        /// <param name="options">DB context options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Timesheets DB set.
        /// </summary>
        public DbSet<Timesheet> Timesheets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Development.db");
        }
    }
}
