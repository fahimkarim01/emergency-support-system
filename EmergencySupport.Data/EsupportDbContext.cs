using EmergencySupport.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmergencySupport.Data
{
    public class EsupportDbContext(DbContextOptions<EsupportDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<EmergencyRequests> EmergencyRequests { get; set; }
        public DbSet<Responders> Responders { get; set; }
        public DbSet<Assignments> Assignments { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<ReportsLogs> ReportsLogs { get; set; }
    }
}