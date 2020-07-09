using ASNRTech.CoreService.Dashboard;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Utilities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core
{
    internal static class DataSeeder
    {
        private static TeamHttpContext httpContext;

        internal static async Task SeedAsync()
        {
            try
            {
                if (!Utility.IsProduction)
                {
                    //AddUsersAndClients();
                    //AddNewTransactions();
                    //AddNewUserDashboard();
                }
            }
            catch (Exception ex)
            {
                int i = 0;
                throw;
            }
        }

        private static void AddUser(User user)
        {
            user.UserId = user.UserId.ToUpper(CultureInfo.InvariantCulture);
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.Users.FirstOrDefault(e => e.UserId == user.UserId) == null)
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
            }
        }

        private static void AddTransactions(Transactions transactions)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                dbContext.Transactions.Add(transactions);
                dbContext.SaveChanges();
            }
        }

        private static void AddUserDashboard(UserDashboard userdashboard)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                dbContext.UserDashboards.Add(userdashboard);
                dbContext.SaveChanges();
            }
        }

        private static void AddUsersAndClients()
        {
            AddUser(new User
            {
                UserId = "Admin",
                Email = "admin@asnrtech.com",
                UserType = UserType.Admin,
                Password = Utility.GetMd5Hash("testing_1"),
                Status = UserStatus.Active
            });

            AddUser(new User
            {
                UserId = "client",
                Email = "client@asnrtech.com",
                UserType = UserType.Client,
                Password = Utility.GetMd5Hash("testing_1"),
                Status = UserStatus.Active
            });
        }

        private static void AddNewTransactions()
        {
            AddTransactions(new Transactions
            {
                TransactionId = 1,
                BranchCode = "221",
                BranchName = "East Branch",
                MakerId = "FenixPlaza",
                AssignedTo = "Jayanth",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Authorised"
            });

            AddTransactions(new Transactions
            {
                TransactionId = 2,
                BranchCode = "222",
                BranchName = "West Branch",
                MakerId = "RentoPlaza",
                AssignedTo = "Santhosh",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "SBank_SA",
                Status = "Authorised"
            });

            AddTransactions(new Transactions
            {
                TransactionId = 3,
                BranchCode = "223",
                BranchName = "South Branch",
                MakerId = "FenixPlaza",
                AssignedTo = "Ashok",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Unauthorised"
            });

            AddTransactions(new Transactions
            {
                TransactionId = 4,
                BranchCode = "224",
                BranchName = "North Branch",
                MakerId = "FenixPlaza",
                AssignedTo = "Jayanth",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Unauthorised"
            });

            AddTransactions(new Transactions
            {
                TransactionId = 5,
                BranchCode = "221",
                BranchName = "East Branch",
                MakerId = "FenixPlaza",
                AssignedTo = "Jayanth",
                PostingDate = DateTime.Now,
                FunctionId = "TVCL",
                TransactionStatus = "IPR",
                BranchType = "UBank_SA",
                Status = "Authorised"
            });
        }

        private static void AddNewUserDashboard()
        {
            AddUserDashboard(new UserDashboard
            {
                DashboardUserId = "Admin",
                DashboardChartType = "Bar Chart",
                DashboardWidgetName = "My Bar Chart",
                DashbaordQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                DashbaordQueryL1 = "select * from public.\"Transactions\" where status='@status@'",
                DashbaordQueryL2 = "select * from public.\"Transactions\" where assignedto='@assignedto@' and transaction_status='@transaction_status@'",
                DashbaordQueryL3 = "select * from public.\"Transactions\" where branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                DashbaordModifiedOn = null
            });

            AddUserDashboard(new UserDashboard
            {
                DashboardUserId = "Admin",
                DashboardChartType = "Pie Chart",
                DashboardWidgetName = "My Pie Chart",
                DashbaordQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                DashbaordQueryL1 = "select * from public.\"Transactions\" where status='@status@'",
                DashbaordQueryL2 = "select * from public.\"Transactions\" where assignedto='@assignedto@' and transaction_status='@transaction_status@'",
                DashbaordQueryL3 = "select * from public.\"Transactions\" where branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                DashbaordModifiedOn = null
            });
        }
    }
}
