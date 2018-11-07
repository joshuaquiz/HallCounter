using System.Collections.Generic;

namespace HallCounter.Logic.Interfaces
{
	public interface IBoard
	{

		int GetSize();

		void AddPlayer(IPlayer player);

		bool StillHasMovingPlayers();

		IReadOnlyCollection<IPlayer> GetMovingPlayersCurrentlyOnBoard();

		IReadOnlyCollection<IPlayer> GetAllPlayersCurrentlyOnBoard();

		void PostMoveAnalytics();

	}
}