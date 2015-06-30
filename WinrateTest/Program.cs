using RiotSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WinrateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = RiotApi.GetInstance(ConfigurationManager.AppSettings["ApiKey"]);
            var staticApi = StaticRiotApi.GetInstance(ConfigurationManager.AppSettings["ApiKey"]);
            var statusApi = StatusRiotApi.GetInstance();
            int id = int.Parse(ConfigurationManager.AppSettings["Summoner1Id"]);
            string name = ConfigurationManager.AppSettings["Summoner1Name"];
            int id2 = int.Parse(ConfigurationManager.AppSettings["Summoner2Id"]);
            string name2 = ConfigurationManager.AppSettings["Summoner2Name"];
            string team = ConfigurationManager.AppSettings["Team1Id"];
            string team2 = ConfigurationManager.AppSettings["Team2Id"];
            int gameId = int.Parse(ConfigurationManager.AppSettings["GameId"]);
            Region region = (Region)Enum.Parse(typeof(Region), ConfigurationManager.AppSettings["Region"]);

            var languages = staticApi.GetLanguages(region);

            Console.WriteLine(string.Join(", ", languages));

            var summ = api.GetSummoner(region, name);

            var teams = summ.GetTeams();

            var match1 = api.GetMatch(region, gameId);

            Console.WriteLine(match1.MapType);

            var shards = statusApi.GetShards();

            var shardStatus = statusApi.GetShardStatus(region);

            var statSummaries = api.GetStatsSummaries(region, id);

            var championsList = new List<RiotSharp.ChampionEndpoint.Champion>(api.GetChampions(region));
            var championsMap = new Dictionary<long, championStats>();
            foreach (var champ in championsList)
            {
                championStats champStats = new championStats(champ);
                championsMap.Add(champ.Id, champStats);
                
            }

            var championIds = new List<int>();
            //This will get 30 matches in two rounds of 15
            for (int i = 0; i < 30; i += 15)
            {
                var matches = api.GetMatchHistory(region, id, i, i + 15, null,
                    new List<Queue>() { Queue.RankedSolo5x5 });
                foreach (var match in matches)
                {
                    //championIds.Add(match.Participants[0].ChampionId);
                    foreach (var player in match.Participants)
                    {
                        long playedChampID = player.ChampionId;
                        championsMap[playedChampID].gamesPlayed++;
                        if (player.Stats.Winner)
                        {
                            championsMap[playedChampID].gamesWon++;
                        }
                    }

                }
            }
            
            //var mostPlayedChampId = championIds.GroupBy(c => c).OrderByDescending(g => g.Count()).FirstOrDefault().Key;
            //var mostPlayedChamp = staticApi.GetChampion(region, mostPlayedChampId);
            //Console.WriteLine(mostPlayedChamp.Name);

            var games = api.GetRecentGames(region, id);

            Console.WriteLine("Done! Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
