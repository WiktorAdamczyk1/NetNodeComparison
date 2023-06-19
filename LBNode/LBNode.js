const cluster = require('cluster');
const https = require('https');
const fs = require('fs');
const express = require('express');
const { v4: uuidv4 } = require('uuid');
const path = require('path');
const fileUpload = require('express-fileupload');

const app = express();
const mongoose = require('./db/mongoose');
const studentclass = require('./db/dbmodels/studentClass');
const student = require('./db/dbmodels/student');
const guardian = require('./db/dbmodels/guardian');
const teacher = require('./db/dbmodels/teacher');

const options = {
  key: fs.readFileSync('./cert/keytmp.pem'),
  cert: fs.readFileSync('./cert/cert.pem'),
  passphrase: 'WiktorAdamczyk',
};

const numCPUs = require('os').cpus().length;

if (cluster.isMaster) {
  // Code for the master process
  console.log(`Master process ID: ${process.pid}`);

  // Fork worker processes
  for (let i = 0; i < numCPUs; i++) {
    cluster.fork();
  }

  cluster.on('exit', (worker, code, signal) => {
    // Replace the exited worker
    console.log(`Worker process ${worker.process.pid} exited with code ${code} and signal ${signal}`);
    cluster.fork();
  });
} else {
  // Code for worker processes
  console.log(`Worker process ID: ${process.pid}`);

  // Enable our app to parse JSON data format
  app.use(express.json());
  app.use((req, res, next) => {
    res.header("Access-Control-Allow-origin", "*");
    res.header("Access-Control-Allow-Methods", "GET, POST, HEAD, OPTIONS, PUT, PATCH, DELETE");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
  });

//posts method for creating a new student class

app.post('/studentclass', (req, res) => {

  (new studentclass({'className': req.body.className, 'numberOfStudents' : req.body.numberOfStudents, '_teacherId' : req.body._teacherId}))
  
  .save()
  
  .then((studentclass) => res.send(studentclass))
  
  .catch((error) => console.log(error))
  
  })

//read all classes
app.get('/studentclass', (req, res) =>{

  studentclass.find({})
  
  .then(studentclass =>res.send(studentclass))
  
  .catch((error) => console.log(error))
  
  })

//get one student class
app.get('/studentclass/:studentclassId', (req, res) =>{

  studentclass.findOne( { _id: req.params.studentclassId })
  
  .then(studentclass =>res.send(studentclass))
  
  .catch((error) => console.log(error))
  
  })

app.post('/studentclass/:studentclassId/students', function(req, res) {
  (new student({
      'name': req.body.name,
      'lastName': req.body.lastName,
      'phone': req.body.phone,
      'email': req.body.email,
      'dateOfBirth': req.body.dateOfBirth,
      '_classId': req.params.studentclassId,
      '_guardianId': req.body._guardianId,
  }))
  .save()
  .then((student) => res.send(student))
  .catch((error) => console.log(error))
  });

//get all students from this classId
app.get('/studentclass/:studentclassId/students', (req, res) =>{

  student.find({ _classId: req.params.studentclassId })
  
  .then((student) => res.send(student))
  
  .catch((error) => console.log(error))
  
  })

//get one student
app.get('/studentclass/:studentclassId/students/:studentId', (req, res) =>{

  student.findOne({ _classId: req.params.studentclassId, _id: req.params.studentId })
  
  .then((onestudent) => res.send(onestudent))
  
  .catch((error) => console.log(error))
  
  })

//update student class

app.patch('/studentclass/:studentclassId', (req, res) =>{

  studentclass.findOneAndUpdate({ '_id' : req.params.studentclassId }, {$set: req.body})
  
  .then((studentclass) => res.send(studentclass))
  
  .catch((error) => console.log(error))
  
  })

//delete student class
app.delete('/studentclass/:studentclassId', (req, res) =>{

  const deleteStudents = (studentclass) =>{
  
  student.deleteMany({ '_id': req.params.studentclassId})
  
  .then(() => studentclass)
  
  .catch((error) => console.log(error))
  
  }
  
  studentclass.findByIdAndDelete( { '_id': req.params.studentclassId})
  
  .then((studentclass) => res.send(deleteStudents(studentclass)))
  
  .catch((error) => console.log(error))
  
  })


//delete student info
app.delete('/studentclass/:studentclassId/students/:studentId', (req, res) => {

  student.findOneAndDelete({ _id: req.params.studentId, _classId: req.params.studentclassId }).then((student) => res.send(student))
  
  .catch((error) => console.log(error))
  
  })

// create a new guardian
app.post('/guardian', (req, res) => {
  (new guardian({
    'name': req.body.name,
    'lastName': req.body.lastName,
    'phone': req.body.phone,
    'email': req.body.email
  }))
  .save()
  .then((guardian) => res.send(guardian))
  .catch((error) => console.log(error))
});

// get all guardians
app.get('/guardian', (req, res) => {
  guardian.find({})
  .then(guardians => res.send(guardians))
  .catch((error) => console.log(error))
});

// get a single guardian by id
app.get('/guardian/:guardianId', (req, res) => {
  guardian.findOne({ _id: req.params.guardianId })
  .then(guardian => res.send(guardian))
  .catch((error) => console.log(error))
});

// create a new teacher
app.post('/teacher', (req, res) => {
  (new teacher({
    'name': req.body.name,
    'lastName': req.body.lastName,
    'phone': req.body.phone,
    'email': req.body.email
  }))
  .save()
  .then((teacher) => res.send(teacher))
  .catch((error) => console.log(error))
});

// get all teachers
app.get('/teacher', (req, res) => {
  teacher.find({})
  .then(teachers => res.send(teachers))
  .catch((error) => console.log(error))
});

// get a single teacher by id
app.get('/teacher/:teacherId', (req, res) => {
  teacher.findOne({ _id: req.params.teacherId })
  .then(teacher => res.send(teacher))
  .catch((error) => console.log(error))
});

class GradesModel {
  constructor(grades) {
    this.grades = grades;
  }
}

app.post('/test/gpa', (req, res) => {
  const grades = req.body.grades;
  const sum = grades.reduce((acc, curr) => acc + curr);
  const gpa = sum / grades.length;

  res.json({ gpa: gpa });
});

app.use(fileUpload({
  createParentPath: true
}));

app.post('/homework', async (req, res) => {
  try {
      // check if file is included in the request
      if (!req.files || !req.files.file) {
          return res.status(400).send('No file uploaded');
      }

      // get uploaded file
      const file = req.files.file;

      // generate unique file name
      const uniqueFileName = uuidv4() + '-' + file.name;

      // combine uploads directory with unique file name
      const filePath = path.join(__dirname, 'HomeworkFiles', uniqueFileName);

      // save file to server
      await file.mv(filePath);

      res.send({
          message: `Homework submitted successfully. Name:${uniqueFileName}`
      });
  } catch (err) {
      console.error(err);
      res.status(500).send(err);
  }
});

app.get('/homework/:filename', (req, res) => {
  const { filename } = req.params;
  const filePath = path.join(__dirname, 'Resources', filename);
  fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
      console.error(err);
      return res.status(404).send('File not found');
    }
    res.send(data);
  });
});

  const server = https.createServer(options, app);
  server.listen(3001, '0.0.0.0', () => {
    console.log(`Worker process ${process.pid} is listening on port 3001`);
  });
}


