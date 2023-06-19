using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SchoolRegistry.Models;

public class Guardian
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string name { get; set; } = null!;

    public string lastName { get; set; } = null!;

    public string phone { get; set; } = null!;

    public string email { get; set; } = null!;

}
