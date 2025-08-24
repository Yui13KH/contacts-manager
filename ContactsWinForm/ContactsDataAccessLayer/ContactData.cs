using System;
using System.Data;
//using System.Data.SqlClient; old using ms sql
using System.Data.SQLite;


namespace ContactsDataAccessLayer
{
    public class clsContactDataAccess
    {
        public static bool GetContactInfoByID(int ID, ref  string FirstName, ref string LastName,
            ref string Email , ref string Phone, ref string Address ,
            ref DateTime DateOfBirth, ref int CountryID, ref string ImagePath)
        {
            bool isFound = false;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);


            string query = "SELECT * FROM Contacts WHERE ContactID = @ContactID";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@ContactID", ID);

            try
            {
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FirstName = reader["FirstName"].ToString();
                    LastName = reader["LastName"].ToString();
                    Email = reader["Email"].ToString();
                    Phone = reader["Phone"].ToString();
                    Address = reader["Address"].ToString();

                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    CountryID = Convert.ToInt32(reader["CountryID"]);

                    ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : "";
                }

                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
               

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            { 
                connection.Close(); 
            }

            return isFound;
        }

        public static int AddNewContact(string FirstName, string LastName,
    string Email, string Phone, string Address,
    DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            int ContactID = -1;

            using (SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
            INSERT INTO Contacts (FirstName, LastName, Email, Phone, Address, DateOfBirth, CountryID, ImagePath)
            VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @DateOfBirth, @CountryID, @ImagePath);
            SELECT last_insert_rowid();
        ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth.ToString("yyyy-MM-dd")); // store as TEXT
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            // Convert from long safely
                            ContactID = Convert.ToInt32((long)result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Optionally log ex.Message
                        Console.WriteLine("AddNewContact Error: " + ex.Message);
                    }
                }
            }

            return ContactID;
        }


        public static bool UpdateContact(int ID,string FirstName, string LastName,
            string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID,string ImagePath)
        {

            int rowsAffected=0;
            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Contacts  
                            set FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
                                CountryID = @CountryID,
                                ImagePath =@ImagePath
                                where ContactID = @ContactID";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@ContactID", ID);
            command.Parameters.AddWithValue("@FirstName",FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email",    Email);
            command.Parameters.AddWithValue("@Phone",    Phone);
            command.Parameters.AddWithValue("@Address",  Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            if (ImagePath != "" && ImagePath!=null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally 
             { 
                connection.Close(); 
             } 

            return (rowsAffected > 0);
        }

        public static DataTable GetAllContacts()
        {

            DataTable dt = new DataTable();
            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Contacts";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            try
            {
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();
               

            }

            catch (Exception ex)
            {
               // Console.WriteLine("Error: " + ex.Message);
            }
            finally 
            { 
                connection.Close(); 
            }

            return dt;

        }

        public  static bool DeleteContact(int ContactID)
        {

            int rowsAffected=0;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE FROM Contacts WHERE ContactID = @ContactID";


            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@ContactID", ContactID);

            try
            {
                connection.Open();

                 rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
               // Console.WriteLine("Error: " + ex.Message);
            }
            finally 
            { 
                
                connection.Close(); 
            
            }

            return (rowsAffected > 0);

        }

        public static bool IsContactExist(int ID)
        {
            bool isFound = false;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Contacts WHERE ContactID = @ContactID";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@ContactID", ID);

            try
            {
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
                
                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


    }
}
