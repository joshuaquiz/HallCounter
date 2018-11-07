namespace HallCounter.Logic.Interfaces
{
	public interface IPlayerMove
	{

		IPlayer GetPlayer();

		int GetStartingPosition();

		int GetEndingPosition();

	}
}