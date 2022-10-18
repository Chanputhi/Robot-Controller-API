// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    private readonly IMapDataAccess _mapsRepo = new MapADO();

    public MapsController(IMapDataAccess mapsRepo)
    {
        _mapsRepo = mapsRepo;
    }
    // Map Endpoints here

    // GET: api/maps //

    /// <summary>
    /// Get all map.
    /// </summary>
    /// <returns>All of the map from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/maps
    ///     {
    ///     }
    /// </remarks>
    /// <response code="200">Return a list of all map</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(), Authorize(Policy = "UserOnly")]
    public IEnumerable<Map> GetAllMaps() { return _mapsRepo.GetMaps(); }


    //GET: api/maps/square //

    /// <summary>
    /// Get all square map.
    /// </summary>
    /// <returns>All of the square map from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/maps/square
    ///     {  
    ///     }
    /// </remarks>
    /// <response code="200">Return a list of all square map</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("square"), Authorize(Policy = "UserOnly")]
    public IEnumerable<Map> GetSquareMapsOnly()
    {
        var squareMap = _mapsRepo.GetMaps().FindAll(x => x.Columns == x.Rows);

        return squareMap;
    }


    // GET: api/maps/{id} //

    /// <summary>
    /// Get all map by id.
    /// </summary>
    /// <param name="id">A map by id from the HTTP request.</param>
    /// <returns>A single map from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/maps/{id}
    ///     {   
    ///     }
    /// </remarks>
    /// <response code="200">Return a single map by provided Id</response>
    /// <response code="404">If the map by Id is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetMap"), Authorize(Policy = "UserOnly")]
    public IActionResult GetMapById(int id)
    {
        var mapById = _mapsRepo.GetMapById(id);

        if (mapById == null) return NotFound();
        else return Ok(mapById);
    }


    // POST: api/maps //

    /// <summary>
    /// Create a new map.
    /// </summary>
    /// <param name="newMap">A new map from the HTTP request.</param>
    /// <returns>A newly created single map from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/maps
    ///     {
    ///         "columns": 25,
    ///         "rows": 25,
    ///         "name": "Medium 25x25 Map",
    ///         "description": "The meduim square map"    
    ///     }
    /// </remarks>
    /// <response code="201">Return the newly created map</response>
    /// <response code="400">If the map is null</response>
    /// <response code="409">If a map with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost(), Authorize(Policy = "AdminOnly")]
    public IActionResult AddMap(Map newMap)
    {
        var map = _mapsRepo.GetMaps().Find(x => x.Name == newMap.Name);

        if (newMap == null) return BadRequest();
        if (map == newMap) return Conflict();

        _mapsRepo.CreateMaps(newMap);
        return CreatedAtRoute("GetRobotCommand", new { id = newMap.Id }, newMap);
    }


    // PUT: api/maps/{id} //

    /// <summary>
    /// Update an existing map.
    /// </summary>
    /// <param name="id">An existing map to update from the HTTP request.</param>
    /// <param name="updatedMap">An updated map from the HTTP request.</param>
    /// <returns>An updated map from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/maps/{id}
    ///     {
    ///         "id": 5,
    ///         "columns": 25,
    ///         "rows": 25,
    ///         "name": "Medium 25x25 Map",
    ///         "description": "The meduim square map"    
    ///     }
    /// </remarks>
    /// <response code="204">Updated map without showing its content</response>
    /// <response code="404">If the searching map by Id is not found</response>
    /// <response code="400">If the map is null</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        var map = _mapsRepo.GetMapById(id);
        if (map == null) return NotFound();
        else if (id != map.Id) return BadRequest();
        else
        {
            _mapsRepo.UpdateMaps(updatedMap);

            return NoContent();
        }
    }

    // DELETE: api/maps/{id} //

    /// <summary>
    /// Delete a map.
    /// </summary>
    /// <param name="id">A deleted map from the HTTP request.</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/maps/{id}
    ///     {   
    ///     }
    /// </remarks>
    /// <response code="204">Deleted map without showing its content</response>
    /// <response code="404">If the searching map by Id is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteMap(int id)
    {
        var map = _mapsRepo.GetMapById(id);
        if (map == null) return NotFound();

        _mapsRepo.DeleteMaps(id);

        return NoContent();
    }


    // GET: api/maps/{id}/{x}-{y} //

    /// <summary>
    /// Check the coordinate of robot within map by id.
    /// </summary>
    /// <param name="id">A chosen map by id to test robot coordinate from the HTTP request.</param>
    /// <param name="x">A x coordinate of robot within map by id from the HTTP request.</param>
    /// <param name="y">A y coordinate of robot within map by id from the HTTP request.</param>
    /// <returns>A result of robot whether it is True or False from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/maps/{id}/{x}-{y}
    ///     {    
    ///     }
    /// </remarks>
    /// <response code="200">Return a result of robot on map whether True or False</response>
    /// <response code="404">If the map by Id is not found</response>
    /// <response code="400">If the map is null</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{id}/{x}-{y}"), Authorize(Policy = "UserOnly")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        var map = _mapsRepo.GetMapById(id);

        if(x < 0 || y < 0) { return BadRequest(); }
        if (map == null) return NotFound();

        bool isOnMap = false;

        if (x <= 0 || x > map.Columns || y <= 0 || y > map.Rows)
        {
            isOnMap = false;
        }
        else
        {
            isOnMap = true;
        }

        return Ok(isOnMap);
       
    }
}
