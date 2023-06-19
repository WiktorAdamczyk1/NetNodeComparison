using SchoolRegistry.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SchoolRegistry.Services;

public class MongoDBService
{

    private readonly IMongoCollection<StudentClass> _studentClassCollection;
    private readonly IMongoCollection<Student> _studentCollection;
    private readonly IMongoCollection<Guardian> _guardianCollection;
    private readonly IMongoCollection<Teacher> _teacherCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _studentClassCollection = database.GetCollection<StudentClass>(mongoDBSettings.Value.StudentClassesCollectionName);
        _studentCollection = database.GetCollection<Student>(mongoDBSettings.Value.StudentsCollectionName);
        _guardianCollection = database.GetCollection<Guardian>(mongoDBSettings.Value.GuardiansCollectionName);
        _teacherCollection = database.GetCollection<Teacher>(mongoDBSettings.Value.TeachersCollectionName);
    }

    #region "StudentClass"

    public async Task<List<StudentClass>> GetAsync() {
        return await _studentClassCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task UpdateClassAsync(string id, string className, int numberOfStudents, string _teacherId)
    {
        FilterDefinition<StudentClass> filter = Builders<StudentClass>.Filter.Eq("Id", id);
        UpdateDefinition<StudentClass> update = Builders<StudentClass>.Update
            .Set(s => s.className, className)
            .Set(s => s.numberOfStudents, numberOfStudents)
            .Set(s => s._teacherId, _teacherId);

        await _studentClassCollection.UpdateOneAsync(filter, update);
        return;
    }

    public async Task CreateAsync(StudentClass studentClass) {
        await _studentClassCollection.InsertOneAsync(studentClass);
        return;
    }

    public async Task<StudentClass> GetByIdAsync(string id)
    {
        FilterDefinition<StudentClass> filter = Builders<StudentClass>.Filter.Eq("Id", id);
        return await _studentClassCollection.Find(filter).FirstOrDefaultAsync();  
    }

    public async Task DeleteAsync(string id) {
        FilterDefinition<StudentClass> filter = Builders<StudentClass>.Filter.Eq("Id", id);
        await _studentClassCollection.DeleteOneAsync(filter);
        return;
    }

    #endregion

    #region "Student"

    public async Task<List<Student>> GetStudentsByClassIdAsync(string _classId)
    {
        FilterDefinition<Student> filter = Builders<Student>.Filter.Eq("_classId", _classId);
        return await _studentCollection.Find(filter).ToListAsync();
    }

    public async Task CreateStudentAsync(Student student)
    {
        await _studentCollection.InsertOneAsync(student);
        return;
    }

    public async Task<Student> GetStudentByClassIdStudentIdAsync(string _classId, string id)
    {
        FilterDefinition<Student> filter = Builders<Student>.Filter.And(
        Builders<Student>.Filter.Eq("Id", id),
        Builders<Student>.Filter.Eq("_classId", _classId));
        return await _studentCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task DeleteStudentAsync(string _classId, string id)
    {
        FilterDefinition<Student> filter = Builders<Student>.Filter.And(
        Builders<Student>.Filter.Eq("Id", id),
        Builders<Student>.Filter.Eq("_classId", _classId));
        await _studentCollection.DeleteOneAsync(filter);
        return;
    }

    #endregion

    #region "Guardian"
    public async Task CreateGuardianAsync(Guardian guardian)
    {
        await _guardianCollection.InsertOneAsync(guardian);
    }

    public async Task<List<Guardian>> GetGuardiansAsync()
    {
        return await _guardianCollection.Find(guardian => true).ToListAsync();
    }

    public async Task<Guardian> GetGuardianByIdAsync(string id)
    {
        return await _guardianCollection.Find(guardian => guardian.Id == id).FirstOrDefaultAsync();
    }
    #endregion

    #region "Teacher"
    public async Task CreateTeacherAsync(Teacher teacher)
    {
        await _teacherCollection.InsertOneAsync(teacher);
    }

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        var teachers = await _teacherCollection.Find(_ => true).ToListAsync();
        return teachers;
    }

    public async Task<Teacher> GetTeacherAsync(string id)
    {
        var teacher = await _teacherCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        return teacher;
    }
    #endregion
}