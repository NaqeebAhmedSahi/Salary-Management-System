using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
namespace Salary_Management
{
    public partial class GenerateSalaryWindow : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WorkerSalary;Integrated Security=True;";

        public GenerateSalaryWindow()
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
                EmployeeNameTextBlock.Text = selectedEmployee.Name;
                LoadBasicSalary(selectedEmployee.Id);
                LoadRemainingLoan(selectedEmployee.Id);
            }
        }

        private void LoadBasicSalary(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT BasicSalary FROM Employee WHERE WorkerID = @WorkerID", conn);
                cmd.Parameters.AddWithValue("@WorkerID", employeeId);

                var result = cmd.ExecuteScalar();

                if (result != null && decimal.TryParse(result.ToString(), out decimal basicSalary))
                {
                    BasicSalaryTextBox.Text = basicSalary.ToString("C", CultureInfo.CurrentCulture);
                }
                else
                {
                    BasicSalaryTextBox.Text = "0.00"; // Default to 0 if no salary found
                }
            }
        }


        private void LoadRemainingLoan(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT LoanAmount FROM LoanTable WHERE EmployeeId = @EmployeeId", conn);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                var remainingLoan = cmd.ExecuteScalar();

                RemainingLoanTextBlock.Text = remainingLoan != null ? decimal.Parse(remainingLoan.ToString()).ToString("C") : "0";
            }
        }

        private void DeductionsTextBox_TextChanged_1(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateLoanDeduction();
            CalculateTotalSalary(); // Recalculate salary when deductions change
        }
        int i = 0;
        // Update the remaining loan balanc
        decimal newUpdate = 10000000000000;
        private void CalculateLoanDeduction()
        {
            // Parse the deduction amount, treat empty or invalid input as 0
            decimal deductions = 0;
            if (!decimal.TryParse(DeductionsTextBox.Text, out deductions))
            {
                deductions = 0; // Treat empty or invalid input as 0
                DeductionsTextBox.Text = "0"; // Update the textbox to show "0" for clarity
            }

            // Parse the remaining loan amount
            if (decimal.TryParse(RemainingLoanTextBlock.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal remainingLoan))
            {
                // Ensure deduction is not greater than the remaining loan
                if (deductions > newUpdate)
                {
                    MessageBox.Show("Deduction cannot be greater than the remaining loan.");
                    return; // Stop further processing if deduction is too large
                }
                else
                {
                    
                    if(i == 0)
                    { newUpdate = remainingLoan; }
                    i++;
                    remainingLoan = newUpdate;
                    decimal updatedLoan = remainingLoan - deductions;
                    RemainingLoanTextBlock.Text = updatedLoan.ToString("C", CultureInfo.CurrentCulture);
                }
            }
            else
            {
                // Handle invalid remaining loan
                MessageBox.Show("Invalid remaining loan amount.");
            }
        }



        private void CalculateTotalSalary()
        {
            try
            {
                if (decimal.TryParse(BasicSalaryTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal basicSalary) &&
                    int.TryParse(TotalBasicHoursTextBox.Text, out int totalBasicHours) &&
                    int.TryParse(TotalOvertimeHoursTextBox.Text, out int totalOvertimeHours) &&
                    decimal.TryParse(DeductionsTextBox.Text, out decimal deductions))
                {
                    decimal oneDaySalary = basicSalary / 30 ;
                    decimal overtimeRate = oneDaySalary / 6 ;
                    decimal totalOvertimeSalary = totalOvertimeHours * overtimeRate;
                    decimal basicRate = oneDaySalary / 8;
                    decimal totalBasicSalary = decimal.Parse(TotalBasicHoursTextBox.Text) * basicRate;


                    decimal totalSalary = totalOvertimeSalary + totalBasicSalary - deductions;

                    TotalSalaryTextBlock.Text = totalSalary.ToString("C", CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total salary: " + ex.Message);
            }
        }

        private void CalculateSalary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if all necessary fields are filled
                if (string.IsNullOrWhiteSpace(TotalBasicHoursTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TotalOvertimeHoursTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DeductionsTextBox.Text) ||
                    string.IsNullOrWhiteSpace(BasicSalaryTextBox.Text))
                {
                    MessageBox.Show("Please fill all fields before calculating salary.");
                    return;
                }

                // Calculate total salary
                CalculateTotalSalary();

                // Save to database
                SaveSalaryToDatabase();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SaveSalaryToDatabase()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var selectedEmployee = (dynamic)EmployeeComboBox.SelectedItem;

                // Parse the total salary as decimal instead of int
                decimal totalSalary = decimal.Parse(TotalSalaryTextBlock.Text, NumberStyles.Currency);
                int numberOfDays = int.Parse(DaysTextBox.Text);

                // Insert salary details into the SalaryTable
                SqlCommand cmd = new SqlCommand("INSERT INTO SalaryTable (EmployeeId, EmployeeName, BasicSalary, TotalBasicHours, TotalOvertimeHours, Deductions, NumberOfDays, SalaryDate, TotalSalary) " +
                                                "VALUES (@EmployeeId, @EmployeeName, @BasicSalary, @TotalBasicHours, @TotalOvertimeHours, @Deductions, @NumberOfDays, @SalaryDate, @TotalSalary)", conn);
                cmd.Parameters.AddWithValue("@EmployeeId", selectedEmployee.Id);
                cmd.Parameters.AddWithValue("@EmployeeName", EmployeeNameTextBlock.Text);

                // Convert BasicSalary to decimal
                decimal basicSalary = decimal.Parse(BasicSalaryTextBox.Text, NumberStyles.Currency);
                cmd.Parameters.AddWithValue("@BasicSalary", basicSalary);
                cmd.Parameters.AddWithValue("@TotalBasicHours", TotalBasicHoursTextBox.Text);
                cmd.Parameters.AddWithValue("@TotalOvertimeHours", TotalOvertimeHoursTextBox.Text);
                cmd.Parameters.AddWithValue("@Deductions", DeductionsTextBox.Text);
                cmd.Parameters.AddWithValue("@NumberOfDays", numberOfDays);
                cmd.Parameters.AddWithValue("@SalaryDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@TotalSalary", totalSalary);

                // Execute the salary insert command
                cmd.ExecuteNonQuery();

                // Update the LoanTable based on the RemainingLoanTextBox value
                if (decimal.TryParse(RemainingLoanTextBlock.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal remainingLoanAmount))
                {
                    SqlCommand updateLoanCmd = new SqlCommand("UPDATE LoanTable SET LoanAmount = @LoanAmount WHERE EmployeeID = @EmployeeId", conn);
                    updateLoanCmd.Parameters.AddWithValue("@LoanAmount", remainingLoanAmount);
                    updateLoanCmd.Parameters.AddWithValue("@EmployeeId", selectedEmployee.Id);

                    // Execute the loan update command
                    updateLoanCmd.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Invalid loan amount entered.");
                }
            }
        }



        private void GeneratePDF_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if all necessary data is filled
                if (string.IsNullOrWhiteSpace(EmployeeNameTextBlock.Text) ||
                    string.IsNullOrWhiteSpace(BasicSalaryTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TotalBasicHoursTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TotalOvertimeHoursTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DeductionsTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TotalSalaryTextBlock.Text))
                {
                    MessageBox.Show("Please ensure all fields are filled out before generating the PDF.");
                    return;
                }

                // Gather data for PDF
                var employeeName = EmployeeNameTextBlock.Text;
                var basicSalary = BasicSalaryTextBox.Text;
                var totalBasicHours = TotalBasicHoursTextBox.Text;
                var totalOvertimeHours = TotalOvertimeHoursTextBox.Text;
                var deductions = DeductionsTextBox.Text;
                var totalSalary = TotalSalaryTextBlock.Text;

                // Path where the PDF will be saved
                string pdfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SalaryStatement.pdf");

                // Create a new PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Salary Statement";

                // Create an empty page
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Set up fonts
                XFont headerFont = new XFont("Verdana", 24);
                XFont subHeaderFont = new XFont("Verdana", 14);
                XFont bodyFont = new XFont("Verdana", 12);

                // Draw watermark
                gfx.Save(); // Save the current state
                gfx.TranslateTransform(page.Width / 2, page.Height / 2); // Move to center of the page
                gfx.RotateTransform(45); // Rotate 45 degrees
                XFont watermarkFont = new XFont("Verdana", 60);
                gfx.DrawString("Company Name", watermarkFont, new XSolidBrush(XColor.FromArgb(50, XColors.Gray)),
                    new XRect(-200, -30, page.Width, page.Height), // Center the text
                    XStringFormats.Center);
                gfx.Restore(); // Restore the state

                // Draw header
                gfx.DrawString("Company Name", headerFont, XBrushes.DarkBlue, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);
                gfx.DrawString("Salary Statement", headerFont, XBrushes.Black, new XRect(0, 60, page.Width, page.Height), XStringFormats.TopCenter);

                // Draw a horizontal line
                gfx.DrawLine(XPens.Black, 50, 90, page.Width - 50, 90);

                // Employee details section
                gfx.DrawString($"Employee Name: {employeeName}", bodyFont, XBrushes.Black, 50, 110);
                gfx.DrawString($"Basic Salary: {basicSalary}", bodyFont, XBrushes.Black, 50, 140);
                gfx.DrawString($"Total Basic Hours: {totalBasicHours}", bodyFont, XBrushes.Black, 50, 170);
                gfx.DrawString($"Total Overtime Hours: {totalOvertimeHours}", bodyFont, XBrushes.Black, 50, 200);
                gfx.DrawString($"Deductions: {deductions}", bodyFont, XBrushes.Black, 50, 230);
                gfx.DrawString($"Total Salary: {totalSalary}", bodyFont, XBrushes.Black, 50, 260);
                gfx.DrawString($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", bodyFont, XBrushes.Gray, 50, 290);

                // Draw a border around the content
                gfx.DrawRectangle(XPens.Gray, 40, 90, page.Width - 80, 240); // Content border

                // Optional footer
                gfx.DrawLine(XPens.Black, 50, page.Height - 70, page.Width - 50, page.Height - 70);
                gfx.DrawString("Thank you for your service!", bodyFont, XBrushes.Gray, 50, page.Height - 50);

                // Save the document
                document.Save(pdfPath);
                document.Close();

                MessageBox.Show("PDF generated successfully at " + pdfPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message);
            }
        }

        private void TotalBasicHoursTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
