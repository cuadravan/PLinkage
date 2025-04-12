using PLinkage.Interfaces;

namespace PLinkage.Models
{
    public class Admin: IUser
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserLocation { get; set; } = string.Empty;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.Admin;
        public string UserStatus { get; set; } = string.Empty;
    }
}
