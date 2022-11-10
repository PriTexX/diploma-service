namespace DiplomaService.Controllers.Student;

public class StudentResponseModel
{
    public string Guid { get; set; }
    
    public string FullName { get; set; }
    
    public string Group { get; set; }
    
    public int Course { get; set; }

    public Models.Project? Project { get; set; }
    
    public Models.Professor? Professor { get; set; }
    
    public static StudentResponseModel ToResponseModel(Database.Student student)
    {
        var project = student.Project is not null ? Models.Project.ToProjectModel(student.Project) : null;
        var professor = student.Project?.Professor is not null ? Models.Professor.ToProfessorModel(student.Project.Professor) : null;

        return new StudentResponseModel
        {
            Guid = student.Guid,
            FullName = student.FullName,
            Group = student.Group,
            Course = student.Course,
            Project = project,
            Professor = professor,
        };
    }
}