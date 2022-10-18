// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using System;
namespace robot_controller_api
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginModel() { }

        public LoginModel(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}

