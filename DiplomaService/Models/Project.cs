namespace DiplomaService.Models;

public class Project
{
    public string Guid { get; set; }
    public string Name { get; set; }
    public string? StudentId { get; set; }
    public string? ProfessorId { get; set; }

    public static Project ToProjectModel(DiplomaService.Database.Project project)
    {
        return new Project
        {
            Guid = project.Guid,
            Name = project.Name,
            StudentId = project.StudentId,
            ProfessorId = project.ProfessorId
        };
    }
}