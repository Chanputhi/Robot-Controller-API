// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.1P

using System;
using Npgsql;
using robot_controller_api.Persistence;

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRepository, IRobotCommandDataAccess
    {
        private IRepository _repo => this;

        // GET All Robot Commands (done)
        public List<RobotCommand> GetRobotCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM public.robotcommand");
            return commands;
        }

        // GET All Robot Commands which Move (done)
        public List<RobotCommand> GetRobotCommandsMove()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robotcommand WHERE isMoveCommand = true ");
            return commands;
        }

        // GET All Robot Commands By Id (done)
        public RobotCommand? GetRobotCommandsById(int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                };
            var command = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robotcommand WHERE id = @id", sqlParams).Single();

            return command;
        }

        // Create (or POST) Robot Command (done)
        public RobotCommand? CreateRobotCommands(RobotCommand createdCommand)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("name", createdCommand.Name),
                    new("description", createdCommand.Description ?? (object)DBNull.Value),
                    new("ismovecommand", createdCommand.MoveCommand)
                };

            var result = _repo.ExecuteReader<RobotCommand>(
                 "INSERT INTO robotcommand VALUES(DEFAULT, @name, @description, @ismovecommand, now(), now()) RETURNING *;", sqlParams).Single();
            return result;
        }

        // Update (or PUT) Robot Command (done)
        public RobotCommand? UpdateRobotCommands(RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", updatedCommand.Id),
                    new("name", updatedCommand.Name),
                    new("description", updatedCommand.Description ?? (object)DBNull.Value),
                    new("ismovecommand", updatedCommand.MoveCommand)
                };

            var result = _repo.ExecuteReader<RobotCommand>(
                "UPDATE robotcommand SET \"Name\"=@name, description=@description, ismovecommand = @ismovecommand, modifieddate = current_timestamp WHERE id = @id RETURNING *; ", sqlParams).Single();
            return result;
        }

        // DELETE Robot Command (done)
        public RobotCommand? DeleteRobotCommands(int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                };
            var command = _repo.ExecuteReader<RobotCommand>("DELETE FROM robotcommand WHERE id = @id RETURNING *;", sqlParams).Single();

            return command;
        }

    }
}

