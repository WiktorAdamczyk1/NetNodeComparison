const mongoose = require('mongoose');

const StudentSchema = new mongoose.Schema({
  name: { type: String, trim: true, minlength: 3 },
  lastName: { type: String, trim: true },
  phone: { type: String },
  email: { type: String },
  dateOfBirth: { type: Date },
  _classId: { type: mongoose.Types.ObjectId, required: true },
  _guardianId: { type: mongoose.Types.ObjectId, required: true },
}, { versionKey: false });

const Student = mongoose.model('Student', StudentSchema);

module.exports = Student;