using System.Threading.Tasks;

public interface IUserSessionManager
{
    Task<bool> CheckSessionAsync();
    Task CreateSessionAsync(string userName, ProblemHeaderMetadata problemHeaderMetadata);
    UserData GetSession();
}