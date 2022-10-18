// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 2.1P

namespace robot_controller_api;

public class RobotCommand
{
    /// Implement <see cref="RobotCommand"> here following the task sheet requirements
    ///

    public int Id { get; set; }
    public string Name { get; set; }
    public bool MoveCommand { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string? Description { get; set; }

    public RobotCommand() { }


    public RobotCommand(int id, string name, bool isMoveCommand,
        DateTime createdDate, DateTime modifiedDate, string? description = null )
    {
        this.Id = id;
        this.Name = name;
        this.MoveCommand = isMoveCommand;
        this.CreatedDate = createdDate;
        this.ModifiedDate = modifiedDate;
        this.Description = description;

    }


}
