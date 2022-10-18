// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using System;
using Npgsql;
namespace robot_controller_api;

    public interface IUserDataAccess {

        List<UserModel> GetUsers();
        List<UserModel> GetUsersAdmin();
        UserModel? GetUserById(int id);
        UserModel? CreateUser(UserModel user);
        UserModel? UpdateUser(int id, UserModel user);
        UserModel? DeleteUser(int id);
        UserModel? PatchUser(LoginModel user, int id);
    }

    public class UserADO : IUserDataAccess 
    {
        // Connection string is usually set in a config file for the ease of change.

        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=;Database=sit331";
        
        // GET All Users (done)
        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM user", conn);
       
            using var dr = cmd.ExecuteReader();

            var id = dr.GetInt32(0);
            var email = dr.GetString(1);
            var firstName = dr.GetString(2);
            var lastName = dr.GetString(3);
            var passwordHash = dr.GetString(4);
            var createdDate = dr.GetDateTime(7);
            var modifiedDate = dr.GetDateTime(8);
            var description = dr.IsDBNull(5) ? null : dr.GetString(5);
            var role = dr.IsDBNull(6) ? null : dr.GetString(6);

            while (dr.Read())
            {
                //read values off the data reader and create a new
                //user here and then add it to the result list.
                users.Add(new UserModel(
                    id,
                    email,
                    firstName,
                    lastName,
                    passwordHash,
                    createdDate,
                    modifiedDate,
                    description,
                    role
                ));
            }
            return users;
        }

        // GET All Users which are Admin (done)
        public List<UserModel> GetUsersAdmin()
        {
            var users = new List<UserModel>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM user WHERE role = 'admin'", conn);
       
            using var dr = cmd.ExecuteReader();

            var id = dr.GetInt32(0);
            var email = dr.GetString(1);
            var firstName = dr.GetString(2);
            var lastName = dr.GetString(3);
            var passwordHash = dr.GetString(4);
            var createdDate = dr.GetDateTime(7);
            var modifiedDate = dr.GetDateTime(8);
            var description = dr.IsDBNull(5) ? null : dr.GetString(5);
            var role = dr.IsDBNull(6) ? null : dr.GetString(6);

            while (dr.Read())
            {
                users.Add(new UserModel(
                    id,
                    email,
                    firstName,
                    lastName,
                    passwordHash,
                    createdDate,
                    modifiedDate,
                    description,
                    role
                ));
            }
            return users;
        }

        // GET User by ID (done)
        public UserModel? GetUserById(int id)
        {
            UserModel? user = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM user WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
       
            using var dr = cmd.ExecuteReader();

            var userId = dr.GetInt32(0);
            var email = dr.GetString(1);
            var firstName = dr.GetString(2);
            var lastName = dr.GetString(3);
            var passwordHash = dr.GetString(4);
            var createdDate = dr.GetDateTime(7);
            var modifiedDate = dr.GetDateTime(8);
            var description = dr.IsDBNull(5) ? null : dr.GetString(5);
            var role = dr.IsDBNull(6) ? null : dr.GetString(6);

            if (dr.Read())
            {
                user = new UserModel(
                    userId,
                    email,
                    firstName,
                    lastName,
                    passwordHash,
                    createdDate,
                    modifiedDate,
                    description,
                    role
                );

            }
            return user;
        }

        // POST (or Create) User
        public UserModel? CreateUser(UserModel user)
        {
            UserModel? newUser = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO user VALUES (DEFAULT, @email, @first_name, @last_name, @password_hash, @description, @role, @created_date, @modified_date)", conn);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@first_name", user.FirstName);
            cmd.Parameters.AddWithValue("@last_name", user.LastName);
            cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@created_date", user.CreatedDate);
            cmd.Parameters.AddWithValue("@modified_date", user.ModifiedDate);
            cmd.Parameters.AddWithValue("@description", user.Description == null ? "NULL" : user.Description);
            cmd.Parameters.AddWithValue("@role", user.Role == null ? "NULL" : user.Role);

            using var dr = cmd.ExecuteReader();

            var userId = dr.GetInt32(0);
            var email = dr.GetString(1);
            var firstName = dr.GetString(2);
            var lastName = dr.GetString(3);
            var passwordHash = dr.GetString(4);
            var createdDate = dr.GetDateTime(7);
            var modifiedDate = dr.GetDateTime(8);
            var description = dr.IsDBNull(5) ? null : dr.GetString(5);
            var role = dr.IsDBNull(6) ? null : dr.GetString(6);

            if (dr.Read())
            {
                newUser = new UserModel(
                    userId,
                    email,
                    firstName,
                    lastName,
                    passwordHash,
                    createdDate,
                    modifiedDate,
                    description,
                    role
                );

            }
       
            return newUser;
        }

        // PUT (or Update) User
        public UserModel? UpdateUser(int id, UserModel user)
        {
            UserModel? updatedUser = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            //using var cmd = new NpgsqlCommand("UPDATE user SET Email = @email, FirstName = @first_name, LastName = @last_name, PasswordHash = @password_hash, Description = @description, Role = @role, CreatedDate = @created_date, ModifiedDate = @modified_date WHERE Id = @id", conn);
            using var cmd = new NpgsqlCommand("UPDATE user SET FirstName = @first_name, LastName = @last_name, Description = @description, Role = @role, CreatedDate = @created_date, ModifiedDate = @modified_date WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            //cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@first_name", user.FirstName);
            cmd.Parameters.AddWithValue("@last_name", user.LastName);
            //cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@created_date", user.CreatedDate);
            cmd.Parameters.AddWithValue("@modified_date", user.ModifiedDate);
            cmd.Parameters.AddWithValue("@description", user.Description == null ? "NULL" : user.Description);
            cmd.Parameters.AddWithValue("@role", user.Role == null ? "NULL" : user.Role);

            using var dr = cmd.ExecuteReader();

            var userId = dr.GetInt32(0);
            var email = dr.GetString(1);
            var firstName = dr.GetString(2);
            var lastName = dr.GetString(3);
            var passwordHash = dr.GetString(4);
            var createdDate = dr.GetDateTime(7);
            var modifiedDate = dr.GetDateTime(8);
            var description = dr.IsDBNull(5) ? null : dr.GetString(5);
            var role = dr.IsDBNull(6) ? null : dr.GetString(6);

            if (dr.Read())
            {
                updatedUser = new UserModel(
                    userId,
                    email,
                    firstName,
                    lastName,
                    passwordHash,
                    createdDate,
                    modifiedDate,
                    description,
                    role
                );
            }

            return updatedUser;
        }

        // DELETE User
        public UserModel? DeleteUser(int id)
        {
            UserModel? deletedUser = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM user WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();

            var userId = dr.GetInt32(0);
            var email = dr.GetString(1);
            var firstName = dr.GetString(2);
            var lastName = dr.GetString(3);
            var passwordHash = dr.GetString(4);
            var createdDate = dr.GetDateTime(7);
            var modifiedDate = dr.GetDateTime(8);
            var description = dr.IsDBNull(5) ? null : dr.GetString(5);
            var role = dr.IsDBNull(6) ? null : dr.GetString(6);

            if (dr.Read())
            {
                deletedUser = new UserModel(
                    userId,
                    email,
                    firstName,
                    lastName,
                    passwordHash,
                    createdDate,
                    modifiedDate,
                    description,
                    role
                );
            }

            return deletedUser;
        }

    // PatchUser
    public UserModel? PatchUser(LoginModel user, int id)
    {
        UserModel? patchedUser = null;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        using var cmd = new NpgsqlCommand("UPDATE user SET Email = @email,  PasswordHash = @password WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);

        using var dr = cmd.ExecuteReader();

        var userId = dr.GetInt32(0);
        var email = dr.GetString(1);
        var firstName = dr.GetString(2);
        var lastName = dr.GetString(3);
        var passwordHash = dr.GetString(4);
        var createdDate = dr.GetDateTime(7);
        var modifiedDate = dr.GetDateTime(8);
        var description = dr.IsDBNull(5) ? null : dr.GetString(5);
        var role = dr.IsDBNull(6) ? null : dr.GetString(6);

        if (dr.Read())
        {
            patchedUser = new UserModel(
                userId,
                email,
                firstName,
                lastName,
                passwordHash,
                createdDate,
                modifiedDate,
                description,
                role
            );
        }

        return patchedUser;
    }






}


