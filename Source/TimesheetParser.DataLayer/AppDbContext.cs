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
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        /// <summary>
        /// Timesheets DB set.
        /// </summary>
        public DbSet<Timesheet> Timesheets { get; set; }
    }
}
