// Name: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;
using BCrypt.Net;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserDataAccess _usersRepo;

    public UsersController(IUserDataAccess usersRepo)
    {
        _usersRepo = usersRepo;
    }

    // User endpoints here

    // GET: api/users//

    /// <summary>
    /// Get all user.
    /// </summary>
    /// <returns>All of the user from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/users
    ///     { 
    ///     }
    /// </remarks>
    /// <response code="200">Return a list of all user</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(""), Authorize(Policy = "UserOnly")]
    [AllowAnonymous]
    public IEnumerable<UserModel> GetAllUsers()
    {
        return _usersRepo.GetUsers();
    }


    // GET: api/users/admin//

    /// <summary>
    /// Get all users admin.
    /// </summary>
    /// <returns>All of the users admin from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/users/admin
    ///     {
    ///     }
    /// </remarks>
    /// <response code="200">Return a list of users admin</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("admin"), Authorize(Policy = "AdminOnly")]
    public IEnumerable<UserModel> GetUsersAdminOnly()
    {
        return _usersRepo.GetUsersAdmin();
    }


    // GET: api/users/{id}//

    /// <summary>
    /// Get a user by Id.
    /// </summary>
    /// <param name="id">A user by id from the HTTP request.</param>
    /// <returns>A user by Id from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/users/{id}
    ///     {
    ///     }
    /// </remarks>
    /// <response code="200">Return a user by Id</response>
    /// <response code="404">If the searching user by Id is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetUser"), Authorize(Policy = "UserOnly")]
    public IActionResult GetUsersById(int id)
    {
        var userById = _usersRepo.GetUserById(id);

        if (userById == null) return NotFound();
        else return Ok(userById);
    }


    //POST: api/users//

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="newUser">A new user from the HTTP request.</param>
    /// <returns>A newly created user from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users
    ///     {
    ///         "email": "chan@gmail.com",
    ///         "lastname": "chan",
    ///         "firstname": "tith",
    ///         "password": "123456",
    ///         "role": "admin"  
    ///     }
    /// </remarks>
    /// <response code="201">Return the newly created user</response>
    /// <response code="400">If the user is null</response>
    /// <response code="409">If a user with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost(), Authorize(Policy = "AdminOnly")]
    public IActionResult AddUser(UserModel newUser)
    {
        var user = _usersRepo.GetUsers().Find(x => x.Email == newUser.Email);

        if (newUser == null) return BadRequest();
        if (user == newUser) return Conflict();

        //var hasher = new PasswordHasher<UserModel>();
        //var pwHash = hasher.HashPassword(newUser, newUser.PasswordHash);
        //var pwVerificationResult = hasher.VerifyHashedPassword(newUser, pwHash, newUser.PasswordHash);

        //With BCrypt
        var pwHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);
        //var pwVerificationResult = BCrypt.Net.BCrypt.Verify(pwHash, newUser.PasswordHash);
        var pwVerificationResult = BCrypt.Net.BCrypt.Verify(newUser.PasswordHash, pwHash);

        newUser.PasswordHash = pwHash;

        if (pwVerificationResult == true)
        {
            _usersRepo.CreateUser(newUser);
            return CreatedAtRoute("GetUser", new { Id = newUser.Id }, newUser);
        }
        else
        {
            return BadRequest();
        }
    }


    // PUT: api/users/{id}//

        /// <summary>
    /// Update a user.
    /// </summary>
    /// <param name="id">An existing user to update from the HTTP request.</param>
    /// <param name="updatedUser">An updated user from the HTTP request.</param>
    /// <returns>An updated user from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/users/{id}
    ///     {
    ///         "id": 5
    ///         "lastname": "chan",
    ///         "firstname": "tith",
    ///         "role": "admin" 
    ///     }
    /// </remarks>
    /// <response code="204">Updated user without showing its content</response>
    /// <response code="404">If the searching user by Id is not found</response>
    /// <response code="400">If the user is null</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
    public IActionResult UpdatesUser(int id, UserModel updatedUser)
    {
        var user = _usersRepo.GetUserById(id);

        if (user == null) return NotFound();
        else if (id != user.Id) return BadRequest();
        else {
            _usersRepo.UpdateUser(id, updatedUser);
            return NoContent();
        }
    }


    // DELETE: api/users/{id}//

    /// <summary>
    /// Delete a user.
    /// </summary>
    /// <param name="id">A deleted user from the HTTP request.</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/users/{id}
    ///     {   
    ///     }
    /// </remarks>
    /// <response code="204">Deleted user without showing its content</response>
    /// <response code="404">If the searching user by Id is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
    public IActionResult DeleteUsers(int id)
    {
        var user = _usersRepo.DeleteUser(id);
        if (user == null) return NotFound();

        return NoContent();
    }




    // PATCH: api/users/{id}//

    /// <summary>
    /// Update a user's email and password Only.
    /// </summary>
    /// <param name="id">An existing user's email and password to update from the HTTP request.</param>
    /// <param name="patchedUser">An updated user's email and password from the HTTP request.</param>
    /// <returns>An patched user's email and password from database.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/users/{id}
    ///     {
    ///         "id": 5
    ///         "email": chan@gmail.com
    ///         "password": chanchan123
    ///     }
    /// </remarks>
    /// <response code="204">Updated user's email and password without showing its content</response>
    /// <response code="404">If the searching user by Id is not found</response>
    /// <response code="400">If the user is null</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPatch("{id}"), Authorize(Policy = "AdminOnly")]
    public IActionResult PatchUsers(LoginModel patchedUser, int id)
    {
        var users = _usersRepo.GetUserById(id);

        if (users == null) return NotFound();
        else if (id != users.Id) return BadRequest();

        //var hasher = new PasswordHasher<LoginModel>();
        //var pwHash = hasher.HashPassword(patchedUser, patchedUser.Password);
        //var pwVerificationResult = hasher.VerifyHashedPassword(patchedUser, pwHash, patchedUser.Password);

        //With BCrypt
        var pwHash = BCrypt.Net.BCrypt.HashPassword(patchedUser.Password);
        var pwVerificationResult = BCrypt.Net.BCrypt.Verify(patchedUser.Password, pwHash);

        patchedUser.Password = pwHash;

        if (pwVerificationResult == true)
        {
            patchedUser.Password = pwHash;

            _usersRepo.PatchUser(patchedUser, id);
            return NoContent();

        }
        else
        {
            return NoContent();
        }
    }


}

