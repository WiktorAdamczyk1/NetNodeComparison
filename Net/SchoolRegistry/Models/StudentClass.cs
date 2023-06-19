using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolRegistry.Models;

public class StudentClass
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string className { get; set; } = null!;

    public int numberOfStudents { get; set; } = 0;

    [BsonRepresentation(BsonType.ObjectId)]
    public string? _teacherId { get; set; }

}   