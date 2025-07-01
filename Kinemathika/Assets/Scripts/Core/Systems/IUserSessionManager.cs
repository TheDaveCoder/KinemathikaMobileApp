using System.Threading.Tasks;

public interface IUserSessionManager
{
    Task InitializeAsync();
    Task<bool> CheckSessionAsync();
    Task CreateSessionAsync(string userName, ProblemHeaderMetadata problemHeaderMetadata);
    UserData GetSession();
}