public class ApiClient
{
    private readonly ApiAuthService _auth;

    public ApiClient(ApiAuthService auth)
    {
        _auth = auth;
    }

    public async Task<string> GetAsync(string endpoint)
    {
        return $"Simulated GET {endpoint} with token {_auth.JwtToken}";
    }
}
