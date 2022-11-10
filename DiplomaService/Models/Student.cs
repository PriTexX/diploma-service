namespace DiplomaService.Models;

public class Student
{
    public string Guid { get; set; }
    
    public string FullName { get; set; }
    
    public string Group { get; set; }
    
    public int Course { get; set; }

    public string? ProjectId { get; set; }

    public static Student ToStudentModel(DiplomaService.Database.Student student)
    {
        return new Student
        {
            Guid = student.Guid,
            FullName = student.FullName,
            ProjectId = student.ProjectId,
            Course = student.Course,
            Group = student.Group
        };
    }
}