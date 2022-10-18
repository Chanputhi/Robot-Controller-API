
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace robot_controller_api.Controllers;


public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserDataAccess _usersRepo;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions>
    options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserDataAccess usersRepo) : base(options, logger, encoder, clock)
    {
        _usersRepo = usersRepo;
    }

    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");
        var authHeader = base.Request.Headers["Authorization"].ToString();
        string[] str = authHeader.Split(' ');

        if (str.Length <= 1)
        {
            return Task.FromResult(AuthenticateResult.Fail("No details added"));
        }

        // AllowAnonymous
        var endpoint = this.Context.GetEndpoint();

        // Check for [AllowAnonymous] present on method
        // this is always null and not present in Metadata?
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }


        // Authentication logic will be here.

        // Get only Base64 without Basic
        var token = authHeader.Substring("Basic ".Length).Trim();
        //var token = authHeader;

        // Decode Base64 using Convert and Encoding
        var credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        // Split email and password by :
        var credentials = credentialString.Split(':');
        var email = credentials[0];
        var password = credentials[1];

        // Get the user from the database with email from the retreived credentials
        UserModel user = _usersRepo.GetUsers().FirstOrDefault(x => x.Email == email);

        // Check if the user with email exists
        if (user == null)
        {
            Response.StatusCode = 401;
            return Task.FromResult(AuthenticateResult.Fail("User with {email} does not exist."));
        }

        // Initialize the password hasher
        //var hasher = new PasswordHasher<UserModel>();
        //var pwVerificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        // Initialize the password hasher with BCrypt
        var pwVerificationResult = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        // Check if the password is correct
        if (pwVerificationResult == true)
        {
            // Create the claims
            var claims = new []
            {
                new Claim("name", $"{user.FirstName} {user.FirstName}"),
                new Claim(ClaimTypes.Role, user.Role ?? "Unknown")
                
            };
            // Create the identity
            var identity = new ClaimsIdentity(claims, "Basic");
            // Create the principal
            var principal = new ClaimsPrincipal(identity);
            // Create the ticket
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            // Return the ticket
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        Response.StatusCode = 401;
        return Task.FromResult(AuthenticateResult.Fail("Password is incorrect."));
    }
}