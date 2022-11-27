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
    
    public static bool operator ==(Project current,Project other)
    {
        return current.Name == other.Name &&
               current.StudentId == other.StudentId &&
               current.Guid == other.Guid &&
               current.ProfessorId == other.ProfessorId;
    }

    public static bool operator !=(Project current, Project other)
    {
        return !(current == other);
    }
}