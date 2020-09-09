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
                UserId = "ADMIN",
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
                UserId = "CLIENT",
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
                Status = "Authorised",
                UserEmail = "jayanthnadig@gmail.com"
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
                Status = "Authorised",
                UserEmail = "jayanthnadig@gmail.com"
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
                Status = "Unauthorised",
                UserEmail = "santhuanandmca@gmail.com"
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
                Status = "Unauthorised",
                UserEmail = "jayanthnadig@gmail.com"
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
                Status = "Authorised",
                UserEmail = "santhuanandmca@gmail.com"
            });
        }

        private static void AddNewUserDashboard()
        {
            AddUserDashboard(new UserDashboard
            {
                //DashboardUserId = "ADMIN",
                CreatedBy = "ADMIN",
                DashboardChartType = "Bar Chart",
                DashboardWidgetName = "My Bar Chart",
                DashboardUserPermission = "ADMIN,CLIENT",
                DashboardEmailFormat = "Excel",
                WidgetConnectionString = "PgAdmin4ConnectionString",
                WidgetSchedulerType = "SchedulerName1",
                WidgetSchedulerEmailIDs = "jayanthnadig@gmail.com;santhuanandmca@gmail.com",
                WidgetQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                Level1ConnectionString = "SqlConnectionString",
                Level1SchedulerType = "SchedulerName1",
                L1SchedulerEmailIDs = "jayanthnadig@gmail.com;santhuanandmca@gmail.com",
                DashbaordQueryL1 = "select * from public.\"Transactions\" where status='@status@'",
                Level2ConnectionString = "OracleConnectionString",
                Level2SchedulerType = "SchedulerName1",
                L2SchedulerEmailIDs = "jayanthnadig@gmail.com;santhuanandmca@gmail.com",
                DashbaordQueryL2 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@'",
                Level3ConnectionString = "PgAdmin4ConnectionString",
                Level3SchedulerType = "SchedulerName1",
                L3SchedulerEmailIDs = "jayanthnadig@gmail.com;santhuanandmca@gmail.com",
                DashbaordQueryL3 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@' and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                Level4ConnectionString = "OracleConnectionString",
                Level4SchedulerType = "SchedulerName1",
                L4SchedulerEmailIDs = "jayanthnadig@gmail.com;santhuanandmca@gmail.com",
                DashbaordQueryL4 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@' and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                WidgetSendEmail = true
            });

            AddUserDashboard(new UserDashboard
            {
                //DashboardUserId = "ADMIN",
                CreatedBy = "ADMIN",
                DashboardChartType = "Pie Chart",
                DashboardWidgetName = "My Pie Chart",
                DashboardUserPermission = "ADMIN",
                DashboardEmailFormat = "PDF",
                WidgetConnectionString = "PgAdmin4ConnectionString",
                WidgetSchedulerType = "SchedulerName2",
                WidgetSchedulerEmailIDs = "jayanthnadig@gmail.com",
                WidgetQuery = "select t1.status as Name, COUNT(t1.status) as Count from public.\"Transactions\" t1 join public.\"Transactions\"  t2 on t1.Id = t2.Id group by t1.status",
                Level1ConnectionString = "SqlConnectionString",
                Level1SchedulerType = "SchedulerName2",
                L1SchedulerEmailIDs = "jayanthnadig@gmail.com",
                DashbaordQueryL1 = "select * from public.\"Transactions\" where status='@status@'",
                Level2ConnectionString = "OracleConnectionString",
                Level2SchedulerType = "SchedulerName2",
                L2SchedulerEmailIDs = "jayanthnadig@gmail.com",
                DashbaordQueryL2 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@'",
                Level3ConnectionString = "PgAdmin4ConnectionString",
                Level3SchedulerType = "SchedulerName2",
                L3SchedulerEmailIDs = "jayanthnadig@gmail.com",
                DashbaordQueryL3 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@' and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                Level4ConnectionString = "OracleConnectionString",
                Level4SchedulerType = "SchedulerName2",
                L4SchedulerEmailIDs = "jayanthnadig@gmail.com",
                DashbaordQueryL4 = "select * from public.\"Transactions\" where status='@status@' and assignedto='@assignedto@' and transaction_status='@transaction_status@' and branchname='@branchname@' and makerid='@makerid@' and functionid='@functionid@'",
                WidgetSendEmail = false
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
