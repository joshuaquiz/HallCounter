using System.Threading.Tasks;

namespace HallCounter.Logic.Interfaces
{
	public interface IPlayer
	{

		int GetId();

		Task MovePlayer();

		int GetPosition();

		int GetDirection();

	}
}