using DevTest.Hubs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DevTest.DAL
{
    public class DevTestContext : DbContext 
    {
        
        public DbSet<Models.DevTest> DevTest { get; set; }

        public DevTestContext() : base("name=DefaultConnection")
        {
            
        }

        public void Commit()
        {
            SaveChanges();
        }
    }

    public class TestRepository : ITestRepository, IDisposable
    {
        private DevTestContext context;

        public TestRepository(DevTestContext context)
        {
            this.context = context;
        }

        public List<Models.DevTest> GetTests()
        {
            return context.DevTest.ToList();
        }

        public List<Models.DevTest> GetTestsMessages()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string commandText = null;

            using (var db = new DevTestContext())
            {
                var query = db.DevTest as System.Data.Entity.Infrastructure.DbQuery<Models.DevTest>;

                commandText = query.ToString();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<Models.DevTest> messages = new List<Models.DevTest>();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    connection.Open();

                    var sqlDependency = new SqlDependency(command);

                    sqlDependency.OnChange += new OnChangeEventHandler(sqlDependency_OnChange);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new Models.DevTest
                        {
                            ID = (int)reader["ID"],
                            CampaignName = (string)reader["CampaignName"],
                            AffiliateName = reader["AffiliateName"] != DBNull.Value ? (string)reader["AffiliateName"] : "",
                            Date = Convert.ToDateTime(reader["Date"]),
                            Clicks = (int)reader["Clicks"],
                            Conversions = (int)reader["Conversions"],
                            Impressions = (int)reader["Impressions"],
                        });
                    }
                }
                return messages;
            }
        }
            
        
        

        private void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                messagesHub.SendMessages();
            }
        }

        public Models.DevTest GetTestByID(int id)
        {
            return context.DevTest.Find(id);
        }

        public void InsertTest(Models.DevTest Test)
        {
            context.DevTest.Add(Test);
        }

        public void DeleteTest(int TestID)
        {
            Models.DevTest Test = context.DevTest.Find(TestID);
            context.DevTest.Remove(Test);
        }

        public void UpdateTest(Models.DevTest Test)
        {
            context.Entry(Test).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    public interface ITestRepository : IDisposable
    {
        List<Models.DevTest> GetTests();
        Models.DevTest GetTestByID(int ID);
        void InsertTest(Models.DevTest test);
        void DeleteTest(int ID);
        void UpdateTest(Models.DevTest test);
        List<Models.DevTest> GetTestsMessages();
        void Save();
    }
}