using DevTest.Models;

namespace DevTest.DAL
{
    public interface IUnitOfWork
    {
        GenericRepository<Models.DevTest> TestRepository { get; }

        void Dispose();
        void Save();
    }
}