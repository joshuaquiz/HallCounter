namespace HallCounter.Logic.Interfaces
{
	public interface ITwoPlayerEvent
	{

		int GetPosition1();

		int GetPosition2();

		IPlayer GetPlayerAt1();

		IPlayer GetPlayerAt2();

	}
}