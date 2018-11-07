using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HallCounter.Logic.Interfaces;

namespace HallCounter.Logic.Implementations
{
	public sealed class Stats : IStats
	{

		private readonly Action<string> logger;
		private readonly Action<string, string> statsLogger;

		private DateTime? startDateTime;
		private DateTime? endDateTime;

		private readonly IList<IPlayerMove> playerMoves;
		private readonly IList<ITwoPlayerEvent> playerMeets;
		private readonly IList<ITwoPlayerEvent> playerPasses;

		private Stats(Action<string> logger, Action<string, string> statsLogger)
		{
			this.logger = logger;
			this.statsLogger = statsLogger;
			playerMoves = new List<IPlayerMove>();
			playerMeets = new List<ITwoPlayerEvent>();
			playerPasses = new List<ITwoPlayerEvent>();
		}

		public static IStats Create(Action<string> logger, Action<string, string> statsLogger) =>
			new Stats(logger, statsLogger);

		public void StartGame()
		{
			if (startDateTime.HasValue)
				logger("Cannot start game, it was already started");
			startDateTime = DateTime.Now;
			logger($"Game started at {startDateTime}");
		}

		public void LogStats(IBoard board)
		{
			var meets = GetPlayerMeets();
			var passes = GetPlayerPasses();
			statsLogger(
				string.Join(
					", ",
					$"Total player moves: {GetPlayerMoves().Count}",
					$"Total player meets: {meets.Count}",
					$"Unique player meets: {GetUniqueEvents(meets)}",
					$"Total player passes: {passes.Count}",
					$"Unique player passes: {GetUniqueEvents(passes)}"),
				PrintBoard(board));
		}

		private static int GetUniqueEvents(IEnumerable<ITwoPlayerEvent> eventItems)
		{
			var unique = new List<ITwoPlayerEvent>();
			foreach (var eventItem in eventItems)
			{
				if (!unique.Contains(eventItem)
				    && !unique.Any(x => x.GetPlayerAt2() == eventItem.GetPlayerAt1()
				                        && x.GetPlayerAt1() == eventItem.GetPlayerAt2()))
				{
					unique.Add(eventItem);
				}
			}
			return unique.Count;
		}

		private static string PrintBoard(IBoard board)
		{
			var players = board.GetAllPlayersCurrentlyOnBoard()
				.ToLookup(x => x.GetPosition());
			var boardLeftStr = new char[board.GetSize()];
			for (var i = 0; i < board.GetSize(); i++)
			{
				if (players[i].Any(x => x.GetDirection() == -1))
				{
					boardLeftStr[i] = players[i].Count() == 1
						? '<'
						: '«';
				}
				else
				{
					boardLeftStr[i] = '-';
				}
			}
			var boardNotMovingStr = new char[board.GetSize()];
			for (var i = 0; i < board.GetSize(); i++)
			{
				if (players[i].Any(x => x.GetDirection() == 0))
				{
					boardNotMovingStr[i] = '=';
				}
				else
				{
					boardNotMovingStr[i] = '-';
				}
			}
			var boardRightStr = new char[board.GetSize()];
			for (var i = 0; i < board.GetSize(); i++)
			{
				if (players[i].Any(x => x.GetDirection() == 1))
				{
					boardRightStr[i] = players[i].Count() == 1
						? '>'
						: '»';
				}
				else
				{
					boardRightStr[i] = '-';
				}
			}
			return new string(boardLeftStr) + Environment.NewLine +
			       new string(boardNotMovingStr) + Environment.NewLine +
			       new string(boardRightStr);
		}

		public IReadOnlyCollection<IPlayerMove> GetPlayerMoves() =>
			new ReadOnlyCollection<IPlayerMove>(
				playerMoves);

		public void AddPlayerMove(IPlayerMove playerMove)
		{
			playerMoves.Add(playerMove);
			logger($"Player {playerMove.GetPlayer().GetId()} moved from {playerMove.GetStartingPosition()} to {playerMove.GetEndingPosition()}");
		}

		public IReadOnlyCollection<ITwoPlayerEvent> GetPlayerMeets() =>
			new ReadOnlyCollection<ITwoPlayerEvent>(
				playerMeets);

		public void AddPlayerMeet(ITwoPlayerEvent playerMeet)
		{
			playerMeets.Add(playerMeet);
			logger($"Player {playerMeet.GetPlayerAt1().GetId()} was at {playerMeet.GetPosition1()} when they meet {playerMeet.GetPlayerAt2().GetId()} who was at {playerMeet.GetPosition2()}");
		}

		public IReadOnlyCollection<ITwoPlayerEvent> GetPlayerPasses() =>
			new ReadOnlyCollection<ITwoPlayerEvent>(
				playerPasses);

		public void AddPlayerPass(ITwoPlayerEvent playerPass)
		{
			playerPasses.Add(playerPass);
			logger($"Player {playerPass.GetPlayerAt1().GetId()} was at {playerPass.GetPosition1()} when they passed {playerPass.GetPlayerAt2().GetId()} who was at {playerPass.GetPosition2()}");
		}

		public void EndGame()
		{
			if (endDateTime.HasValue)
				logger("Cannot end game, it was already ended");
			endDateTime = DateTime.Now;
			logger($"Game ended at {endDateTime}");
		}
	}
}