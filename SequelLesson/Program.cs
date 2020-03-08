using System;
using SqlLibrary;
namespace SequelLesson {

    class Program {
        

        static void Main(string[] args) {

            var sqllib = new BcConnection();
            sqllib.connect(@"localhost\sqlexpress", "EdDb", "trusted_connection=true");
            InstructorController.bcConnection = sqllib;


            var instructors = InstructorController.GetAllInstructors();
            foreach (var instructor in instructors) {
                Console.WriteLine(instructor);
            }
            sqllib.Disconnect();

            var sqllib = new BcConnection();
            sqllib.connect(@"localhost\sqlexpress", "EdDb", "trusted_connection=true");
            MajorController.bcConnection = sqllib;

            var major = MajorController.GetMajorByPk(1);
            Console.WriteLine(major);
            Console.WriteLine("Hi");

            var majors = MajorController.GetAllMajors();
            foreach(var m in majors) {
                Console.WriteLine(m);
            }

            StudentController.bcConnection = sqllib; //giving student class access to connection so 
            //can use Db - do 1x
            

           // var newMajor = new Major {
           //     Id = 13,
           //     Description = "Computer Science",
           //     MinSat = 1175
           // };
           // var Msuccess = MajorController.InsertMajor(newMajor);
           // Console.WriteLine(major);

           // foreach (var m in majors) {
           //     Console.WriteLine(m);
            //}

             var upMajor = new Major {
                Id = 10,
                Description = "Zoology2",
            };
             var UpSuccess = MajorController.UpdateMajor(upMajor);
             Console.WriteLine(major);

            var MajorToDelete = new Major {
                Id = 25
            };
            var msuccess = MajorController.DeleteMajor(25);

            
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
