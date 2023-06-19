const mongoose = require('mongoose');

const TeacherSchema = new mongoose.Schema({
  name: { type: String, trim: true, minlength: 3 },
  lastName: { type: String, trim: true },
  phone: { type: String },
  email: { type: String },
}, { versionKey: false });

const Teacher = mongoose.model('Teachers', TeacherSchema);

module.exports = Teacher;