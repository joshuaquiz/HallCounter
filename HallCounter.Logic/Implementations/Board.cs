using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HallCounter.Logic.Interfaces;

namespace HallCounter.Logic.Implementations
{
	public sealed class Board : IBoard
	{

		private readonly IStats stats;
		private readonly int size;
		private readonly IList<IPlayer> players;

		private Board(IStats stats, int size)
		{
			this.stats = stats;
			this.size = size;
			players = new List<IPlayer>();
		}

		public static IBoard Create(IStats stats, int size) =>
			new Board(stats, size);

		public int GetSize() =>
			size;

		public void AddPlayer(IPlayer player) =>
			players.Add(player);

		public bool StillHasMovingPlayers() =>
			GetMovingPlayersCurrentlyOnBoard()
				.Any();

		public IReadOnlyCollection<IPlayer> GetMovingPlayersCurrentlyOnBoard() =>
			new ReadOnlyCollection<IPlayer>(
				players
					.Where(x => IsOnBoard(x.GetPosition())
					            && x.GetDirection() != 0)
					.ToList());

		public IReadOnlyCollection<IPlayer> GetAllPlayersCurrentlyOnBoard() =>
			new ReadOnlyCollection<IPlayer>(
				players
					.Where(x => IsOnBoard(x.GetPosition()))
					.ToList());

		private bool IsOnBoard(int position) =>
			position >= 0 && position < size;

		public void PostMoveAnalytics()
		{
			var playersByLocation = players.ToLookup(x => x.GetPosition());
			foreach (var meetingPlayers in playersByLocation.Where(x => playersByLocation.Any(y => y.Key == x.Key + 1)))
			{
				foreach (var meetingPlayer in meetingPlayers)
				{
					if (meetingPlayer.GetDirection() < 0)
						continue;
					foreach (var player in playersByLocation
						.Where(y => y.Key == meetingPlayers.Key + 1)
						.SelectMany(x => x)
						.Where(x => x.GetDirection() < 0))
					{
						stats.AddPlayerMeet(PlayerMeet.Create(
							meetingPlayers.Key,
							meetingPlayer,
							meetingPlayers.Key + 1,
							player));
					}
				}
			}
			foreach (var meetingPlayers in playersByLocation.Where(x => playersByLocation.Any(y => y.Key == x.Key - 1)))
			{
				foreach (var meetingPlayer in meetingPlayers)
				{
					if (meetingPlayer.GetDirection() > 0)
						continue;
					foreach (var player in playersByLocation
						.Where(y => y.Key == meetingPlayers.Key - 1)
						.SelectMany(x => x)
						.Where(x => x.GetDirection() > 0))
					{
						stats.AddPlayerMeet(PlayerMeet.Create(
							meetingPlayers.Key,
							meetingPlayer,
							meetingPlayers.Key - 1,
							player));
					}
				}
			}
			foreach (var passingPlayers in playersByLocation.Where(x => playersByLocation.Any(y => y.Key == x.Key - 1)))
			{
				foreach (var meetingPlayer in passingPlayers)
				{
					if (meetingPlayer.GetDirection() < 0)
						continue;
					foreach (var player in playersByLocation
						.Where(y => y.Key == passingPlayers.Key - 1)
						.SelectMany(x => x)
						.Where(x => x.GetDirection() < 0))
					{
						stats.AddPlayerPass(PlayerPass.Create(
							passingPlayers.Key,
							meetingPlayer,
							passingPlayers.Key - 1,
							player));
					}
				}
			}
			foreach (var passingPlayers in playersByLocation.Where(x => playersByLocation.Any(y => y.Key == x.Key + 1)))
			{
				foreach (var meetingPlayer in passingPlayers)
				{
					if (meetingPlayer.GetDirection() > 0)
						continue;
					foreach (var player in playersByLocation
						.Where(y => y.Key == passingPlayers.Key + 1)
						.SelectMany(x => x)
						.Where(x => x.GetDirection() > 0))
					{
						stats.AddPlayerPass(PlayerPass.Create(
							passingPlayers.Key,
							meetingPlayer,
							passingPlayers.Key + 1,
							player));
					}
				}
			}
			foreach (var passingPlayers in playersByLocation.Where(x => x.Count() > 1))
			{
				foreach (var meetingPlayer in passingPlayers)
				{
					foreach (var player in playersByLocation
						.SelectMany(x => x))
					{
						stats.AddPlayerPass(PlayerPass.Create(
							passingPlayers.Key,
							meetingPlayer,
							passingPlayers.Key + 1,
							player));
					}
				}
			}
			foreach (var playersNotOnBoard in playersByLocation.Where(x => !IsOnBoard(x.Key)))
			{
				foreach (var player in playersNotOnBoard)
				{
					players.Remove(player);
				}
			}
		}

	}
}