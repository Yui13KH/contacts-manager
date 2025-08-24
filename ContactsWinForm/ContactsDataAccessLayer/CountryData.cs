using System;
using System.Data;
using System.Data.SQLite;

namespace ContactsDataAccessLayer
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName,
                                      ref string Code, ref string PhoneCode)
        {
            bool isFound = false;

            string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            using (var connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString))
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryID", ID);

                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            Console.WriteLine(
                           $"DEBUG CountryName={reader["CountryName"]}, Code={reader["Code"]}, PhoneCode={reader["PhoneCode"]}");
                            isFound = true;
                            CountryName = Convert.ToString(reader["CountryName"]);
                            Code        = Convert.ToString(reader["Code"]);
                            PhoneCode   = Convert.ToString(reader["PhoneCode"]);
                            
                        }
                    } // reader is closed here
                }
                catch (Exception ex)
                {
                    // optional: log ex.Message
                    isFound = false;
                }
            } // connection & command disposed here

            return isFound;
        }



        public static bool GetCountryInfoByName(string CountryName, ref int ID,
                                                ref string Code, ref string PhoneCode)
        {
            bool isFound = false;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    ID = (int)reader["CountryID"];
                    
                    if (reader["Code"] != DBNull.Value)
                    {
                        Code = (string)reader["Code"];
                    }
                    else
                    {
                        Code = "";
                    }

                    if (reader["PhoneCode"] != DBNull.Value)
                    {
                        PhoneCode = (string)reader["PhoneCode"];
                    }
                    else
                    {
                        PhoneCode = "";
                    }

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


        public static int AddNewCountry(string CountryName,string Code, string PhoneCode)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int CountryID = -1;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Countries (CountryName,Code,PhoneCode)
                             VALUES (@CountryName,@Code,@PhoneCode);
                             SELECT SCOPE_IDENTITY();";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            if (Code != "")
                command.Parameters.AddWithValue("@Code", Code);
            else
                command.Parameters.AddWithValue("@Code", System.DBNull.Value);

            if (PhoneCode != "")
                command.Parameters.AddWithValue("@PhoneCode", PhoneCode);
            else
                command.Parameters.AddWithValue("@PhoneCode", System.DBNull.Value);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
            

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    CountryID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
               
            }

            finally 
            { 
                connection.Close(); 
            }


            return CountryID;
        }

        public static bool UpdateCountry(int ID,string CountryName,string Code,string PhoneCode)
        {

            int rowsAffected=0;
            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Countries  
                            set CountryName=@CountryName,
                                Code=@Code,
                                PhoneCode=@PhoneCode
                                where CountryID = @CountryID";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", ID);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            command.Parameters.AddWithValue("@Code", Code);
            command.Parameters.AddWithValue("@PhoneCode", PhoneCode);

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

        public static DataTable GetAllCountries()
        {

            DataTable dt = new DataTable();
            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Countries order by CountryName";

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

        public  static bool DeleteCountry(int CountryID)
        {

            int rowsAffected=0;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Countries 
                                where CountryID = @CountryID";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", CountryID);

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

        public static bool IsCountryExist(int ID)
        {
            bool isFound = false;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Countries WHERE CountryID = @CountryID";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", ID);

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


        public static bool IsCountryExist(string CountryName)
        {
            bool isFound = false;

            SQLiteConnection connection = new SQLiteConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Countries WHERE CountryName = @CountryName";

            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

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
