using System;
using SqlLibrary;
namespace SequelLesson {

    class Program {

        static void Main(string[] args) {

            var sqllib = new BcConnection();
            sqllib.connect(@"localhost\sqlexpress", "EdDb", "trusted_connection=true");
            MajorController.bcConnection = sqllib;

            var majors = MajorController.GetAllMajors();
            foreach(var major in majors) {
                Console.WriteLine(major);
            }

            StudentController.bcConnection = sqllib; //giving student class access to connection so 
            //can use Db - do 1x


          //  var newStudent = new Student {
              //  Id = 444,
               // Firstname = "Fred",
               // Lastname = "Fives",
               // SAT = 500,
               // GPA = 5.00,
               // MajorId = 1
           // };
          //  var success = StudentController.InsertStudent(newStudent);

            var student = StudentController.GetStudentByPk(888);
            if (student == null) {
                Console.WriteLine("Student not found");
                } else {
                Console.WriteLine(student);
            }
            student.Firstname = "Charlie";
            student.Lastname = "Chan";
            var success = StudentController.UpdateStudent(student);

            var StudentToDelete = new Student {
                Id = 999
            };
            //success = StudentController.DeleteStudent(999);

            var students = StudentController.GetAllStudents();
            foreach(var student0 in students) {
                Console.WriteLine(student0);
            }
            sqllib.Disconnect();
        }
    }
}
