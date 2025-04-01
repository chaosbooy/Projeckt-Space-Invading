using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

public static class SavingSystem
{
    public static List<(string, int)> Scores { get; private set; }
    private const string FilePath = "HighScores.json";

    //Initializes the Scores list since the class is static
    public static void Initialize()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            Scores = JsonConvert.DeserializeObject<List<(string, int)>>(json) ?? new List<(string, int)>();
            return;
        }


        Scores = new List<(string, int)>();
    }

    // Sorts the Scores list from highest to lowest score
    public static void SortScores()
    {
        Scores = Scores.OrderByDescending(score => score.Item2).ToList();
    }

    // Adds score to the list so that it remains sorted
    public static void AddScore(string name, int score)
    {
        var newScore = (name, score);

        // Find the correct position to insert while maintaining order
        int index = Scores.BinarySearch(newScore, Comparer<(string, int)>.Create((a, b) => b.Item2.CompareTo(a.Item2)));

        if (index < 0)
            index = ~index; // Convert to insertion index

        Scores.Insert(index, newScore);
        SaveScores();
    }

    // Saves the scores to a file
    public static void SaveScores()
    {
        var json = JsonConvert.SerializeObject(Scores);
        File.WriteAllText(FilePath, json);
    }
}