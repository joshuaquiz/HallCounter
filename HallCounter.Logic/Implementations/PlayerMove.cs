using HallCounter.Logic.Interfaces;

namespace HallCounter.Logic.Implementations
{
	public sealed class PlayerMove : IPlayerMove
	{

		private readonly IPlayer player;
		private readonly int startPosition;
		private readonly int endPosition;

		private PlayerMove(IPlayer player, int startPosition, int endPosition)
		{
			this.player = player;
			this.startPosition = startPosition;
			this.endPosition = endPosition;
		}

		public static IPlayerMove Create(IPlayer player, int startPosition, int endPosition) =>
			new PlayerMove(player, startPosition, endPosition);

		public IPlayer GetPlayer() =>
			player;

		public int GetStartingPosition() =>
			startPosition;

		public int GetEndingPosition() =>
			endPosition;

	}
}