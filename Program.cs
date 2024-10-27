using System;
using System.Collections.Generic;
using System.IO;

namespace EMS
{
    class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Dept { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfHire { get; set; }

        public override string ToString()
        {
            return $"{ID},{Name},{Dept},{Position},{Salary},{DateOfHire:yyyy-MM-dd}";
        }

    }

    class Program
    {
        static List<Employee> Employees = new List<Employee>();
        static string filePath = "employees.csv";

        static void Main(string[] args)
        {
            LoadEmployeesFromCSV();
            while (true)
            {
                DisplayMenu();
              
                string response = Console.ReadLine();
                switch (response)
                {
                    case "1":
                        AddNewEmployee();
                        break;
                    case "2":
                        UpdateEmployee();
                        break;
                    case "3":
                        DeleteEmployee();
                        break;
                    case "4":
                        ViewAllEmployees();
                        break;
                    case "5":
                        SearchEmployee();
                        break;
                    case "6":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid selection, please try again.");
                        break;
                }
            }
        }

        static void LoadEmployeesFromCSV()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No existing employee data found.");
                return;
            }

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Employee emp = new Employee
                    {
                        ID = int.Parse(values[0]),
                        Name = values[1],
                        Dept = values[2],
                        Position = values[3],
                        Salary = double.Parse(values[4]),
                        DateOfHire = DateTime.Parse(values[5])
                    };
                    Employees.Add(emp);
                }
            }

            Console.WriteLine($"{Employees.Count} employees loaded from file.");
        }

        static void SaveEmployeesToCSV()
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var emp in Employees)
                {
                    writer.WriteLine(emp.ToString());  // Write employee data in CSV format
                }
            }

            Console.WriteLine("Employee data saved to file.");
        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("███████╗███╗   ███╗███████╗");
            Console.WriteLine("██╔════╝████╗ ████║██╔════╝");
            Console.WriteLine("█████╗  ██╔████╔██║███████╗");
            Console.WriteLine("██╔══╝  ██║╚██╔╝██║╚════██║");
            Console.WriteLine("███████╗██║ ╚═╝ ██║███████║");
            Console.WriteLine("╚══════╝╚═╝     ╚═╝╚══════╝");
            Console.WriteLine();
            Console.WriteLine("Employee Management System - John Carlo A. Almero");
            Console.WriteLine();
            Console.WriteLine("1. Add New Employee");
            Console.WriteLine("2. Update Employee");
            Console.WriteLine("3. Delete Employee");
            Console.WriteLine("4. View All Employees");
            Console.WriteLine("5. Search Employee");
            Console.WriteLine("6. Exit");
            Console.WriteLine();
            Console.Write("Select: ");
        }

        static void AddNewEmployee()
        {
            Employee emp = new Employee
            {
                ID = Employees.Count + 1, // Auto-incremented ID
                Name = GetInput("Please enter employee Name: "),
                Dept = GetInput("Please enter employee Department: "),
                Position = GetInput("Please enter employee Position: "),
                Salary = GetValidDoubleInput("Please enter employee Salary: "),
                DateOfHire = GetValidDateInput("Please enter Date of Hire (YYYY-MM-DD): ")
            };

            Employees.Add(emp);
            Console.WriteLine("Employee added successfully!");
            SaveEmployeesToCSV();  // Save employees after adding
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }


        static void UpdateEmployee()
        {
            int id = GetValidIntInput("Enter Employee ID to update: ");
            Employee emp = FindEmployeeById(id);
            if (emp == null)
            {
                Console.WriteLine("Employee not found!");
                return;
            }

            emp.Name = GetInput("Enter new name (or press Enter to keep current): ", emp.Name);
            emp.Dept = GetInput("Enter new department (or press Enter to keep current): ", emp.Dept);
            emp.Position = GetInput("Enter new position (or press Enter to keep current): ", emp.Position);
            emp.Salary = GetValidDoubleInput("Enter new salary (or press Enter to keep current): ", emp.Salary);
            emp.DateOfHire = GetValidDateInput("Enter new Date of Hire (or press Enter to keep current): ", emp.DateOfHire);

            Console.WriteLine("Employee updated successfully!");
            SaveEmployeesToCSV();  // Save employees after updating
        }

        static void DeleteEmployee()
        {
            int id = GetValidIntInput("Enter Employee ID to delete: ");
            Employee emp = FindEmployeeById(id);
            if (emp != null)
            {
                Employees.Remove(emp);
                Console.WriteLine("Employee deleted successfully!");
                SaveEmployeesToCSV();  // Save employees after deletion
            }
            else
            {
                Console.WriteLine("Employee not found!");
            }
        }

        static void ViewAllEmployees()
        {
            if (Employees.Count == 0)
            {
                Console.WriteLine("No employees to display.");
                return;
            }

            // Header for the table
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine("| {0,-5} | {1,-20} | {2,-15} | {3,-15} | {4,-10} | {5,-12} |",
                              "ID", "Name", "Department", "Position", "Salary", "Date of Hire");
            Console.WriteLine("--------------------------------------------------------------------------------------------");

            // Display each employee in table format
            foreach (Employee emp in Employees)
            {
                Console.WriteLine("| {0,-5} | {1,-20} | {2,-15} | {3,-15} | {4,-10} | {5,-12} |",
                                  emp.ID, emp.Name, emp.Dept, emp.Position, emp.Salary.ToString("F2"), emp.DateOfHire.ToShortDateString());
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();  // Pauses the output
        }

        static void SearchEmployee()
        {
            if (Employees.Count == 0)
            {
                Console.WriteLine("No employees to search.");
                return;
            }

            string name = GetInput("Enter employee name to search: ");

            var foundEmployees = Employees.FindAll(e => e.Name.ToLower().Contains(name.ToLower()));

            if (foundEmployees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            Console.WriteLine("Search Results:");
            foreach (var emp in foundEmployees)
            {
                Console.WriteLine($"ID: {emp.ID}, Name: {emp.Name}, Department: {emp.Dept}, Position: {emp.Position}, Salary: {emp.Salary}, Date of Hire: {emp.DateOfHire.ToShortDateString()}");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();  // Pauses the output
        }

        static Employee FindEmployeeById(int id)
        {
            return Employees.Find(e => e.ID == id);
        }

        static string GetInput(string prompt, string defaultValue = "")
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }

        static int GetValidIntInput(string prompt)
        {
            int value;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out value) || value <= 0)
            {
                Console.Write("Invalid input. " + prompt);
            }
            return value;
        }

        static double GetValidDoubleInput(string prompt, double defaultValue = -1)
        {
            double value;
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (double.TryParse(input, out value) && value >= 0)
            {
                return value;
            }
            return defaultValue != -1 ? defaultValue : GetValidDoubleInput(prompt);
        }

        static DateTime GetValidDateInput(string prompt, DateTime defaultValue = default)
        {
            DateTime value;
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (DateTime.TryParse(input, out value))
            {
                return value;
            }
            return defaultValue != default ? defaultValue : GetValidDateInput(prompt);
        }
    }

  
}
