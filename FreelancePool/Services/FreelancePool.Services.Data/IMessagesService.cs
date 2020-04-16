namespace FreelancePool.Services.Data
{
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task Create(string content, int projectId, string authorId);

        Task<int> Delete(int id);
    }
}
