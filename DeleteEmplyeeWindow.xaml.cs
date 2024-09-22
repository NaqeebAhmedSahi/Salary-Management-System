using System;
using System.Data.SqlClient;
using System.Windows;

namespace Salary_Management
{
    public partial class DeleteEmployeeWindow : Window
    {
        // SQL Server connection string
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WorkerSalary;Integrated Security=True";

        public DeleteEmployeeWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT WorkerID, WorkerName FROM Employee", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EmployeeComboBox.Items.Add(new EmployeeItem
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is EmployeeItem selectedEmployee)
            {
                DeleteEmployee(selectedEmployee.Id);
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        private void DeleteEmployee(int empId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE WorkerID = @EmployeeId", conn);
                    cmd.Parameters.AddWithValue("@EmployeeId", empId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee deleted successfully!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting employee. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private class EmployeeItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name; // Display the employee name in the ComboBox
            }
        }
    }
}
