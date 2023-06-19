const mongoose = require('mongoose')

//S3 creating the Schema

const StudentClassSchema = new mongoose.Schema({

className:{type : String, trim: true, minlength: 3},

numberOfStudents:{type : Number, trim: true, minlength: 3},

_teacherId: { type: mongoose.Types.ObjectId, required: true }

}, { versionKey: false })

const StudentClass = mongoose.model('StudentClasses', StudentClassSchema)

module.exports = StudentClass