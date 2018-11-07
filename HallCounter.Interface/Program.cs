using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HallCounter.Logic.Implementations;
using HallCounter.Logic.Interfaces;

namespace HallCounter.Interface
{
	internal class Program
	{

		private static async Task Main(string[] args)
		{
			var logs = new Dictionary<DateTime, string>();
			var stats = Stats.Create(
				l =>
				{
					logs.Add(DateTime.Now, l);
					Console.SetCursorPosition(0, 5);
					foreach (var log in logs.OrderByDescending(x => x.Key).Take(30))
					{
						Console.WriteLine(log.Value.PadRight(100));
					}
				},
				(logStr, boardStr) =>
				{
					Console.SetCursorPosition(0, 0);
					Console.WriteLine(logStr);
					Console.WriteLine(boardStr);
				});
			var board = Board.Create(stats, await GetInt("How long in the hall?"));
			var game = Game.Create(
				await GetInt("How long does each turn take (in milliseconds)?"),
				stats,
				board,
				await GetPlayers(stats, board.GetSize()));
			await game.Run();
			Console.ReadLine();
		}

		private static Task<int> GetInt(string str, int min = 0, int max = int.MaxValue) =>
			Task.Run(() =>
			{
				int result;
				Console.Clear();
				Console.WriteLine(str);
				while (!IsValidInt(Console.ReadLine(), min, max, out result))
				{
					Console.Clear();
					Console.WriteLine(str);
				}
				return result;
			});

		private static bool IsValidInt(string str, int min, int max, out int result) =>
			int.TryParse(str, out result)
			&& result > min
			&& result <= max
			&& Regex.IsMatch(str, $"^(?:\\-)?\\d{{1,{max}}}$");

		private static Task<List<IPlayer>> GetPlayers(IStats stats, int boardSize) =>
			Task.Run(async () =>
			{
				var players = new List<IPlayer>();
				int addPlayer;
				do
				{
					Console.Clear();
					Console.WriteLine();
					players.Add(
						Player.Create(
							stats,
							players.Count,
							await GetInt("Player movement (-1: Backwards, 0: Stationary, 1: Forward):", -2, 1),
							await GetInt("Where does this player start?", -1, boardSize)));
					Console.Clear();
					Console.WriteLine();
					addPlayer = await GetInt("Add another player (0: No, 1: Yes)?", -1, 1);
				} while (addPlayer == 1);
				return players;
			});
	}
}