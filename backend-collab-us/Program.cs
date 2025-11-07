using backend_collab_us.comment_managment.Application.Internal.QueryService;
using backend_collab_us.IAM.Application.Internal.CommandServices;
using backend_collab_us.IAM.Application.Internal.OutboundServices;
using backend_collab_us.IAM.Application.Internal.QueryServices;
using backend_collab_us.IAM.domain.repositories;
using backend_collab_us.IAM.domain.Services;
using backend_collab_us.IAM.Infrastructure.Hashing.BCrypt.Services;
using backend_collab_us.IAM.Infrastructure.Persistence.Repositories;
using backend_collab_us.IAM.Infrastructure.Token.JWT.Configuration;
using backend_collab_us.IAM.Infrastructure.Token.JWT.Services;
using backend_collab_us.profile_managment.Infrastructure.Persistence.Repositories;
using backend_collab_us.profile_managment.Application.Internal.CommandService;
using backend_collab_us.profile_managment.Application.Internal.QueryService;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.profile_managment.domain.services;
using backend_collab_us.projects.Application.Internal.CommandService;
using backend_collab_us.projects.Application.Internal.QueryService;
using backend_collab_us.projects.domain.repositories;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;
using backend_collab_us.projects.infrastructur.persistence;
using backend_collab_us.Shared.Domain.Exeptions;
using backend_collab_us.Shared.Domain.Infrastructure.Interfaces.ASP.Configuration;
using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using backend_collab_us.task_management.Application.Internal.CommandService;
using backend_collab_us.task_management.Application.Internal.QueryService;
using backend_collab_us.task_management.domain.repositories;
using backend_collab_us.task_management.domain.Services;
using backend_collab_us.task_management.Infrastructure.EFC.Persistence;
using Cortex.Mediator.Behaviors;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add ASP NET CORE MVC with kebab case route naming Convention

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));
builder.Services.AddEndpointsApiExplorer();

// Add Cors Policy
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
       ); 
});

// Add Configuration for entity framework Core

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null) throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
});

// Add Swagger/ OpenApi support

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Collab-us PlatForm API",
        Version = "v1",
        Description = "Api for the DevWeb Collab us Platform Api",
        Contact = new OpenApiContact
        {
            Name = "DevWeb team",
            Email = "contact@devweb.com"
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });
    options.EnableAnnotations();
});

// Dependency Inyection

// Shared Bounded Context

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Identity and Access Management Bounded Context
// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Commands
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
// Query Services
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
// Authentication Services
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// Profile Management 
// Repositories
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
// Commands
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<ICommentCommandService, CommentCommandService>();
// Query Services
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<ICommentQueryService, CommentQueryService>();

// Projects Bounded Context - AGREGAR TODO ESTO
// Repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
// Command Services
builder.Services.AddScoped<IProjectCommandService, ProjectCommandService>();
builder.Services.AddScoped<IApplicationCommandService, ApplicationCommandService>();
builder.Services.AddScoped<IFavoriteCommandService, FavoriteCommandService>();
// Query Services
builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();
builder.Services.AddScoped<IApplicationQueryService, ApplicationQueryService>();
builder.Services.AddScoped<IFavoriteQueryService, FavoriteQueryService>();

// Task Management Bounded Context

// Repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskSubmissionRepository, TaskSubmissionRepository>();
// Command Services
builder.Services.AddScoped<ITaskCommandService, TaskCommandService>();
builder.Services.AddScoped<ITaskSubmissionCommandService, TaskSubmissionCommandService>();

// Query Services
builder.Services.AddScoped<ITaskQueryService, TaskQueryService>();
builder.Services.AddScoped<ITaskSubmissionQueryService, TaskSubmissionQueryService>();


// Add Mediator for CQRS
builder.Services.AddCortexMediator(
    configuration: builder.Configuration,
    handlerAssemblyMarkerTypes: new []{typeof(Program)}, configure: options =>
    {
        options.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<>));
    });
    
var app = builder.Build();

// Verify if the database exists and create it if it doesn't
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    if (app.Environment.IsDevelopment())
    {
        // Recreate the database on each run during development
        context.Database.EnsureCreated();
    }

    context.Database.EnsureCreated();
}

// Use Swagger for API documentation if in development mode

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "News API v1");
    options.RoutePrefix = string.Empty;
});
// Apply Cors Policy
app.UseCors("AllowAllPolicy");
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
