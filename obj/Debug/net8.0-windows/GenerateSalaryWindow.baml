   M S B A M L   `   `   `  ����  Salary Management,    &Salary_Management.GenerateSalaryWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Generate Salary��5   	   $	��593��6!   $	��800��6.   .��5      � $��20q�6   .+�5   
   �� $��Calculate Salary��6   $��28(�5      $
��Bold"�5      $��Center=�5	      $	��Top=�5
      $��	0,30,0,20q�5      ���#3335      �� 5   
   
����������#E0E0E0$��1q�5   '    �� CornerRadius$ 10��5      $��0,20,0,0q�6#   ._�5      � $��20q�6    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$    � ColumnDefinitions 5"      �� 5#      $	��150 �6'   �� 5$      $��* �6'   .+�5'      �� $��Select Employee:��6   $��14(�64   $
��0,10q�6B   $��0��6P   $��0��6]   �� 5(      -   5        $��EmployeeComboBox��5(      $	��250��63   $��30��6?   $��14(�6K   $��0��5)      $��1��6(   5*      �� 5,      $��Employee Name:��6   $��14(�62   $
��0,10q�6@   $��1��6N   $��0��6[   �� 5-      -   5        $��EmployeeNameTextBlock��5-      $��14(�69   $��0,0,0,5q�6G   ���#666$��1��5.      $��1��6)   �� 50      $��Basic Salary:��6   $��14(�61   $
��0,10q�6?   $��2��6M   $��0��6Z   �� 51      -   5        $��BasicSalaryTextBox��51      $	��250��64   $��30��6@   $��14(�6L    �� 
IsReadOnly$
 True��52      $��5q�6,   ���#BDBDBD$��1q�53   0   ���#FAFAFA$��2��54      $��1��6'   �� 56      $��Total Basic Hours:��6   $��14(�66   $
��0,10q�6D   $��3��6R   $��0��6_   �� 57      -   5        $��TotalBasicHoursTextBox��57      $	��250��68   $��30��6D   $��14(�6P   $��5q�6^   ���#BDBDBD$��1q�58   0   ���#FAFAFA$��3��59      $��1��6'   67   �� 5;      $��Total Overtime Hours:��6   $��14(�69   $
��0,10q�6G   $��4��6U   $��0��6b   �� 5<      -   5        $��TotalOvertimeHoursTextBox��5<      $	��250��6;   $��30��6G   $��14(�6S   $��5q�6a   ���#BDBDBD$��1q�5=   0   ���#FAFAFA$��4��5>      $��1��6'   �� 5@      $��Deductions:��6   $��14(�6/   $
��0,10q�6=   $��6��6K   $��0��6X   �� 5A      -   5        $��DeductionsTextBox��5A      $	��250��63   $��30��6?   $��14(�6K   $��5q�6Y   ���#BDBDBD$��1q�5B   0   ���#FAFAFA$��6��5C      $��1��6'   67   �� 5E      $��Remaining Loan:��6   $��14(�63   $
��0,10q�6A   $��7��6O   $��0��6\   �� 5F      -   5        $��RemainingLoanTextBlock��5F      $��14(�6:   $��0,0,0,5q�6H   ���#666$��7��5G      $��1��6)   �� 5I      $��Number of Days:��6   $��14(�63   $
��0,10q�6A   $��8��6O   $��0��6\   �� 5J      -   5        $��DaysTextBox��5J      $	��250��6-   $��30��69   $��14(�6E   $��5q�6S   ���#BDBDBD$��1q�5K   0   ���#FAFAFA$��8��5L      $��1��6'   �� 5N      -	   5        $��Calculate Salary��5N      $	��200��64   $��40��6@   ���#4CAF50
�������$��16(�5P      $
��Bold"�6'   $��	20,20,0,0q�5Q      $��9��6,   $��2��69   $��Center=�6M   �� 5S      $��Total Salary:��6   $��16(�61   $
��Bold"�6?   $��0,20,0,5q�6Q   $��10��5T      $��0��6*   �� 5U      -
   5        $��TotalSalaryTextBlock��5U      $��16(�68   $��0,5,0,5q�6F   ���#4CAF50$��10��5V      $��1��6*   �� 5X      -   5        $��Generate PDF��5X      $	��200��60   $��40��6<   ���#2196F3
�������$��16(�5Z      $
��Bold"�6'   $��	20,20,0,0q�5[      $��11��6,   $��2��6:   $��Center=�6N   6k   5\      5]      5^      5_      