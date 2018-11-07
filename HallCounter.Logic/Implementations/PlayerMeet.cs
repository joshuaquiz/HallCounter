using HallCounter.Logic.Interfaces;

namespace HallCounter.Logic.Implementations
{
	public sealed class PlayerMeet : ITwoPlayerEvent
	{

		private readonly int position1;
		private readonly IPlayer playerAt1;
		private readonly int position2;
		private readonly IPlayer playerAt2;

		private PlayerMeet(
			int position1,
			IPlayer playerAt1,
			int position2,
			IPlayer playerAt2)
		{
			this.position1 = position1;
			this.playerAt1 = playerAt1;
			this.position2 = position2;
			this.playerAt2 = playerAt2;
		}

		public static ITwoPlayerEvent Create(
			int position1,
			IPlayer playerAt1,
			int position2,
			IPlayer playerAt2) =>
			new PlayerMeet(
				position1,
				playerAt1,
				position2,
				playerAt2);

		public int GetPosition1() =>
			position1;

		public int GetPosition2() =>
			position2;

		public IPlayer GetPlayerAt1() =>
			playerAt1;

		public IPlayer GetPlayerAt2() =>
			playerAt2;

	}
}