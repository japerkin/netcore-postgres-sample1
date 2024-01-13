// This code sample connects to a postgres database, performs some
// operations, and then closes the connection.
//
// ran into:
// System.InvalidOperationException: "Connection property has not been initialized"
// 
// The cause if this exception was due to the fact a command was being run
// without the secondary connection parameter being passed to "NpgsqlCommand".
//
//
//
using System;
using Npgsql;


namespace PostgresSample1;

class Program
{
    // Obtain connection string information for database.
    private static string Host = "";
    private static string User = "";
    private static string DBName = "";
    private static string Password = "";
    private static string Port = "";


    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Building connection string...");

        // Build connection string
        string connectionString = String.Format(
            "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                Host,
                User,
                DBName,
                Port,
                Password
            );

        // Create a connection.
        using (var connection = new NpgsqlConnection(connectionString))
        {

            Console.Out.WriteLine("Opening connection...");
            connection.Open();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Successfully connected to database.");
            } else
            {
                Console.WriteLine("Error: Unable to connect to database.");
                return;
            }

            using (var dropTableCommand = new NpgsqlCommand("DROP TABLE IF EXISTS people", connection))
            {
                dropTableCommand.ExecuteNonQuery();
                Console.Out.WriteLine("Finished dropping table (if existed).");
            }

            using (var createTableCommand = new NpgsqlCommand("CREATE TABLE people(id serial PRIMARY KEY, fname VARCHAR(50), lname VARCHAR(50))", connection))
            {
                createTableCommand.ExecuteNonQuery();
                Console.Out.WriteLine("Finished creating table.");
            }

            using (var insertValuesCommand = new NpgsqlCommand("INSERT INTO people (fname, lname) VALUES (@f1, @l1), (@f2, @l2), (@f3, @l3)", connection))
            {
                insertValuesCommand.Parameters.AddWithValue("f1", "Jacob");
                insertValuesCommand.Parameters.AddWithValue("l1", "Perkins");

                insertValuesCommand.Parameters.AddWithValue("f2", "Ryan");
                insertValuesCommand.Parameters.AddWithValue("l2", "Parks");

                insertValuesCommand.Parameters.AddWithValue("f3", "Elon");
                insertValuesCommand.Parameters.AddWithValue("l3", "Musk");

                int numRows = insertValuesCommand.ExecuteNonQuery();
                Console.Out.WriteLine(String.Format("Number of rows inserted={0}", numRows));
            }
        }

        Console.WriteLine("Press RETURN to exit.");
        Console.ReadLine();
    }
}

