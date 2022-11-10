using System.ComponentModel.DataAnnotations;
using DiplomaService.swagger;

namespace DiplomaService.Database;

public class Project : BaseModel
{
    [Key]
    [Required]
    public override string Guid { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string? StudentId { get; set; }
    
    [SwaggerIgnore]
    public Student? Student { get; set; }
    
    public string? ProfessorId { get; set; }
    
    [SwaggerIgnore]
    public Professor? Professor { get; set; }
}