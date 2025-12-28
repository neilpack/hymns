using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Hymn
{
    private class HymnData
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string FullLine { get; set; } // The original line for printing
    }

    private List<HymnData> _allHymns = new List<HymnData>();
    private Random _random = new Random();

    public Hymn(string path)
    {
        // Load and Parse the file ONCE when the class is created
        try
        {
            var lines = File.ReadAllLines(path).Skip(1); // Skip header
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length >= 3)
                {
                    _allHymns.Add(new HymnData
                    {
                        Number = parts[0].Trim(),
                        Name = parts[1].Trim(),
                        Category = parts[2].Trim(),
                        FullLine = line
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    // --- MODE 1: STANDARD RANDOM ---
    public void SelectRandom(int quantity)
    {
        var selection = _allHymns
                        .OrderBy(x => _random.Next())
                        .Take(quantity)
                        .ToList();

        DisplayHymns(selection);
    }

    // --- MODE 2: SACRAMENT MODE ---
    public void SelectSacrament(int quantity)
    {
        // 1. Get exactly ONE Sacrament hymn
        var sacramentHymn = _allHymns
                            .Where(h => h.Category.Equals("Sacrament", StringComparison.OrdinalIgnoreCase))
                            .OrderBy(x => _random.Next())
                            .Take(1)
                            .ToList();

        // 2. Define allowed categories for the rest
        string[] allowed = { "Restoration", "Praise and Thanksgiving", "Prayer and Supplication", "Special Topics" };

        // 3. Get the rest of the hymns (quantity - 1)
        int remainingNeeded = quantity - sacramentHymn.Count;
        
        var otherHymns = _allHymns
                         .Where(h => allowed.Contains(h.Category, StringComparer.OrdinalIgnoreCase))
                         .OrderBy(x => _random.Next())
                         .Take(remainingNeeded)
                         .ToList();

        // 4. Combine them
        var finalSelection = sacramentHymn.Concat(otherHymns).ToList();

        DisplayHymns(finalSelection);
    }

    // --- MODE 3: CHRISTMAS MODE ---
    public void SelectChristmas(int quantity)
    {
        // Just filters for Christmas category
        var selection = _allHymns
                        .Where(h => h.Category.Equals("Christmas", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(x => _random.Next())
                        .Take(quantity)
                        .ToList();

        if (selection.Count < quantity)
        {
            Console.WriteLine($"Note: You asked for {quantity} hymns, but there are only {selection.Count} Christmas hymns available.");
        }

        DisplayHymns(selection);
    }

    // --- MODE 4: EASTER MODE ---
    public void SelectEaster(int quantity)
    {
        // Just filters for Easter category
        var selection = _allHymns
                        .Where(h => h.Category.Equals("Easter", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(x => _random.Next())
                        .Take(quantity)
                        .ToList();

        if (selection.Count < quantity)
        {
            Console.WriteLine($"Note: You asked for {quantity} hymns, but there are only {selection.Count} Easter hymns available.");
        }

        DisplayHymns(selection);
    }

    // --- HELPER TO DISPLAY HYMNS ---
    private void DisplayHymns(List<HymnData> hymns)
    {
        Console.WriteLine("\n--- Selected Hymns ---");
        foreach (var h in hymns)
        {
            Console.WriteLine($"#{h.Number}: {h.Name} ({h.Category})");
        }
    }
}