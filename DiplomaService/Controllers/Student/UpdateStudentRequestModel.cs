using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiplomaService.Controllers.Student;

public class UpdateStudentRequestModel
{
    [Required]
    public string Guid { get; set; }
    
    [DefaultValue(null)]
    public string? FullName { get; set; }
    
    [DefaultValue(null)]
    public string? Group { get; set; }
    
    [DefaultValue(null)]
    public int? Course { get; set; }
    
    [DefaultValue(null)]
    public string? ProjectId { get; set; }

    public void UpdateStudent(Database.Student old)
    {
        if (FullName is not null)
            old.FullName = FullName;
        
        if (Course is not null)
            old.Course = (int)Course;
        
        if (Group is not null)
            old.Group = Group;
        
        if (ProjectId is not null)
            old.ProjectId = ProjectId;
    }
}