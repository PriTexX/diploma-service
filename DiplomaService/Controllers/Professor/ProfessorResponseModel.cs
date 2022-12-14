namespace DiplomaService.Controllers.Professor;

public class ProfessorResponseModel
{
    public string Guid { get; set; }
    public string FullName { get; set; }
    public List<Models.Project>? Projects { get; set; }
    public List<Models.Student?>? Students { get; set; }

    public static ProfessorResponseModel ToResponseModel(Database.Professor professor)
    {
        var students = professor.Projects?.Select(s => 
            s.Student is not null 
                ? Models.Student.ToStudentModel(s.Student) 
                : null
        );
        var projects = professor.Projects?.Select(Models.Project.ToProjectModel);

        return new ProfessorResponseModel
        {
            Guid = professor.Guid,
            FullName = professor.FullName,
            Students = students?.ToList(),
            Projects = projects?.ToList()
        };
    }
}