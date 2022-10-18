// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using System;
using Npgsql;
using robot_controller_api.Persistence;

namespace robot_controller_api.Persistence
{
    public class UserRepository : IRepository, IUserDataAccess
    {
        private IRepository _repo => this;

        // GET All User (done)
        public List<UserModel> GetUsers()
        {
            var users = _repo.ExecuteReader<UserModel>("SELECT * FROM public.user");
            return users;
        }

        // GET All User Admin (done)
        public List<UserModel> GetUsersAdmin()
        {
            var users = _repo.ExecuteReader<UserModel>("SELECT * FROM public.user WHERE role = 'admin'");
            return users;
        }

        // GET All User By Id (done)
        public UserModel? GetUserById(int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                };
            var user = _repo.ExecuteReader<UserModel>("SELECT * FROM public.user WHERE Id = @id", sqlParams).Single();

            return user;
        }

        // Create (or POST) User
        public UserModel? CreateUser(UserModel createdUser)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("email", createdUser.Email),
                    new("first_name", createdUser.FirstName),
                    new("last_name", createdUser.LastName),
                    new("password_hash", createdUser.PasswordHash),
                    new("description", createdUser.Description ?? (object)DBNull.Value),
                    new("role", createdUser.Role ?? (object)DBNull.Value)
                };

            var result = _repo.ExecuteReader<UserModel>(
                "INSERT INTO public.user VALUES (DEFAULT, @email, @first_name, @last_name, @password_hash, @description, @role, now(), now()) RETURNING *;", sqlParams).Single();
            return result;
        }

        // Update (or PUT) User
        public UserModel? UpdateUser(int id, UserModel updatedUser)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                    //new("email", updatedUser.Email),
                    new("first_name", updatedUser.FirstName),
                    new("last_name", updatedUser.LastName),
                    //new("password_hash", updatedUser.PasswordHash),
                    new("description", updatedUser.Description ?? (object)DBNull.Value),
                    new("role", updatedUser.Role ?? (object)DBNull.Value)
                };

            var result = _repo.ExecuteReader<UserModel>(
                "UPDATE public.user SET FirstName = @first_name, LastName = @last_name, Description = @description, Role = @role, ModifiedDate = now() WHERE Id = @id RETURNING *;", sqlParams).Single();
            return result;
        }

        // DELETE Robot Command (done)
        public UserModel? DeleteUser(int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                };
            var user = _repo.ExecuteReader<UserModel>("DELETE FROM public.user WHERE Id = @id RETURNING *;", sqlParams).Single();

            return user;
        }


        // PATCH (or PUT) User's Email and Password ONLY
        public UserModel? PatchUser(LoginModel patchedUser, int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                    new("email", patchedUser.Email),
                    new("password", patchedUser.Password),
                };

        var result = _repo.ExecuteReader<UserModel>(
            "UPDATE public.user SET Email = @email,  PasswordHash = @password, ModifiedDate = now() WHERE Id = @id RETURNING *;", sqlParams).Single();
            return result;
        }



}
}