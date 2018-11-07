using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallCounter.Logic.Interfaces;

namespace HallCounter.Logic.Implementations
{
	public sealed class Game : IGame
	{

		private readonly int rate;
		private readonly IStats stats;
		private readonly IBoard board;

		private Game(int rate, IStats stats, IBoard board, IEnumerable<IPlayer> players)
		{
			this.rate = rate;
			this.stats = stats;
			this.board = board;
			foreach (var player in players)
			{
				this.board.AddPlayer(player);
			}
		}

		public static IGame Create(int rate, IStats stats, IBoard board, IEnumerable<IPlayer> players) =>
			new Game(rate, stats, board, players);

		public async Task Run()
		{
			stats.StartGame();
			while (board.StillHasMovingPlayers())
			{
				await Task.Delay(TimeSpan.FromMilliseconds(rate));
				await Task.WhenAll(board.GetMovingPlayersCurrentlyOnBoard().Select(x => x.MovePlayer()));
				board.PostMoveAnalytics();
				stats.LogStats(board);
			}
			stats.EndGame();
		}

	}
}