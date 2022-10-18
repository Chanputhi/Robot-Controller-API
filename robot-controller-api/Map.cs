// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 2.1P

namespace robot_controller_api;

public class Map
{
    /// Implement <see cref="Map"> here following the task sheet requirements


    public int Id { get; set; }
    public int Columns { get; set; }
    public int Rows { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public Map() { }

    public Map(int id, int columns, int rows, string name,
         DateTime createdDate, DateTime modifiedDate, string? description = null)
    {
        this.Id = id;
        this.Columns = columns;
        this.Rows = rows;
        this.Name = name;
        this.CreatedDate = createdDate;
        this.ModifiedDate = modifiedDate;
        this.Description = description;
    }

}
