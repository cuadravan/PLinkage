using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLinkageShared.Enums;

namespace PLinkageShared.DTOs
{
    public class LoginRequestDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
    }

    public class LoginResultDto
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public UserRole? UserRole { get; set; } = null;
        public string Message { get; set; } = string.Empty;
    }

    public class RegisterUserDto
    {
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public CebuLocation? UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole? UserRole { get; set; } = null;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
    }
}
