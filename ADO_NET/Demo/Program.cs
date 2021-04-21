using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Data Source=localhost;Database=SoftUniTest;User Id=sa;Password=Password123@jkl#";

            //using (SqlConnection connectionOne = new SqlConnection(connectionString))
            //{
            //    connectionOne.Open();   

            //    SqlCommand command = new SqlCommand("SELECT * FROM Employees", connectionOne);

            //    int nonQuery = command.ExecuteNonQuery();  // NonQuery mostly for CRUD operations, changes to the DB, will return affected rows.
            //                                               // => "UPDATE Employees SET Salary = Salary + 0.24"

            //    var scalar = command.ExecuteScalar();  // Return obj.=> "SELECT COUNT(*) FROM Employees" // "SELECT SUM(Salary) FROM Employees"
            //                                           // Return the first cell of the table as obj. 

            //    Console.WriteLine(scalar);

            //    connectionOne.Close();

            //}

            //using (SqlConnection connectionTwo = new SqlConnection(connectionString))   // => creating the connection
            //{
            //    connectionTwo.Open();   // => open the connection

            //    SqlCommand command = new SqlCommand("SELECT * FROM Employees", connectionTwo);   // => creating sql command

            //    SqlDataReader reader = command.ExecuteReader();     // command has been given to the Sqlreader

            //    reader.Read();       //  => move one row         // same =>  reader.NextResult();

            //                                  // .Read() => return true/false -> is the next row exists
            //    Console.WriteLine(reader[1]); // will return the second column/cell from the current row, starting from 0..1..2...
            //    reader.Read();                // => move one more row
            //    Console.WriteLine(reader[1]);
            //    reader.Read();                // => move one more row
            //    Console.WriteLine(reader[1]);
            //    reader.Read();                // => move one more row
            //    Console.WriteLine(reader[1]);
            //    reader.Read();                // => move one more row
            //    Console.WriteLine(reader[1]);

            //    Console.WriteLine(reader["JobTitle"]);   // => using column name 

            //    while (reader.Read())       
            //    {
            //        Console.WriteLine($"{reader[0]} / {reader[1]} /{reader[2]} /{reader[3]} /{reader[4]} /");  // => concat columns
            //    }

            //    connectionTwo.Close();
            //}

            //using (SqlConnection connectionThree = new SqlConnection(connectionString))
            //{
            //    connectionThree.Open();

            //    string sqlQuery = "SELECT e.FirstName, a.AddressText FROM Employees AS e JOIN Addresses AS a ON a.AddressID = e.AddressID";

            //    SqlCommand command = new SqlCommand(sqlQuery, connectionThree);

            //    using (SqlDataReader reader = command.ExecuteReader())  // SqlDataReader => should also be used in using due to his usage of resources
            //    {
            //        while (reader.Read())
            //        {
            //            Console.WriteLine($"{reader[0]} / {reader[1]}");
            //        }

            //        reader.Close();          //   => more options to close the reader except => using 
            //        reader.DisposeAsync();   //// => more options to close the reader except => using 
            //    }

            //    // => SqlDataReader => return obj which need to be cast in order to be used. 
            //    // => The reader always needs to be closed before we use another one.

            //    connectionThree.Close();

            //}


            using (SqlConnection sqlInjectionConnection = new SqlConnection(connectionString))
            {
                sqlInjectionConnection.Open();

                Console.Write("Enter Name: ");
                var name = Console.ReadLine();
                Console.Write("Enter LastName: ");
                var passowrd = Console.ReadLine();

                //string sqlQuery = "SELECT [FirstName],[LastName],[Salary] FROM Employees WHERE [LastName] = 'Gilbert'";

                //SqlCommand command = new SqlCommand        // =>  SQL injection threat => not validating user data
                //    ($"SELECT [FirstName],[LastName],[Salary] FROM Employees WHERE [LastName] = '{lastName}'",
                //    sqlInjectionConnection);

                SqlCommand command = new SqlCommand
                ($"SELECT [FirstName],[LastName],[Salary] FROM Employees WHERE [LastName] = @Password",
                sqlInjectionConnection);

                command.Parameters.Add(new SqlParameter("@Password", passowrd));  

                var result = command.ExecuteScalar();
                    
                if (result != null)
                {
                    Console.WriteLine("Access granted: ");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]}/{reader[1]}/{reader[2]}/");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Access denied");
                }
               

            }

            //    List<Employee> employees = new List<Employee>();

            //using (SqlConnection connectionFour = new SqlConnection(connectionString))
            //{
            //    string sqlQuery = "SELECT e.[FirstName] , e.[LastName], e.[Salary] , d.[Name] FROM Employees AS e JOIN Departments AS d ON d.DepartmentID = e.DepartmentID";

            //    connectionFour.Open();

            //    SqlCommand command = new SqlCommand(sqlQuery, connectionFour);

            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            var employee = new Employee
            //            {
            //                FirstName = reader[0] as string,
            //                LastName = reader[1] as string,
            //                Salary = reader[2] as decimal?,
            //                DepartmentName = reader[3] as string
            //            };

            //            employees.Add(employee);

            //        }

            //    }

            //}

            //Console.WriteLine(string.Join("\n", employees));

        }

        //public class Employee
        //{
        //    public string FirstName { get; set; }

        //    public string LastName { get; set; }

        //    public decimal? Salary { get; set; }

        //    public string DepartmentName { get; set; }

        //    public override string ToString()
        //    {
        //        return $"{this.FirstName} / {this.LastName} / {this.Salary} / {this.DepartmentName}";
        //    }
        //}
    }
}
