const mongoose = require('mongoose');

const GuardianSchema = new mongoose.Schema({
  name: { type: String, trim: true, minlength: 3 },
  lastName: { type: String, trim: true },
  phone: { type: String },
  email: { type: String },
}, { versionKey: false });

const Guardian = mongoose.model('Guardians', GuardianSchema);

module.exports = Guardian;