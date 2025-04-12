namespace PLinkage.Interfaces
{
    public interface IUser
    {
        Guid UserId { get; }
        UserRole UserRole { get; }
    }
}

public enum UserRole
{
    SkillProvider,
    ProjectOwner,
    Admin
}
