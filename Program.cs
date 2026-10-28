
using EFCOREPractice.DataHelper;
using EFCOREPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCOREPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using var context = new AppDbContext();

                // Check database connection
                if (!context.Database.CanConnect())
                {
                    Console.WriteLine("Database connection failed!");
                    return;
                }

                Console.WriteLine("Database connected successfully!");

                while (true)
                {
                    Console.WriteLine("\n===== DEPARTMENT MANAGEMENT =====");
                    Console.WriteLine("1. Create Department");
                    Console.WriteLine("2. Display All Departments");
                    Console.WriteLine("3. Search Department");
                    Console.WriteLine("4. Update Department");
                    Console.WriteLine("5. Delete Department");
                    Console.WriteLine("6. Exit");
                    Console.Write("Enter your choice: ");

                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CreateDepartment(context);
                            break;

                        case "2":
                            DisplayDepartments(context);
                            break;

                        case "3":
                            SearchDepartment(context);
                            break;

                        case "4":
                            UpdateDepartment(context);
                            break;

                        case "5":
                            DeleteDepartment(context);
                            break;

                        case "6":
                            Console.WriteLine("Application closed.");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // CREATE
        static void CreateDepartment(AppDbContext context)
        {
            Console.Write("Enter department name: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Department name is required.");
                return;
            }

            if (name.Length > 100)
            {
                Console.WriteLine("Department name cannot exceed 100 characters.");
                return;
            }

            Console.Write("Enter location: ");
            string? location = Console.ReadLine();

            if (location?.Length > 100)
            {
                Console.WriteLine("Location cannot exceed 100 characters.");
                return;
            }

            var department = new Department
            {
                DepartmentName = name.Trim(),
                Location = string.IsNullOrWhiteSpace(location)
                    ? null
                    : location.Trim(),
                IsActive = true
            };

            context.Departments.Add(department);
            context.SaveChanges();

            Console.WriteLine(
                $"Department created successfully! ID: {department.DepartmentId}");
        }

        // READ ALL
        static void DisplayDepartments(AppDbContext context)
        {
            var departments = context.Departments
                .AsNoTracking()
                .OrderBy(d => d.DepartmentId)
                .ToList();

            if (departments.Count == 0)
            {
                Console.WriteLine("No departments found.");
                return;
            }

            Console.WriteLine("\n----- DEPARTMENT LIST -----");

            foreach (var department in departments)
            {
                Console.WriteLine(
                    $"ID: {department.DepartmentId} | " +
                    $"Name: {department.DepartmentName} | " +
                    $"Location: {department.Location ?? "Not specified"} | " +
                    $"Active: {department.IsActive}");
            }
        }

        // READ ONE
        static void SearchDepartment(AppDbContext context)
        {
            int id = ReadDepartmentId();
            if (id <= 0)
                return;

            var department = context.Departments
                .AsNoTracking()
                .FirstOrDefault(d => d.DepartmentId == id);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            Console.WriteLine("\nDepartment Details");
            Console.WriteLine($"ID: {department.DepartmentId}");
            Console.WriteLine($"Name: {department.DepartmentName}");
            Console.WriteLine($"Location: {department.Location ?? "Not specified"}");
            Console.WriteLine($"Active: {department.IsActive}");
        }

        // UPDATE
        static void UpdateDepartment(AppDbContext context)
        {
            int id = ReadDepartmentId();
            if (id <= 0)
                return;

            var department = context.Departments
                .FirstOrDefault(d => d.DepartmentId == id);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            Console.WriteLine(
                $"Current name: {department.DepartmentName}");
            Console.Write("Enter new department name: ");

            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Department name is required.");
                return;
            }

            if (name.Length > 100)
            {
                Console.WriteLine("Department name cannot exceed 100 characters.");
                return;
            }

            Console.WriteLine(
                $"Current location: {department.Location ?? "Not specified"}");
            Console.Write("Enter new location (leave blank for none): ");

            string? location = Console.ReadLine();

            if (location?.Length > 100)
            {
                Console.WriteLine("Location cannot exceed 100 characters.");
                return;
            }

            Console.Write("Is department active? (y/n): ");
            string? activeChoice = Console.ReadLine();

            if (!string.Equals(activeChoice, "y",
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(activeChoice, "n",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter y or n.");
                return;
            }

            department.DepartmentName = name.Trim();
            department.Location = string.IsNullOrWhiteSpace(location)
                ? null
                : location.Trim();
            department.IsActive = string.Equals(
                activeChoice, "y", StringComparison.OrdinalIgnoreCase);

            context.SaveChanges();

            Console.WriteLine("Department updated successfully!");
        }

        // DELETE
        static void DeleteDepartment(AppDbContext context)
        {
            int id = ReadDepartmentId();
            if (id <= 0)
                return;

            var department = context.Departments
                .FirstOrDefault(d => d.DepartmentId == id);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            Console.Write(
                $"Delete '{department.DepartmentName}'? (y/n): ");

            string? confirmation = Console.ReadLine();

            if (!string.Equals(confirmation, "y",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            context.Departments.Remove(department);
            context.SaveChanges();

            Console.WriteLine("Department deleted successfully!");
        }

        // INPUT VALIDATION
        static int ReadDepartmentId()
        {
            Console.Write("Enter department ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("Enter a valid positive department ID.");
                return 0;
            }

            return id;
        }
    }
}
