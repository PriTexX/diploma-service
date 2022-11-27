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

    public static bool operator ==(StudentResponseModel studentResponseModel, Database.Student student)
    {
        return studentResponseModel.Course == student.Course &&
               studentResponseModel.Group == student.Group &&
               studentResponseModel.Guid == student.Guid &&
               studentResponseModel.FullName == student.FullName;
    }
    
    public static bool operator !=(StudentResponseModel studentResponseModel, Database.Student student)
    {
        return !(studentResponseModel == student);
    }

    public static bool operator ==(Database.Student student, StudentResponseModel studentResponseModel)
    {
        return studentResponseModel == student;
    }

    public static bool operator !=(Database.Student student, StudentResponseModel studentResponseModel)
    {
        return studentResponseModel != student;
    }
}