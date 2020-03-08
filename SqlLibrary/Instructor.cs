using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLibrary {
   public class Instructor {

        public static BcConnection bcConnection { get; set; }

        private static Instructor LoadInstructInstance(sqlDataReader reader) {
            var instructor = new Instructor();
            instructor.Id = Convert.ToInt32(reader["Id"]);
            instructor.Firstname = reader["Firstname"].ToString();
            instructor.Lastname = reader["Lastname"].ToString();
            instructor.YearsExperience = Convert.ToInt32(reader["YearsExperience"]);
            instructor.IsTenured = Convert.ToBoolean(reader["IsTenured"]);
            return instructor;
        }
        public static List<Instructor>GetAllInstructors() {
            var sql = "SELECT * From Instructor";
            var command = new SqlCommand(sql, bcConnection.Connection);
            var reader = command.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                reader = null;
                Console.WriteLine("No Rows from GetAllInstructors()");
                return new List<Instructor>();
            }
            var instructors = new List<Instructor>();
            while(reader.Read()) {
                var instructor = LoadInstructInstance(reader);
                instructors.Add(instructor);
            }
            reader.Close();
            reader = null;
            return instructors;
        }
     }
}
