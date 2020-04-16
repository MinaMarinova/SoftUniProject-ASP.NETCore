namespace FreelancePool.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task Create(string content, int projectId, string authorId)
        {
            var message = new Message
            {
                Content = content,
                ProjectId = projectId,
                AuthorId = authorId,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var messageToDelete = this.messagesRepository.All().Where(m => m.Id == id).FirstOrDefault();

            messageToDelete.IsDeleted = true;

            await this.messagesRepository.SaveChangesAsync();

            return messageToDelete.ProjectId;
        }
    }
}
