namespace DiplomaService.Controllers.Project;

public class ProjectResponseModel
{
    public string Guid { get; set; }
    
    public string Name { get; set; }
    
    public Models.Student? Student { get; set; }
    
    public Models.Professor? Professor { get; set; }

    public static ProjectResponseModel ToResponseModel(Database.Project project)
    {
        return new ProjectResponseModel
        {
            Guid = project.Guid,
            Name = project.Name,
            Student = project.Student is not null ? Models.Student.ToStudentModel(project.Student) : null,
            Professor = project.Professor is not null ? Models.Professor.ToProfessorModel(project.Professor) : null
        };
    }
    
    public static bool operator ==(ProjectResponseModel projectResponseModel, Database.Project project)
    {
        return projectResponseModel.Guid == project.Guid &&
               projectResponseModel.Name == project.Name;
    }
    
    public static bool operator !=(ProjectResponseModel projectResponseModel, Database.Project project)
    {
        return !(projectResponseModel == project);
    }

    public static bool operator ==(Database.Project project, ProjectResponseModel projectResponseModel)
    {
        return projectResponseModel == project;
    }

    public static bool operator !=(Database.Project project, ProjectResponseModel projectResponseModel)
    {
        return projectResponseModel != project;
    }
}