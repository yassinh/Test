using DevTest.Hubs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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

    public class UnitOfWork : IDisposable
    {
        private DevTestContext context = new DevTestContext();
        private GenericRepository<Models.DevTest> testRepository;

        public GenericRepository<Models.DevTest> TestRepository
        {
            get
            {

                if (this.testRepository == null)
                {
                    this.testRepository = new GenericRepository<Models.DevTest>(context);
                }
                return testRepository;
            }
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

    public class GenericRepository<TEntity> where TEntity : class
    {
        internal DevTestContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DevTestContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public List<TEntity> GetTestsMessages()
        {
           

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string commandText = null;

            using (var db = new DevTestContext())
            {
                var query = db.DevTest as System.Data.Entity.Infrastructure.DbQuery<TEntity>;

                commandText = query.ToString();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<TEntity> messages = new List<TEntity>();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    connection.Open();

                    var sqlDependency = new SqlDependency(command);

                    sqlDependency.OnChange += new OnChangeEventHandler(sqlDependency_OnChange);

                    var reader = command.ExecuteReader();

                    messages = this.Get().ToList();
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
    }
    
   
}