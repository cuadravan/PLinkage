using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PLinkageAPI.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Entities
{
    public class SkillProvider: IUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public CebuLocation? UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.SkillProvider;
        public string UserStatus { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.String)]
        public List<Guid> OfferApplicationId { get; set; } = new List<Guid>();
        public List<Education> Educations { get; set; } = new List<Education>();
        public List<Skill> Skills { get; set; } = new List<Skill>();

        [BsonRepresentation(BsonType.String)]
        public List <Guid> EmployedProjects { get; set; } = new List<Guid>();
        public double UserRating { get; set; } = 0.0;
        public double UserRatingTotal { get; set; } = 0.0;
        public int UserRatingCount { get; set; } = 0;
        public double TempRating { get; set; } = 0.0;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        public List<Guid> UserMessagesId { get; set; } = new List<Guid>();

        public void AddEducation(Education educationToAdd)
        {
            this.Educations.Add(educationToAdd);
        }
        
        public bool UpdateEducation(int indexToUpdate, Education updatedEducation)
        {
            if(indexToUpdate >= this.Educations.Count())
            {
                return false;
            }
            this.Educations[indexToUpdate] = updatedEducation;
            return true;
        }

        public bool DeleteEducation(int indexToDelete)
        {
            if (indexToDelete >= this.Educations.Count())
            {
                return false;
            }
            this.Educations.RemoveAt(indexToDelete);
            return true;
        }

        public void AddSkill(Skill skillToAdd)
        {
            this.Skills.Add(skillToAdd);
        }

        public bool UpdateSkill(int indexToUpdate, Skill updatedSkill)
        {
            if (indexToUpdate >= this.Skills.Count())
            {
                return false;
            }
            this.Skills[indexToUpdate] = updatedSkill;
            return true;
        }

        public bool DeleteSkill(int indexToDelete)
        {
            if (indexToDelete >= this.Skills.Count())
            {
                return false;
            }
            this.Skills.RemoveAt(indexToDelete);
            return true;
        }

        public void UpdateProfile(UserProfileUpdateDto skillProviderUpdateDto)
        {
            this.UserFirstName = skillProviderUpdateDto.UserFirstName;
            this.UserLastName = skillProviderUpdateDto.UserLastName;
            this.UserPhone = skillProviderUpdateDto.UserPhone;
            this.UserLocation = skillProviderUpdateDto.UserLocation;
            this.UserBirthDate = skillProviderUpdateDto.UserBirthDate;
            this.UserLocation = skillProviderUpdateDto.UserLocation;
        }

        //// This is a method that returns a location 
        //// If there is UserLocation, it calls the static factory method which returns a Location object
        //// This is done to reduce massive overhauls of existing database
        //[BsonIgnore]
        //public Location? LocationObject =>
        //UserLocation.HasValue ? Location.From(UserLocation.Value) : null;
    }

    public class Skill
    {
        public string SkillName { get; set; } = string.Empty;
        public string SkillDescription { get; set; } = string.Empty;
        public int SkillLevel { get; set; } // 0-5
        public DateTime TimeAcquired { get; set; } = DateTime.Now;
        public string OrganizationInvolved { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
    }

    public class Education
    {
        public string CourseName { get; set; } = string.Empty;
        public string SchoolAttended { get; set; } = string.Empty;
        public DateTime TimeGraduated { get; set; } = DateTime.Now;
    }
}
