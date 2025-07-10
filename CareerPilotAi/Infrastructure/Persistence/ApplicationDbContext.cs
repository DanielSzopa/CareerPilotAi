using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CareerPilotAi.Infrastructure.Persistence.DataModels;
using System.Reflection;

namespace CareerPilotAi.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    internal DbSet<JobApplicationDataModel> JobApplications { get; set; }
    internal DbSet<InterviewQuestionsSectionDataModel> InterviewQuestionsSections { get; set; }
    internal DbSet<InterviewQuestionDataModel> InterviewQuestions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}