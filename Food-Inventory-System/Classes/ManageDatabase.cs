﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Food_Inventory_System.Classes
{
    public class ManageDatabase
    {
        public List<Food> Foods { get; private set; }
        public User User { get; private set; }

        private string server = "localhost";
        private string uid = "root";
        private string pwd = "Laurente1234.";
        private string con;

        public ManageDatabase()
        {
            string uid = "root";
            string pwd = "Laurente1234.";
            con = $"server=127.0.0.1;port=3306;uid={uid};pwd={pwd}";
            Foods = new List<Food>();
        }

        public List<Food> GetAllFoods()
        {
            Foods.Clear();
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM application.food", connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Foods.Add(new Food(
                                (string)reader["FoodID"],
                                (string)reader["Name"],
                                (int)reader["Quantity"],
                                (Category)Enum.Parse(typeof(Category), reader["Category"].ToString()),
                                (DateTime)reader["ExpiryDate"],
                                (StorageLocation)Enum.Parse(typeof(StorageLocation), reader["StorageLocation"].ToString()),
                                (Status)Enum.Parse(typeof(Status), reader["FoodStatus"].ToString())
                            ));
                            }
                        }
                    }
                    return Foods;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while retrieving users: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return Foods;

        }

        public bool addFood(Food food)
        {
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                try
                {
                    string query = "INSERT INTO application.food(FoodID, Name, Quantity, Category, ExpiryDate, StorageLocation, FoodStatus) VALUES (@FoodID, @Name, @Quantity, @Category, @ExpiryDate, @StorageLocation, @FoodStatus)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FoodID", food.FoodID);
                        command.Parameters.AddWithValue("@Name", food.FoodName);
                        command.Parameters.AddWithValue("@Quantity", food.Quantity);
                        command.Parameters.AddWithValue("@Category", food.Category.ToString());
                        command.Parameters.AddWithValue("@ExpiryDate", food.ExpiryDate);
                        command.Parameters.AddWithValue("@StorageLocation", food.StorageLocation.ToString());
                        command.Parameters.AddWithValue("@FoodStatus", food.Status.ToString());
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while adding food: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return false;
        }

    }
}
