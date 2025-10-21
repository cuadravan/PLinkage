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
        List<Guid> OfferApplicationId { get; }
        List<Guid> UserMessagesId { get; }
        void AddOfferApplication(Guid guid);

        bool AddChat(Guid chatId);
    }
}
