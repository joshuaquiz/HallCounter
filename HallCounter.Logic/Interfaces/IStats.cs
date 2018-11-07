using System.Collections.Generic;

namespace HallCounter.Logic.Interfaces
{
	public interface IStats
	{

		void StartGame();

		void LogStats(IBoard board);

		IReadOnlyCollection<IPlayerMove> GetPlayerMoves();

		void AddPlayerMove(IPlayerMove playerMove);

		IReadOnlyCollection<ITwoPlayerEvent> GetPlayerMeets();

		void AddPlayerMeet(ITwoPlayerEvent playerMeet);

		IReadOnlyCollection<ITwoPlayerEvent> GetPlayerPasses();

		void AddPlayerPass(ITwoPlayerEvent playerPass);

		void EndGame();

	}
}