using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SqlLibrary {
    public class StudentController {

        public static BcConnection bcConnection { get; set; }

        private static Student LoadStudentInstance(SqlDataReader reader) { //method to load instance
            var student = new Student();                     //used in multiple places to eliminate
            student.Id = Convert.ToInt32(reader["Id"]);        //duplicated code below
            student.Firstname = reader["Firstname"].ToString();
            student.Lastname = reader["Lastname"].ToString();
            student.SAT = Convert.ToInt32(reader["SAT"]);
            student.GPA = Convert.ToDouble(reader["GPA"]);
            return student;
        } 

        //sql statement to retrieve info from Db and make a collection
        public static List<Student> GetAllStudents() {
            var sql = "SELECT * From Student s Left Join Major m on m.Id = s.MajorId";
            var command = new SqlCommand(sql, bcConnection.Connection); //makes the connection
            var reader = command.ExecuteReader();
            if (!reader.HasRows) {  //does not have rows
                reader.Close();
                reader = null;
                Console.WriteLine("No rows from GetAllStudents()");
                return new List<Student>();
            }
            var students = new List<Student>();
            while (reader.Read()) {
                  var student = LoadStudentInstance(reader); //callling method ablove to avoid dup code
              //  var student = new Student();
              //  student.Id = Convert.ToInt32(reader["Id"]);
              //  student.Firstname = reader["Firstname"].ToString();
              //  student.Lastname = reader["Lastname"].ToString();
              //  student.SAT = Convert.ToInt32(reader["SAT"]);
              //  student.GPA = Convert.ToDouble(reader["GPA"]);
                //student.MajorId = Convert.ToInt32(reader["MajorId"]);
                if(Convert.IsDBNull(reader["Description"])) { //to convert from sql null to c# null
                    student.Major = null;
                } else {
                    var major = new Major {
                        Description = reader["Description"].ToString(),
                        MinSat = Convert.ToInt32(reader["MinSat"])
                    };
                    student.Major = major;
                }

                students.Add(student); //add to the list then repeat the loop
            }
            reader.Close();
            reader = null;
            return students; //collection of students filled in above loop
        }

        public static Student GetStudentByPk(int id ) { //test sql statement in SSMS first
            var sql = $"SELECT * from Student Where ID = {id}";//for string '{Id}' need single quotes
            var command = new SqlCommand(sql, bcConnection.Connection); //opens connection
            var reader = command.ExecuteReader();
            if (!reader.HasRows) { //no rows
                reader.Close();
                reader = null;
                return null;
            }
            reader.Read();  //opens reader and points it at the next row - starts before and ends after
            var student = LoadStudentInstance(reader);
            // var student = new Student();
           // student.Id = Convert.ToInt32(reader["Id"]);
           // student.Firstname = reader["Firstname"].ToString();
           // student.Lastname = reader["Lastname"].ToString();
           // student.SAT = Convert.ToInt32(reader["SAT"]);
           // student.GPA = Convert.ToDouble(reader["GPA"]);
            //student.MajorId = Convert.ToInt32(reader["MajorId"]);
            reader.Close();
            reader = null;
            return student;
        }

        public static bool InsertStudent(Student student) {//usually pass in an instance we create
            var majorid = "";
            if(student.MajorId == null) {
                majorid = "NULL";
            } else {
                majorid = student.MajorId.ToString();
            }
            //using string interpolation for SQL statements is not good practice - too easy to hack
            //var sql = $"INSERT into Student(Id, Firstname, Lastname, SAT, GPA, MajorId) " +
              //     $" VALUES({student.Id}, '{student.Firstname}', '{student.Lastname}', {student.SAT}, //{student.GPA}, {majorid})";
              //Use SQL with parameters as below
            var sql = $"INSERT into Student(Id, Firstname, Lastname, SAT, GPA, MajorId)  VALUES(@Id, @Firstname, @Lastname, @SAT, @GPA, @MajorId)";

            var command = new SqlCommand(sql, bcConnection.Connection);
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@Firstname", student.Firstname);
            command.Parameters.AddWithValue("@Lastname", student.Lastname);
            command.Parameters.AddWithValue("@SAT", student.SAT);
            command.Parameters.AddWithValue("@GPA", student.GPA);
            command.Parameters.AddWithValue("@MajorId", student.MajorId ?? Convert.DBNull);

            var recsAffected = command.ExecuteNonQuery(); //number of Records Affected
            if(recsAffected != 1) {             //no records affected means it didn't work
                throw new Exception("Insert failed");
            }
            return true;
            
        }
        public static bool UpdateStudent(Student student) {
            var sql = "UPDATE Student Set" +
                " Firstname = @Firstname, " +
                " Lastname = @Lastname, " +      //this is all the sql statement
                " SAT = @SAT, " +
                " GPA = @GPA, " +
                " MajorID = @MajorId " +
                " Where Id = @Id ";

            var command = new SqlCommand(sql, bcConnection.Connection); //opens the connection
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@Firstname", student.Firstname);
            command.Parameters.AddWithValue("@Lastname", student.Lastname);
            command.Parameters.AddWithValue("@SAT", student.SAT);
            command.Parameters.AddWithValue("@GPA", student.GPA);
            command.Parameters.AddWithValue("@MajorId", student.MajorId ?? Convert.DBNull);

            var recsAffected = command.ExecuteNonQuery();
            if (recsAffected != 1) { //no recs affected = failed
                throw new Exception("Update failed");
            }
            return true;

        }

        public static bool DeleteStudent(Student student) {
            var sql = " DELETE from Student where Id = @Id ";

            var command = new SqlCommand(sql, bcConnection.Connection); //opens connection
            command.Parameters.AddWithValue("@Id", student.Id); //passes in the Id

            var recsAffected = command.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new Exception("Delete failed");
            }
            return true;

        }
        public static bool DeleteStudent(int id) {
            var student = GetStudentByPk(id);
            if(student == null) {
                return false;
            }
            var success = DeleteStudent(student);
            return true;
        }
    } 

    
}
