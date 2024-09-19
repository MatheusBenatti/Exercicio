using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        using HttpClient client = new();
        int totalGoals = 0;
        int page = 1;

        while (true)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";
            var response = await client.GetStringAsync(url);
            
            JObject data = JObject.Parse(response);

            foreach (var match in data["data"])
            {
                totalGoals += (int)match["team1goals"];
            }

            url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={page}";
            response = await client.GetStringAsync(url);
            data = JObject.Parse(response);

            foreach (var match in data["data"])
            {
                totalGoals += (int)match["team2goals"];
            }

            int totalPages = (int)data["total_pages"];
            if (page >= totalPages) break;
            page++;
        }
        return totalGoals;
    }
}