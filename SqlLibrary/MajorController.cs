using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SqlLibrary {
    public class MajorController {

        public static BcConnection bcConnection { get; set; }

        private static Major LoadMajorInstance(SqlDataReader reader) { //method
            var major = new Major();
            major.Id = Convert.ToInt32(reader["Id"]);
            major.Description = reader["Description"].ToString();
            major.MinSat = Convert.ToInt32(reader["MinSat"]);
            return major;
        }

        public static List<Major> GetAllMajors() {
            var sql = "SELECT * from Major; ";
            var command = new SqlCommand(sql, bcConnection.Connection);
            var reader = command.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                reader = null;
                Console.WriteLine("No rows for GetAllMajors");
                return new List<Major>();
            }
            var majors = new List<Major>();
            while(reader.Read()) {
                var major = LoadMajorInstance(reader); //callling method above - replaces below info
               // var major = new Major(); //create instance of class to hold data
               // major.Id = Convert.ToInt32(reader["Id"]);
              //  major.Description = reader["Description"].ToString();
              //  major.MinSat = Convert.ToInt32(reader["MinSat"]);
                majors.Add(major);  //add instance to the collection
            }
            reader.Close();
            reader = null;
            return majors;
        }

        public static Major GetMajorByPk(int id) {
            Console.WriteLine( id);
            var sql =$"SELECT * from Major Where Id = {id} "; //Connection is sqlConnection
            var command = new SqlCommand(sql, bcConnection.Connection); //class BcConnection
          //  command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            if(!reader.HasRows) { //no rows
                reader.Close();
                reader = null;
                return null;
            }
            reader.Read(); //positions reader
            var major = LoadMajorInstance(reader); //callling method above to replace below
            //  var major = new Major(); //creat instance of class and put data in it
            //  major.Id = Convert.ToInt32(reader["Id"]); //converts object to int
            //  major.Description = reader["Description"].ToString(); //can turn anything into a string
            //  major.MinSat = Convert.ToInt32(reader["MinSat"]);
            reader.Close();
            reader = null;
            return major;
        }

        public static bool InsertMajor(Major major) {

            var sql = $"INSERT into Major(Id, Description, MinSat) VALUES(@Id, @Description, @MinSat)";
            var command = new SqlCommand(sql, bcConnection.Connection);

            command.Parameters.AddWithValue("@Id", major.Id);
            command.Parameters.AddWithValue("@Description", major.Description);
            command.Parameters.AddWithValue("@MinSat", major.MinSat);

            var recsAffected = command.ExecuteNonQuery(); //number of Records Affected
            if (recsAffected != 1) {             //no records affected means it didn't work
                throw new Exception("Insert failed");
            }
            return true;
        }

        public static bool DeleteMajor(Major major) {
            var sql = " DELETE from Major where Id = @Id ";

            var command = new SqlCommand(sql, bcConnection.Connection); //opens connection
            command.Parameters.AddWithValue("@Id", major.Id); //passes in the Id

            var recsAffected = command.ExecuteNonQuery();
            if (recsAffected != 1) {
            throw new Exception("Delete failed");
            }
                return true;

        }
        public static bool DeleteMajor(int id) {
            var major = GetMajorByPk(id);
            if (major ==null) {
            return false;
            }
                var success = DeleteMajor(id);
                return true;
        }

        

        public static bool UpdateMajor(Major major) {

            var sql = $"UPDATE Major SET " +
                    " Description = @Description, " +
                    " Minsat = @MinSat " +
                    " Where Id = @Id ";
            var command = new SqlCommand(sql, bcConnection.Connection);

            command.Parameters.AddWithValue("@Description", major.Description);
            command.Parameters.AddWithValue("@MinSat", major.MinSat);
            command.Parameters.AddWithValue("@Id", major.Id);
            var recsAffected = command.ExecuteNonQuery(); //number of Records Affected
            if (recsAffected != 1) {             //no records affected means it didn't work
                throw new Exception("Update failed");
            }
            return true;
        }
    }
    
}
