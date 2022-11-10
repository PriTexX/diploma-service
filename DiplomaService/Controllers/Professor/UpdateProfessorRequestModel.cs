using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiplomaService.Controllers.Professor;

public class UpdateProfessorRequestModel
{
    [Required]
    public string Guid { get; set; }
    
    [DefaultValue(null)]
    public string? FullName { get; set; }
    
    public void UpdateProfessor(Database.Professor old)
    {
        if (FullName is not null)
            old.FullName = FullName;
    }
}