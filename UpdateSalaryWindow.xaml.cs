using System;
using System.Data.SqlClient;
using System.Windows;

namespace Salary_Management
{
    public partial class UpdateSalaryWindow : Window
    {
        // SQL Server connection string
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WorkerSalary;Integrated Security=True";

        public UpdateSalaryWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT WorkerID, WorkerName, BasicSalary FROM Employee", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EmployeeComboBox.Items.Add(new EmployeeItem
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PreviousSalary = reader.GetDecimal(2)
                    });
                }
            }
        }

        private void EmployeeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is EmployeeItem selectedEmployee)
            {
                PreviousSalaryTextBox.Text = selectedEmployee.PreviousSalary.ToString("C"); // Display as currency
            }
        }

        private void UpdateSalary_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is EmployeeItem selectedEmployee)
            {
                if (decimal.TryParse(NewSalaryTextBox.Text, out decimal newSalary))
                {
                    UpdateEmployeeSalary(selectedEmployee.Id, newSalary);
                }
                else
                {
                    MessageBox.Show("Please enter a valid salary amount.");
                }
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        private void UpdateEmployeeSalary(int empId, decimal newSalary)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Employee SET BasicSalary = @NewSalary WHERE WorkerID = @EmployeeId", conn);
                    cmd.Parameters.AddWithValue("@NewSalary", newSalary);
                    cmd.Parameters.AddWithValue("@EmployeeId", empId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Salary updated successfully!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error updating salary. Please try again.");
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
            public decimal PreviousSalary { get; set; }

            public override string ToString()
            {
                return Name; // Display the employee name in the ComboBox
            }
        }
    }
}
