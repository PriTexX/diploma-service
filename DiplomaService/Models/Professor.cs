namespace DiplomaService.Models;

public class Professor
{
    public string Guid { get; set; }
    public string FullName { get; set; }

    public static Professor ToProfessorModel(DiplomaService.Database.Professor professor)
    {
        return new Professor
        {
            Guid = professor.Guid,
            FullName = professor.FullName
        };
    }
}