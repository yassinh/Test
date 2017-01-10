using DevTest.Models;

namespace DevTest.DAL
{
    public interface IUnitOfWork<TEntity> where TEntity : class
    {
        IGenericRepository<TEntity> TestRepository { get; }

        void Dispose();
        void Save();
    }
}