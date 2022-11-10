using System.ComponentModel.DataAnnotations;
using DiplomaService.swagger;

namespace DiplomaService.Database;

public class Student : BaseModel
{
    [Key]
    [Required]
    public override string Guid { get; set; }
    
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string Group { get; set; }
    
    [Required]
    public int Course { get; set; }
    
    public string? ProjectId { get; set; }
    
    [SwaggerIgnore]
    public Project? Project { get; set; }
}