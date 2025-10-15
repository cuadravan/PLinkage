using PLinkageShared.Enums;

namespace PLinkageAPI.Interfaces
{
    public interface IUser
    {
        Guid UserId { get; }
        string UserFirstName { get; }
        string UserLastName { get; }
        UserRole UserRole { get; }
        CebuLocation? UserLocation { get; }
    }
}
