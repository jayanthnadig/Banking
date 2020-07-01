using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Dashboard;
using ASNRTech.CoreService.Email;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Reports;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Data {
    public class TeamDbContext : DbContext {
        private TeamHttpContext httpContext;

        /// <summary>
        /// use this when using DbContext in Async mode with multiple iterations
        /// </summary>
        /// <param name="httpContext"></param>
        public TeamDbContext(TeamHttpContext httpContext) {
            this.httpContext = httpContext;
        }

        public TeamDbContext() {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseNpgsql(Utility.ConnString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("public");

            // ensure property g/setters are used
            modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);

            // indices
            modelBuilder.Entity<AppLogEntry>().HasIndex(e => new { e.Level, e.AdminNotified });
            modelBuilder.Entity<AppEvent>().HasIndex(e => new { e.ParentType, e.ParentId });
            modelBuilder.Entity<User>().HasIndex(e => new { e.UserId, e.Email, e.Deleted });
            modelBuilder.Entity<UserSession>().HasIndex(e => new { e.UserId, e.SessionId, e.Deleted });
        }

        public DbSet<AppLogEntry> AppLogEntries { get; set; }
        public DbSet<ApiLogEntry> ApiLogEntries { get; set; }
        public DbSet<AppEvent> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<EmailDto> Emails { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<UploadLog> uploadLogs { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<UserDashboard> UserDashboards { get; set; }
        public DbSet<ChartType> ChartTypes { get; set; }
        public DbSet<ReportConfig> ReportConfigs { get; set; }

        public override int SaveChanges() {
            SetAuditFields();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            SetAuditFields();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditFields() {
            string userName = Thread.CurrentPrincipal?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName) && this.httpContext?.CurrentUser != null) {
                userName = this.httpContext.CurrentUser.UserId;
            }

            if (string.IsNullOrWhiteSpace(userName)) {
                userName = string.Empty;
            }

            foreach (EntityEntry ent in this.ChangeTracker.Entries()) {
                if (ent.Entity is BaseModel baseEntry) {
                    switch (ent.State) {
                        case EntityState.Added:
                            baseEntry.ModifiedOn = baseEntry.CreatedOn = DateTime.Now;
                            if (string.IsNullOrWhiteSpace(baseEntry.CreatedBy)) {
                                baseEntry.ModifiedBy = baseEntry.CreatedBy = userName;
                            }
                            baseEntry.Deleted = false;
                            break;

                        case EntityState.Deleted:
                            if (baseEntry.LogicalDelete) {
                                // when deleted, do a soft-delete
                                // set the deleted flag and change the state to modified
                                ent.State = EntityState.Modified;
                                baseEntry.Deleted = true;
                                baseEntry.ModifiedOn = DateTime.Now;
                                if (string.IsNullOrWhiteSpace(baseEntry.ModifiedBy)) {
                                    baseEntry.ModifiedBy = userName;
                                }
                            }
                            break;

                        case EntityState.Modified:
                            baseEntry.ModifiedOn = DateTime.Now;
                            if (string.IsNullOrWhiteSpace(baseEntry.ModifiedBy)) {
                                baseEntry.ModifiedBy = userName;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
