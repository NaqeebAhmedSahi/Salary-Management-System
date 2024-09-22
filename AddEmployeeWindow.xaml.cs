using System;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;
using System.IO;


namespace Salary_Management
{
    public partial class AddEmployeeWindow : Window
    {
        string connectionString = $@"Data Source=.;AttachDbFilename={AppDomain.CurrentDomain.BaseDirectory}DemoDatabase1.mdf;Integrated Security=True;User Instance=True";

        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoDatabase1.mdf");

            if (!File.Exists(databaseFile))
            {
                Console.WriteLine("Database file not found: " + databaseFile);
                return;
            }

            // Get input data
            string employeeName = EmployeeNameTextBox.Text;
            string designation = DesignationTextBox.Text;
            string department = DepartmentTextBox.Text;
            string contact = ContactTextBox.Text;
            string email = EmailTextBox.Text;
            string basicSalaryStr = BasicSalaryTextBox.Text;

            // Validate salary input
            if (!decimal.TryParse(basicSalaryStr, out decimal basicSalary))
            {
                MessageBox.Show("Please enter a valid salary amount.");
                return;
            }

            // Validate other inputs
            if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(designation) || string.IsNullOrEmpty(department))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Insert employee data into the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "INSERT INTO Employee (WorkerName, WorkerTitle, WorkerDepartment, Contact, Email, BasicSalary) " +
                                   "VALUES (@WorkerName, @WorkerTitle, @WorkerDepartment, @Contact, @Email, @BasicSalary)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@WorkerName", employeeName);
                        cmd.Parameters.AddWithValue("@WorkerTitle", designation);
                        cmd.Parameters.AddWithValue("@WorkerDepartment", department);
                        cmd.Parameters.AddWithValue("@Contact", contact);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@BasicSalary", basicSalary);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Employee '{employeeName}' has been added successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            // Clear the form after submission
            EmployeeNameTextBox.Clear();
            DesignationTextBox.Clear();
            DepartmentTextBox.Clear();
            ContactTextBox.Clear();
            EmailTextBox.Clear();
            BasicSalaryTextBox.Clear();
        }
    }
}
