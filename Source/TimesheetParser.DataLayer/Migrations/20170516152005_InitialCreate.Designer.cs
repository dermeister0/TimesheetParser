using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TimesheetParser.DataLayer;

namespace TimesheetParser.DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170516152005_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("TimesheetParser.DataLayer.Entities.Timesheet", b =>
                {
                    b.Property<DateTime>("Date");

                    b.Property<string>("Text");

                    b.HasKey("Date");

                    b.ToTable("Timesheets");
                });
        }
    }
}
