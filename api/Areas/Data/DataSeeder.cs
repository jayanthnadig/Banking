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
                    //AddChartType();
                    //AddDBConnection();
                }
            }
            catch (Exception ex)
            {
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

        private static void AddChart(ChartType charttype)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                dbContext.ChartTypes.Add(charttype);
                dbContext.SaveChanges();
            }
        }

        private static void AddConnection(DBConnection dbconnection)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                dbContext.DBConnections.Add(dbconnection);
                dbContext.SaveChanges();
            }
        }

        private static void AddUsersAndClients()
        {
            AddUser(new User
            {
                UserId = "Admin",
                Email = "jayanthnadig@gmail.com",
                UserType = UserType.Admin,
                Password = Utility.GetMd5Hash("testing_1"),
                IsAdd = true,
                IsEdit = true,
                IsDelete = true,
                Status = UserStatus.Active
            });

            AddUser(new User
            {
                UserId = "client",
                Email = "santhuanandmca@gmail.com",
                UserType = UserType.Client,
                Password = Utility.GetMd5Hash("testing_1"),
                IsAdd = false,
                IsEdit = false,
                IsDelete = false,
                Status = UserStatus.Active
            });
        }

        private static void AddNewTransactions()
        {
            AddTransactions(new Transactions
            {
                CreatedBy = "ADMIN",
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
                CreatedBy = "ADMIN",
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
                CreatedBy = "CLIENT",
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
                CreatedBy = "CLIENT",
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
                CreatedBy = "ADMIN",
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
                DashboardUserId = "ADMIN",
                DashboardChartType = "Bar Chart",
                DashboardWidgetName = "My Bar Chart",
                DashboardConnectionString = "PgAdmin4ConnectionString",
                DashbaordQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                Level1ConnectionString = "SqlConnectionString",
                DashbaordQueryL1 = "select * from public.\"Transactions\" where status='@status@'",
                Level2ConnectionString = "OracleConnectionString",
                DashbaordQueryL2 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@'",
                Level3ConnectionString = "PgAdmin4ConnectionString",
                DashbaordQueryL3 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@ and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                Level4ConnectionString = "OracleConnectionString",
                DashbaordQueryL4 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@ and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                DashbaordModifiedOn = null
            });

            AddUserDashboard(new UserDashboard
            {
                DashboardUserId = "ADMIN",
                DashboardChartType = "Pie Chart",
                DashboardWidgetName = "My Pie Chart",
                DashboardConnectionString = "PgAdmin4ConnectionString",
                DashbaordQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                Level1ConnectionString = "SqlConnectionString",
                DashbaordQueryL1 = "select * from public.\"Transactions\" where status='@status@'",
                Level2ConnectionString = "OracleConnectionString",
                DashbaordQueryL2 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@'",
                Level3ConnectionString = "PgAdmin4ConnectionString",
                DashbaordQueryL3 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@ and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                Level4ConnectionString = "OracleConnectionString",
                DashbaordQueryL4 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@ and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                DashbaordModifiedOn = null
            });
        }

        private static void AddChartType()
        {
            AddChart(new ChartType
            {
                ChartName = "Bar Chart"
            });

            AddChart(new ChartType
            {
                ChartName = "Pie Chart"
            });
        }

        private static void AddDBConnection()
        {
            AddConnection(new DBConnection
            {
                DBConnectionId = 1,
                DBConnectionName = "PgAdmin4ConnectionString"
            });

            AddConnection(new DBConnection
            {
                DBConnectionId = 2,
                DBConnectionName = "OracleConnectionString"
            });

            AddConnection(new DBConnection
            {
                DBConnectionId = 3,
                DBConnectionName = "SqlConnectionString"
            });
        }
    }
}
