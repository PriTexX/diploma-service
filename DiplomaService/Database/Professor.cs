using System.ComponentModel.DataAnnotations;
using DiplomaService.swagger;

namespace DiplomaService.Database;

public class Professor : BaseModel
{
    [Key]
    [Required]
    public override string Guid { get; set; }
    
    [Required]
    public string FullName { get; set; }
    
    [SwaggerIgnore]
    public List<Project>? Projects { get; set; }
}