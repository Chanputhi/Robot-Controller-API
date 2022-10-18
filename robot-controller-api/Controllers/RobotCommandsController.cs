// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandsRepo = new RobotCommandADO();

    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }

    // Robot commands endpoints here

    // GET: api/robot-commands//

    /// <summary>
    /// Get all robot command.
    /// </summary>
    /// <returns>All of the robot command from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/robot-commands
    ///     { 
    ///     }
    /// </remarks>
    /// <response code="200">Return a list of all robot command</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(""), Authorize(Policy = "UserOnly")]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        return _robotCommandsRepo.GetRobotCommands();
    }


    // GET: api/robot-commands/move//

    /// <summary>
    /// Get all robot move command.
    /// </summary>
    /// <returns>All of the robot move command from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/robot-commands/move
    ///     {
    ///     }
    /// </remarks>
    /// <response code="200">Return a list of robot move command</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("move"), Authorize(Policy = "UserOnly")]
    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        var moveCommands = _robotCommandsRepo.GetRobotCommandsMove();

        return moveCommands;
    }


    // GET: api/robot-commands/{id}//

    /// <summary>
    /// Get a robot command by Id.
    /// </summary>
    /// <param name="id">A robot command by id from the HTTP request.</param>
    /// <returns>A robot command by Id from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/robot-commands/{id}
    ///     {
    ///     }
    /// </remarks>
    /// <response code="200">Return a robot command by Id</response>
    /// <response code="404">If the searching robot command by Id is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetRobotCommand"), Authorize(Policy = "UserOnly")]
    public IActionResult GetRobotCommandById(int id)
    {
        var commandById = _robotCommandsRepo.GetRobotCommandsById(id);

        if (commandById == null) return NotFound();
        else return Ok(commandById);
    }


    //POST: api/robot-commands//

    /// <summary>
    /// Create a new robot command.
    /// </summary>
    /// <param name="newCommand">A new robot command from the HTTP request.</param>
    /// <returns>A newly created robot command from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/robot-commands
    ///     {
    ///         "name": "PLAY",
    ///         "moveCommand": true,
    ///         "description": "Enjoy playing random stuff"    
    ///     }
    /// </remarks>
    /// <response code="201">Return the newly created robot</response>
    /// <response code="400">If the robot command is null</response>
    /// <response code="409">If a robot command with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost(), Authorize(Policy = "AdminOnly")]
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        var command = _robotCommandsRepo.GetRobotCommands().Find(x => x.Name == newCommand.Name);

        if (newCommand == null) return BadRequest();
        if (command == newCommand) return Conflict();

        var createdRobotCommand = _robotCommandsRepo.CreateRobotCommands(newCommand);
        if (createdRobotCommand == null) return BadRequest();
        return CreatedAtRoute("GetRobotCommand", new { id = createdRobotCommand.Id }, createdRobotCommand);
    }


    // PUT: api/robot-commands/{id}//

    /// <summary>
    /// Update a robot command.
    /// </summary>
    /// <param name="id">An existing robot command to update from the HTTP request.</param>
    /// <param name="updatedCommand">An updated robot command from the HTTP request.</param>
    /// <returns>An updated robot command from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/robot-commands/{id}
    ///     {
    ///         "id": 16
    ///         "name": "PLAY",
    ///         "moveCommand": true,
    ///         "description": "Enjoy playing random stuff"    
    ///     }
    /// </remarks>
    /// <response code="204">Updated robot command without showing its content</response>
    /// <response code="404">If the searching robot command by Id is not found</response>
    /// <response code="400">If the robot command is null</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}"), Authorize(Policy = "UserOnly")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        var command = _robotCommandsRepo.GetRobotCommandsById(id);
        if (command == null) return NotFound();
        else if (id != command.Id) return BadRequest();
        else
        {
            _robotCommandsRepo.UpdateRobotCommands(updatedCommand);

            return NoContent();
        }
    }


    // DELETE: api/robot-commands/{id}//

    /// <summary>
    /// Delete a robot command.
    /// </summary>
    /// <param name="id">A deleted robot command from the HTTP request.</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/robot-commands/{id}
    ///     {   
    ///     }
    /// </remarks>
    /// <response code="204">Deleted robot command without showing its content</response>
    /// <response code="404">If the searching robot command by Id is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteRobotCommand(int id)
    {
        var command = _robotCommandsRepo.DeleteRobotCommands(id);
        if (command == null) return NotFound();

        return NoContent();
    }



}
