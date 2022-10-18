// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 3.2C

using Npgsql;
    namespace robot_controller_api.Persistence;

    public interface IRobotCommandDataAccess
    {
        List<RobotCommand> GetRobotCommands();
        List<RobotCommand> GetRobotCommandsMove();
        RobotCommand? GetRobotCommandsById(int id);
        RobotCommand? CreateRobotCommands(RobotCommand robot);
        RobotCommand? UpdateRobotCommands(RobotCommand robot);
        RobotCommand? DeleteRobotCommands(int id);
    }

    public class RobotCommandADO : IRobotCommandDataAccess
    {
        // Connection string is usually set in a config file for the ease
        //of change.

        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=;Database=sit331";


        // GET All Robot Commands (done)
        public List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
       
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //read values off the data reader and create a new
                //robotCommand here and then add it to the result list.
                robotCommands.Add(new RobotCommand(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetBoolean(3),
                    dr.GetDateTime(4),
                    dr.GetDateTime(5),
                    dr.IsDBNull(2) ? null : dr.GetString(2)
                ));
            }

            return robotCommands;
        }

        // GET All Robot Commands which Move (done)
        public List<RobotCommand> GetRobotCommandsMove()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE isMoveCommand = true ", conn);
       
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                robotCommands.Add(new RobotCommand(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetBoolean(3),
                    dr.GetDateTime(4),
                    dr.GetDateTime(5),
                    dr.IsDBNull(2) ? null : dr.GetString(2)
                ));
            }

            return robotCommands;
        }


        // GET All Robot Commands By Id (done)
        public RobotCommand? GetRobotCommandsById(int id)
        {
            RobotCommand? robotCommands = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
       
            using var dr = cmd.ExecuteReader();

            // read 1st result if it exists
            if (dr.Read())
            {
                robotCommands = new RobotCommand(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetBoolean(3),
                    dr.GetDateTime(4),
                    dr.GetDateTime(5),
                    dr.IsDBNull(2) ? null : dr.GetString(2)
                );
            }

            return robotCommands;
        }


        // Create (or POST) Robot Command (done)
        public RobotCommand? CreateRobotCommands(RobotCommand robot)
        {
            RobotCommand? robotCommands = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand
                    ($"INSERT INTO robotcommand VALUES(DEFAULT, @name, @description, @isMoveCommand, @createdDate, @modifiedDate)", conn);

            cmd.Parameters.AddWithValue("@name", robot.Name);
            cmd.Parameters.AddWithValue("@isMoveCommand", robot.MoveCommand);
            cmd.Parameters.AddWithValue("@createdDate", robot.CreatedDate);
            cmd.Parameters.AddWithValue("@modifiedDate", robot.ModifiedDate);
            cmd.Parameters.AddWithValue("@description", robot.Description == null ? "NULL" : robot.Description);

            using var dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    robotCommands = new RobotCommand(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetBoolean(3),
                        dr.GetDateTime(4),
                        dr.GetDateTime(5),
                        dr.IsDBNull(2) ? null : dr.GetString(2)
                    );
                }

            return robotCommands;
        }


        // Update (or PUT) Robot Command (done)
        public RobotCommand? UpdateRobotCommands(RobotCommand robot)
        {
            RobotCommand? robotCommands = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            // add methods to UPDATE records from the database to data access class

            var commandString = "UPDATE robotcommand SET " +
            $"\"Name\"='{robot.Name}', description='{robot.Description}', isMoveCommand='{robot.MoveCommand}', createdDate='{robot.CreatedDate}', modifiedDate='{robot.ModifiedDate}' WHERE id='{robot.Id}'";

            using var cmd = new NpgsqlCommand(commandString, conn);

            using var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                robotCommands = new RobotCommand(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetBoolean(3),
                    dr.GetDateTime(4),
                    dr.GetDateTime(5),
                    dr.IsDBNull(2) ? null : dr.GetString(2)
                );
            }

            return robotCommands;
        }

        // DELETE Robot Command (done)
        public RobotCommand? DeleteRobotCommands(int id)
        {
            RobotCommand? robotCommands = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            // add methods to DELETE records from the database to data access class

            using var cmd = new NpgsqlCommand
            ("DELETE FROM robotcommand WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                robotCommands = new RobotCommand(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetBoolean(3),
                    dr.GetDateTime(4),
                    dr.GetDateTime(5),
                    dr.IsDBNull(2) ? null : dr.GetString(2)
                );
            }

            return robotCommands;
        }

}