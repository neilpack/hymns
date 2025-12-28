using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        // Ensure the file exists before starting
        string filePath = "hymns.csv";
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: Could not find '{filePath}' in the current directory.");
            return;
        }

        // Calculate total lines minus header for validation
        int totalHymns = File.ReadAllLines(filePath).Length - 1;

        Hymn hymnManager = new Hymn(filePath);

        Console.Clear();

        int quantity = 0;
        while (quantity <= 0 || quantity > totalHymns)
        {
            Console.Write($"Enter the number of random hymns to select (1-{totalHymns}):  ");
            
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                quantity = result;
            }

            if (quantity <= 0 || quantity > totalHymns)
            {
                Console.Clear();
                Console.WriteLine($"Please enter a valid number between 1 and {totalHymns}.");
                Thread.Sleep(3000);
                Console.Clear();
            }
        }
        Console.WriteLine("");

        //What mode are we in? (Sacrament, Christmas, or Easter)
        Console.WriteLine("What mode are we in? (Sacrament, Christmas, Easter, or None)");
        string mode = Console.ReadLine();
        switch (mode.ToLower())
        {
            case "sacrament":
                Console.WriteLine("You selected Sacrament mode.");
                hymnManager.SelectSacrament(quantity);
                break;
            case "christmas":
                Console.WriteLine("You selected Christmas mode.");
                hymnManager.SelectChristmas(quantity);
                break;
            case "easter":
                Console.WriteLine("You selected Easter mode.");
                hymnManager.SelectEaster(quantity);
                break;
            case "none":
                Console.WriteLine("No mode selected. Proceeding with default settings.");
                hymnManager.SelectRandom(quantity);
                break;
            default:
                Console.WriteLine("Unknown mode selected. Proceeding with default settings.");
                hymnManager.SelectRandom(quantity);
                break;
        }
    }
}