using DrinksMachineProgram.Entities;

using System.Threading.Tasks;

namespace DrinksMachineProgram.Authentication
{

    public interface ILoginManager
    {

        Task Login(User user);

        Task Logout();

    }

}
