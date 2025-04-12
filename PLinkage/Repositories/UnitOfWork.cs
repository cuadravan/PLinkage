using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Admin> Admin { get; }
        public IRepository<ProjectOwner> ProjectOwner { get; }
        public IRepository<SkillProvider> SkillProvider { get; }
        public IRepository<Project> Projects { get; }
        public IRepository<Message> Messages { get; }
        public IRepository<OfferApplication> OfferApplications { get; }

        public UnitOfWork()
        {
            Admin = new JsonRepository<Admin>("Admin");
            ProjectOwner = new JsonRepository<ProjectOwner>("ProjectOwner");
            SkillProvider = new JsonRepository<SkillProvider>("SkillProvider");
            Projects = new JsonRepository<Project>("Projects");
            Messages = new JsonRepository<Message>("Messages");
            OfferApplications = new JsonRepository<OfferApplication>("OfferApplications");
        }

        public async Task SaveChangesAsync()
        {
            await Admin.SaveChangesAsync();
            await ProjectOwner.SaveChangesAsync();
            await SkillProvider.SaveChangesAsync();
            await Projects.SaveChangesAsync();
            await Messages.SaveChangesAsync();
            await OfferApplications.SaveChangesAsync();
        }

        public async Task ReloadAsync()
        {
            Admin.Reload();
            ProjectOwner.Reload();
            SkillProvider.Reload();
            Projects.Reload();
            Messages.Reload();
            OfferApplications.Reload();

            await Task.CompletedTask;
        }
    }
}
