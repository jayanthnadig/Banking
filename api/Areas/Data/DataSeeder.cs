using ASNRTech.CoreService.Dashboard;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Utilities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core {
    internal static class DataSeeder {
        private static TeamHttpContext httpContext;

        internal static async Task SeedAsync() {
            try {
                if (!Utility.IsProduction) {
                    AddUsersAndClients();
                    AddNewTransactions();
                    AddNewUserDashboard();
                }
            }
            catch (Exception ex) {
                int i = 0;
                throw;
            }
        }

        private static void AddUser(User user) {
            user.UserId = user.UserId.ToUpper(CultureInfo.InvariantCulture);
            using (TeamDbContext dbContext = new TeamDbContext()) {
                if (dbContext.Users.FirstOrDefault(e => e.UserId == user.UserId) == null) {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
            }
        }

        private static void AddTransactions(Transactions transactions) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                dbContext.Transactions.Add(transactions);
                dbContext.SaveChanges();
            }
        }

        private static void AddUserDashboard(UserDashboard userdashboard) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                dbContext.UserDashboards.Add(userdashboard);
                dbContext.SaveChanges();
            }
        }

        private static void AddUsersAndClients() {
            AddUser(new User {
                UserId = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@asnrtech.com",
                UserType = UserType.Admin,
                Source = "DIRECT",
                Password = Utility.GetMd5Hash("testing_1")
            });

            AddUser(new User {
                UserId = "client",
                FirstName = "Client",
                LastName = "Client",
                Email = "client@asnrtech.com",
                UserType = UserType.Client,
                Source = "ADMIN",
                Password = Utility.GetMd5Hash("testing_1")
            });
        }

        private static void AddNewTransactions() {
            AddTransactions(new Transactions {
                TransactionId = 1,
                BranchCode = "221",
                TotalTransactions = 500,
                MakerId = "FenixPlaza",
                AssignedTo = "Jayanth",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Authorised"
            });

            AddTransactions(new Transactions {
                TransactionId = 2,
                BranchCode = "222",
                TotalTransactions = 500,
                MakerId = "FenixPlaza",
                AssignedTo = "Jayanth",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Authorised"
            });

            AddTransactions(new Transactions {
                TransactionId = 3,
                BranchCode = "223",
                TotalTransactions = 500,
                MakerId = "FenixPlaza",
                AssignedTo = "Jayanth",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Unauthorised"
            });
        }

        private static void AddNewUserDashboard() {
            AddUserDashboard(new UserDashboard {
                DashboardId = 1,
                DashboardUserId = "Admin",
                DashboardChartType = "Bar Chart",
                DashboardWidgetName = "My Bar Chart",
                DashbaordQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                DashbaordCreatedBy = "Admin",
                DashbaordCreatedOn = DateTime.Now,
                DashbaordModifiedOn = null
            });

            AddUserDashboard(new UserDashboard {
                DashboardId = 2,
                DashboardUserId = "Admin",
                DashboardChartType = "Pie Chart",
                DashboardWidgetName = "My Pie Chart",
                DashbaordQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                DashbaordCreatedBy = "Admin",
                DashbaordCreatedOn = DateTime.Now,
                DashbaordModifiedOn = null
            });
        }
    }
}
