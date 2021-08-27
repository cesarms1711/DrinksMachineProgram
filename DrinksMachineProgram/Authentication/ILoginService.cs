
namespace DrinksMachineProgram.Authentication
{

    public interface IAuthenticationService
    {

        bool ValidateUser(string userName, string password);

    }

}
