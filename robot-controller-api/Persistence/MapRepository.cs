// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.1P

using System;
using Npgsql;
using robot_controller_api.Persistence;


namespace robot_controller_api.Persistence
{
    public class MapRepository : IRepository, IMapDataAccess
    {
        private IRepository _repo => this;

        // GET All Maps (done)
        public List<Map> GetMaps()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM public.map");
            return maps;
        }

        // GET All Map By Id (done)
        public Map? GetMapById(int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                };
            var map = _repo.ExecuteReader<Map>("SELECT * FROM map WHERE id = @id", sqlParams).Single();

            return map;
        }

        // Create (or POST) Maps (done)
        public Map? CreateMaps(Map createdMap)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("column", createdMap.Columns),
                    new("row", createdMap.Rows),
                    new("name", createdMap.Name),
                    new("description", createdMap.Description ?? (object)DBNull.Value),
                };

            var result = _repo.ExecuteReader<Map>(
                 "INSERT INTO map VALUES(DEFAULT, @column, @row, @name, @description, now(), now()) RETURNING *;", sqlParams).Single();
            return result;
        }

        // Update (or PUT) Maps (done)
        public Map? UpdateMaps(Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", updatedMap.Id),
                    new("column", updatedMap.Columns),
                    new("row", updatedMap.Rows),
                    new("name", updatedMap.Name),
                    new("description", updatedMap.Description ?? (object)DBNull.Value),
                };

            var result = _repo.ExecuteReader<Map>(
                "UPDATE map SET \"Name\"=@name, columns=@column, rows=@row, description=@description, modifieddate = current_timestamp WHERE id = @id RETURNING *;", sqlParams).Single();
            return result;
        }

        // DELETE Maps by Id (done)
        public Map? DeleteMaps(int id)
        {
            var sqlParams = new NpgsqlParameter[]{
                    new("id", id),
                };
            var command = _repo.ExecuteReader<Map>("DELETE FROM map WHERE id = @id RETURNING *;", sqlParams).Single();

            return command;
        }

    }







}

