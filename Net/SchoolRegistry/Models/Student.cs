using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SchoolRegistry.Models;

public class Student
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string name { get; set; } = null!;

    public string lastName { get; set; } = null!;

    public string phone { get; set; } = null!;

    public string email { get; set; } = null!;

    public DateTime dateOfBirth { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? _classId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? _guardianId { get; set; }

}
