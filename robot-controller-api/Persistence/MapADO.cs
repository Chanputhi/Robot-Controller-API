// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 3.2C

using Npgsql;
    namespace robot_controller_api.Persistence;

    public interface IMapDataAccess
    {
        List<Map> GetMaps();
        Map? GetMapById(int id);
        Map? CreateMaps(Map map);
        Map? UpdateMaps(Map map);
        Map? DeleteMaps(int id);
    }   

    public class MapADO: IMapDataAccess
{
        // Connection string is usually set in a config file for the ease
        //of change.
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=;Database=sit331";

        // GET All Maps (done)
        public List<Map> GetMaps()
        {
            var maps = new List<Map>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
       
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //read values off the data reader and create a new
                //robotCommand here and then add it to the result list.
                maps.Add(new Map(
                    dr.GetInt32(0),
                    dr.GetInt32(1),
                    dr.GetInt32(2),
                    dr.GetString(3),
                    dr.GetDateTime(5),
                    dr.GetDateTime(6),
                    dr.IsDBNull(4) ? null : dr.GetString(4)
                ));

            }

            return maps;
        }


        // GET All Map By Id (done)
        public Map? GetMapById(int id)
        {
            Map? maps = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();

            // read 1st result if it exists
            if (dr.Read())
            {
                maps = new Map(
                    dr.GetInt32(0),
                    dr.GetInt32(1),
                    dr.GetInt32(2),
                    dr.GetString(3),
                    dr.GetDateTime(5),
                    dr.GetDateTime(6),
                    dr.IsDBNull(4) ? null : dr.GetString(4)
                );
            }

            return maps;
        }


    // Create (or POST) Maps (done)
    public Map? CreateMaps(Map map)
    {
        Map? result = null;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // add methods to INSERT records from the database to data access class
        // Method 1
        //using var cmd = new NpgsqlCommand
        //($"INSERT INTO map VALUES(DEFAULT, {map.Columns}, {map.Rows}, {map.Name}, NULL, {map.CreatedDate}, {map.ModifiedDate})", conn);

        // Method 2
        using var cmd = new NpgsqlCommand
        ($"INSERT INTO map VALUES(DEFAULT, @column, @row, @name, NULL, @createdDate, @modifiedDate)", conn);

        cmd.Parameters.AddWithValue("@column", map.Columns);
        cmd.Parameters.AddWithValue("@row", map.Rows);
        cmd.Parameters.AddWithValue("@name", map.Name);
        cmd.Parameters.AddWithValue("@createdDate", map.CreatedDate);
        cmd.Parameters.AddWithValue("@modifiedDate", map.ModifiedDate);
        cmd.Parameters.AddWithValue("@description", map.Description == null ? "NULL" : map.Description);

        using var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            result = new Map(
                dr.GetInt32(0),
                dr.GetInt32(1),
                dr.GetInt32(2),
                dr.GetString(3),
                dr.GetDateTime(5),
                dr.GetDateTime(6),
                dr.IsDBNull(4) ? null : dr.GetString(4)
            );
        }

        return result;
    }

    // Update (or PUT) Maps (done)
    public Map? UpdateMaps(Map map)
    {
        Map? result = null;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // add methods to UPDATE records from the database to data access class
        var commandString = "UPDATE map SET " +
        $"columns='{map.Columns}', rows='{map.Rows}', \"Name\"='{map.Name}', description='{map.Description}', createdDate='{map.CreatedDate}', modifiedDate='{map.ModifiedDate}' WHERE id='{map.Id}'";

        using var cmd = new NpgsqlCommand(commandString, conn);

        using var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            result = new Map(
                dr.GetInt32(0),
                dr.GetInt32(1),
                dr.GetInt32(2),
                dr.GetString(3),
                dr.GetDateTime(5),
                dr.GetDateTime(6),
                dr.IsDBNull(4) ? null : dr.GetString(4)
        );
        }

        return result;
    }


    // DELETE Maps by Id (done)
    public Map? DeleteMaps(int id)
    {
        Map? maps = null;

        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        // add methods to DELETE records from the database to data access class
        using var cmd = new NpgsqlCommand
        ("DELETE FROM map WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            maps = new Map(
                dr.GetInt32(0),
                dr.GetInt32(1),
                dr.GetInt32(2),
                dr.GetString(3),
                dr.GetDateTime(5),
                dr.GetDateTime(6),
                dr.IsDBNull(4) ? null : dr.GetString(4)
            );
        }

        return maps;
    }


}