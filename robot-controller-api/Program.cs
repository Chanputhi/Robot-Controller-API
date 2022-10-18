// NAME: CHANPUTHI TITH
// DEAKIN SIT331
// TASK 4.2P

using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using robot_controller_api;
using robot_controller_api.Controllers;
using robot_controller_api.Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 4.1P
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "New backend service that provides resource for the Moon Robot Simulator",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Chanputhi Tith",
            Email = "ctith@deakin.edu.au"
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

} );

// 4.2P
builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", default);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin", "user"));
});

// 3.1P
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
//builder.Services.AddScoped<IMapDataAccess, MapADO>();
// builder.Services.AddScoped<IUserDataAccess, UserADO>();

// 3.2C
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
builder.Services.AddScoped<IMapDataAccess, MapRepository>();
builder.Services.AddScoped<IUserDataAccess, UserRepository>();

var app = builder.Build();

// 4.2P Enable Authentication
app.UseAuthentication();
app.UseAuthorization();



app.UseHttpsRedirection();

app.MapControllers();

// 4.1P
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-puthi-deakin.css"));
//app.UseSwaggerUI();
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-flattop.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-feeling-blue.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-material.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-monokai.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-muted.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-newspaper.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-outline.css"));
//
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-material.css"));
//app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-puthi-deakin.css"));


app.Run();
