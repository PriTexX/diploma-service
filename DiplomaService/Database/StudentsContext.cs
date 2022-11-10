using Microsoft.EntityFrameworkCore;

namespace DiplomaService.Database;

public sealed class StudentsContext : DbContext
{
    public StudentsContext(DbContextOptions<StudentsContext> options): base(options) // TODO: удалить в релизной версии, так как нужно только для тестов
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Professor> Professors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(d => d.Project)
            .WithOne(p => p.Student)
            .HasForeignKey<Project>(d => d.StudentId);
        
        modelBuilder.Entity<Student>().HasData(new List<Student>
        {
            new ()
            {
                Guid = "1",
                FullName = "Josh",
                Group = "211-111",
                Course = 1,
                ProjectId = "13"
            },
            new ()
            {
                Guid = "2",
                FullName = "Sam",
                Group = "211-222",
                Course = 3,
                ProjectId = "7"
            },
            
            new ()
            {
                Guid = "3",
                FullName = "Georg",
                Group = "223-412",
                Course = 4,
                ProjectId = "1"
            }
        });

        modelBuilder.Entity<Professor>().HasData(new List<Professor>
        {
            new ()
            {
                Guid = "5",
                FullName = "Mr.Sponge",
            },
            new ()
            {
                Guid = "10",
                FullName = "Mr.Bob",
            }
        });

        modelBuilder.Entity<Project>().HasData(new List<Project>
        {
            new ()
            {
                Guid = "13",
                Name = "InnovationSuper",
                StudentId = "1",
                ProfessorId = "5"
            },
            new ()
            {
                Guid = "7",
                Name = "GeniusProj",
                StudentId = "2",
                ProfessorId = "5"
            },
            
            new ()
            {
                Guid = "1",
                Name = "Project with no professor",
                StudentId = "3",
                ProfessorId = null
            },
        });
    }
}