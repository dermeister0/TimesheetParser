using System;
using System.ComponentModel.DataAnnotations;

namespace TimesheetParser.DataLayer.Entities
{
    /// <summary>
    /// Timesheet entity.
    /// </summary>
    public class Timesheet
    {
        /// <summary>
        /// Date of timesheet.
        /// </summary>
        [Key]
        public DateTime Date { get; set; }

        /// <summary>
        /// Raw timesheet text.
        /// </summary>
        public string Text { get; set; }
    }
}
