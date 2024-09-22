using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Salary_Management
{
    /// <summary>
    /// Interaction logic for ApplyForLoadWindow.xaml
    /// </summary>
    public partial class ApplyForLoadWindow : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WorkerSalary;Integrated Security=True;";

        public ApplyForLoadWindow()
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
                    EmployeeComboBox.Items.Add(new
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }

                reader.Close();
            }
        }

        private void EmployeeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem != null)
            {
                var selectedEmployee = (dynamic)EmployeeComboBox.SelectedItem;
                ShowTotalLoans(selectedEmployee.Id);
            }
        }

        private void ShowTotalLoans(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT SUM(LoanAmount) FROM LoanTable WHERE EmployeeID = @EmployeeID", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                object result = cmd.ExecuteScalar();
                decimal totalLoan = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                TotalLoanTextBlock.Text = totalLoan.ToString("C"); // Format as currency
            }
        }

        private void ApplyLoan_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem != null && !string.IsNullOrEmpty(LoanAmountTextBox.Text))
            {
                var selectedEmployee = (dynamic)EmployeeComboBox.SelectedItem;
                decimal loanAmount;

                if (decimal.TryParse(LoanAmountTextBox.Text, out loanAmount) && loanAmount > 0)
                {
                    SaveLoanToDatabase(selectedEmployee.Id, selectedEmployee.Name, loanAmount);
                    ShowTotalLoans(selectedEmployee.Id); // Refresh total loans display
                    LoanAmountTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Please enter a valid loan amount.");
                }
            }
            else
            {
                MessageBox.Show("Please select an employee and enter a loan amount.");
            }
        }

        private void SaveLoanToDatabase(int employeeId, string employeeName, decimal loanAmount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO LoanTable (EmployeeID, EmployeeName, LoanAmount) VALUES (@EmployeeID, @EmployeeName, @LoanAmount)", conn);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                cmd.Parameters.AddWithValue("@LoanAmount", loanAmount);

                cmd.ExecuteNonQuery();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // This will close the current window
        }
    }
}
