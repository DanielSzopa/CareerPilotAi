using Microsoft.AspNetCore.Identity;

namespace CareerPilotAi.Infrastructure.Persistence.Seeders;

public class E2EUsersSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<E2EUsersSeeder> _logger;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly List<UserDto> _users = new List<UserDto>{
            new UserDto("test@example.com", "test@example.com", "Test1234!@#"),
        };

    public E2EUsersSeeder(ApplicationDbContext dbContext, ILogger<E2EUsersSeeder> logger, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task Seed(ApplicationDbContext context)
    {
        foreach (var user in _users)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                _logger.LogInformation("User {Email} already exists, skipping seeding", user.Email);
                continue;
            }

            var newUser = new IdentityUser { Email = user.Email, UserName = user.UserName, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Seeded user {Email} created successfully", user.Email);
            }
            else
            {
                _logger.LogError("Error creating user {Email}: {Errors}", user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }


    public record UserDto(string Email, string UserName, string Password);
}


