using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiplomaService.Controllers.Project;

public class UpdateProjectRequestModel
{
    [Required]
    public string Guid { get; set; }
    
    [DefaultValue(null)]
    public string? Name { get; set; }

    [DefaultValue(null)]
    public string? ProfessorId { get; set; }
    
    [DefaultValue(null)]
    public string? StudentId { get; set; }

    public void UpdateProject(Database.Project old)
    {
        if (Name is not null)
            old.Name = Name;

        if (ProfessorId is not null)
            old.ProfessorId = ProfessorId;

        if (StudentId is not null)
            old.StudentId = StudentId;
    }
}