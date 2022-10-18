// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using System;
namespace robot_controller_api
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string? Description { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public UserModel() { }

        public UserModel(int Id, string Email, string FirstName, string LastName,
            string PasswordHash, DateTime CreatedDate, DateTime ModifiedDate,
            string? Description = null, string? Role = null)
        {
            this.Id = Id;
            this.Email = Email;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PasswordHash = PasswordHash;
            this.CreatedDate = CreatedDate;
            this.ModifiedDate = ModifiedDate;
            this.Description = Description;
            this.Role = Role;
        }



    }
}

