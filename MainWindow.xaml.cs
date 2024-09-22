using Salary_Management;
using System.Windows;

namespace Salary_Management
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Add Employee Button Click Event
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.Show();
        }

        // Generate Salary Button Click Event
        private void GenerateSalary_Click(object sender, RoutedEventArgs e)
        {
            GenerateSalaryWindow salaryWindow = new GenerateSalaryWindow();
            salaryWindow.Show();
        }

        // Apply for Advance Button Click Event
        private void ApplyForAdvance_Click(object sender, RoutedEventArgs e)
        {
            ApplyForLoadWindow advanceWindow = new ApplyForLoadWindow();
            advanceWindow.Show();
        }
      
        private void GeneratePDF_Click(object sender, RoutedEventArgs e)
        {
            // Logic for generating a PDF
        }

        private void ReturnLoanClick(object sender, RoutedEventArgs e)
        {
            ReturnLoanWindow returnLoanWindow = new ReturnLoanWindow();
            returnLoanWindow.Show();
        }

        private void UpgradeSalary_Click(object sender, RoutedEventArgs e)
        {
            UpdateSalaryWindow updateSalaryWindow = new UpdateSalaryWindow();
            updateSalaryWindow.Show();
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            DeleteEmployeeWindow deleteEmployeeWindow = new DeleteEmployeeWindow();
            deleteEmployeeWindow.Show();

        }
    }
}
