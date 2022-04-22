using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RepoDashboard.Services.Models;

namespace RepoDashboard.Services;

public class GithubProxy
{
    private const string TeamReposUrlTemplate = "/orgs/{0}/teams/{1}/repos";
    private const string BranchDetailsUrlTemplate = "/repos/{0}/branches/{1}";
    private const string PullsUrlTemplate = "/repos/{0}/pulls?base={1}";
    private const string BranchesTemplate = "/repos/{0}/branches";
    private readonly HttpClient _client;

    private readonly TeamSettings _teamSettings;

    public GithubProxy(HttpClient client, IOptions<TeamSettings> teamSettings)
    {
        _client = client;
        _teamSettings = teamSettings.Value;
    }

    public async Task<IEnumerable<TeamRepos.Repository>> GetTeamRepositoriesAsync()
    {
        var url = string.Format(TeamReposUrlTemplate, _teamSettings.Org, _teamSettings.TeamSlug);

        var teamRepositories = await GetPagedDataAsync<TeamRepos.Repository>(url);

        return teamRepositories
               .Where(t => !_teamSettings.ExcludeRepos.Contains(t.name))
               .ToList();
    }

    public async Task<BranchDetails.Branch> GetBranchDetailsAsync(string repository, string branch)
    {
        var url = string.Format(BranchDetailsUrlTemplate,
                                repository,
                                branch);

        try
        {
            var response = await _client.GetFromJsonAsync<BranchDetails.Branch>(url);
            return response;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<Pulls.PullRequest>> GetPullRequestsAsync(string repository)
    {
        var url = string.Format(PullsUrlTemplate, repository, _teamSettings.DefaultBranch);

        var pulls = await GetPagedDataAsync<Pulls.PullRequest>(url);

        return pulls;
    }

    public async Task<IEnumerable<Branches.Branch>> GetBranchesAsync(string repository)
    {
        var url = string.Format(BranchesTemplate, repository);

        var branches = await GetPagedDataAsync<Branches.Branch>(url);

        return branches;
    }
    
    private async Task<IEnumerable<T>> GetPagedDataAsync<T>(string url)
    {
        var data = new List<T>();

        do
        {
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var page = await response.Content.ReadFromJsonAsync<T[]>();

            if (page != null)
            {
                data.AddRange(page);
            }

            url = GetNextPageUrl(response);
        } while (url != null);

        return data;
    }


    private static string GetNextPageUrl(HttpResponseMessage response)
    {
        var success = response.Headers.TryGetValues("Link", out var links);
        if (!success)
        {
            return null;
        }

        var link = links.FirstOrDefault();
        var linkHeader = LinkHeader.Parse(link);
        return linkHeader?.NextLink;
    }
}