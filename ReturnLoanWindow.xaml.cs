using System;
using System.Data.SqlClient;
using System.Windows;

namespace Salary_Management
{
    public partial class ReturnLoanWindow : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WorkerSalary;Integrated Security=True;";

        public ReturnLoanWindow()
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
                    EmployeeComboBox.Items.Add(new { ID = reader["WorkerID"], Name = reader["WorkerName"] });
                }
            }
        }

        private void EmployeeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is { } selectedEmployee)
            {
                int employeeId = ((dynamic)selectedEmployee).ID;
                DisplayTotalLoanAmount(employeeId);
            }
        }

        private void DisplayTotalLoanAmount(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT SUM(LoanAmount) FROM LoanTable WHERE EmployeeID = @EmployeeID", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                var totalLoan = cmd.ExecuteScalar();
                TotalLoanTextBlock.Text = totalLoan != null ? totalLoan.ToString() : "0";
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is { } selectedEmployee && decimal.TryParse(ReturnLoanTextBox.Text, out decimal returnAmount))
            {
                int employeeId = ((dynamic)selectedEmployee).ID;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check the total loan amount
                    SqlCommand checkCmd = new SqlCommand("SELECT SUM(LoanAmount) FROM LoanTable WHERE EmployeeID = @EmployeeID", conn);
                    checkCmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                    var totalLoan = checkCmd.ExecuteScalar();

                    if (totalLoan == null || (decimal)totalLoan < returnAmount)
                    {
                        MessageBox.Show("Return amount exceeds total loan.");
                        return;
                    }

                    // Update the LoanTable by deducting the return amount
                    SqlCommand updateCmd = new SqlCommand("UPDATE LoanTable SET LoanAmount = LoanAmount - @ReturnAmount WHERE EmployeeID = @EmployeeID AND LoanAmount >= @ReturnAmount", conn);
                    updateCmd.Parameters.AddWithValue("@ReturnAmount", returnAmount);
                    updateCmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Loan return processed successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to return loan. Please check the values.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee and enter a valid return amount.");
            }
        }

        private void ReturnLoanTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Optional: Handle any specific text change logic if needed
        }
    }
}
