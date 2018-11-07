using System.Threading.Tasks;
using HallCounter.Logic.Interfaces;

namespace HallCounter.Logic.Implementations
{
	public sealed class Player : IPlayer
	{

		private readonly IStats stats;
		private readonly int id;
		private readonly int rate;
		private int position;

		private Player(IStats stats, int id, int rate, int startingPosition)
		{
			this.stats = stats;
			this.id = id;
			this.rate = rate;
			position = startingPosition;
		}

		public static IPlayer Create(IStats stats, int id, int rate, int startingPosition) =>
			new Player(stats, id, rate, startingPosition);

		public int GetId() =>
			id;

		public Task MovePlayer()
		{
			position += rate;
			stats.AddPlayerMove(PlayerMove.Create(this, position - rate, position));
			return Task.CompletedTask;
		}

		public int GetPosition() =>
			position;

		public int GetDirection() =>
			rate;

	}
}