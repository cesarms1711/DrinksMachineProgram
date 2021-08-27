
namespace DrinksMachineProgram.Authentication
{

    public interface IUserSession
    {

        bool IsAuthenticated { get; }

        int Id { get; }

        string FirstName { get; }

        string LastName { get; }

        string UserName { get; }

        string FullName { get; }

    }

}