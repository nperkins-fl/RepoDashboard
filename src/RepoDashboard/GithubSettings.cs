using System.Collections.Generic;

namespace RepoDashboard;

public class AuthenticationSettings
{
    public string AccessToken { get; set; }
    public string UserAgent { get; set; }
}
public class TeamSettings
{
    public string TeamSlug { get; set; }
    public string Org { get; set; }
    public IEnumerable<string> ExcludeRepos { get; set; }
    public string DefaultBranch { get; set; }
}