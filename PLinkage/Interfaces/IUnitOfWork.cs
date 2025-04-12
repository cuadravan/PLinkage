using PLinkage.Models;

namespace PLinkage.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Admin> Admin { get; }
        IRepository<ProjectOwner> ProjectOwner { get; }
        IRepository<SkillProvider> SkillProvider { get; }
        IRepository<Project> Projects { get; }
        IRepository<Message> Messages { get; }
        IRepository<OfferApplication> OfferApplications { get; }

        Task SaveChangesAsync();
    }
}
