namespace SchoolRegistry.Models;

public class MongoDBSettings
{

    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string StudentClassesCollectionName { get; set; } = null!;
    public string StudentsCollectionName { get; set; } = null!;
    public string GuardiansCollectionName { get; set; } = null!;
    public string TeachersCollectionName { get; set; } = null!;

}