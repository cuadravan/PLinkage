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

        public UnitOfWork(
            IRepository<Admin> admin,
            IRepository<ProjectOwner> projectOwner,
            IRepository<SkillProvider> skillProvider,
            IRepository<Project> projects,
            IRepository<Message> messages,
            IRepository<OfferApplication> offerApplications)
        {
            Admin = admin;
            ProjectOwner = projectOwner;
            SkillProvider = skillProvider;
            Projects = projects;
            Messages = messages;
            OfferApplications = offerApplications;
        }

        public async Task SaveChangesAsync()
        {
            // Optional: If your repos buffer changes, commit here.
            // For now, call SaveChangesAsync on each in case they need it.
            await Admin.SaveChangesAsync();
            await SkillProvider.SaveChangesAsync();
            await ProjectOwner.SaveChangesAsync();
            await Projects.SaveChangesAsync();
            await Messages.SaveChangesAsync();
            await OfferApplications.SaveChangesAsync();
        }

        public async Task ReloadAsync()
        {
            Admin.Reload();
            SkillProvider.Reload();
            ProjectOwner.Reload();
            Projects.Reload();
            Messages.Reload();
            OfferApplications.Reload();

            await Task.CompletedTask;
        }

    }
}
