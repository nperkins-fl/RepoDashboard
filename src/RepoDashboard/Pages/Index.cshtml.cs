using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepoDashboard.Models;
using RepoDashboard.Services;

namespace RepoDashboard.Pages;

public class IndexModel : PageModel
{
    private readonly GithubProxy _githubProxy;

    public IndexModel(GithubProxy githubProxy)
    {
        _githubProxy = githubProxy;
    }

    public IEnumerable<Repository> Repositories { get; set; }

    [BindProperty]
    public DateTime? FilterDate { get; set; }
        
    [BindProperty]
    public string ReleaseBranch { get; set; }

    public void OnGet()
    {

    }
    public async Task OnPostAsync()
    {
        var repos = await _githubProxy.GetTeamRepositoriesAsync();

        var list = new List<Repository>();

        foreach (var repo in repos)
        {
            var lastCommitDate = await GetLastCommitDateAsync(repo.full_name);

            if (lastCommitDate < FilterDate || lastCommitDate == null)
            {
                continue;
            }

            var openPullRequestCount = await GetOpenPullRequestCountAsync(repo.full_name);
            var hasReleaseBranch = await GetHasReleaseBranchAsync(repo.full_name);

            var item = new Repository
                       {
                           Name = repo.name,
                           Url = repo.html_url,
                           LastCommitDate = lastCommitDate,
                           OpenPullRequestCount = openPullRequestCount,
                           HasReleaseBranch = hasReleaseBranch
                       };

            list.Add(item);
        }

        Repositories = list.OrderBy(x => x.Name).ToList();
    }

    private async Task<bool> GetHasReleaseBranchAsync(string repository)
    {
        var branchDetails = await _githubProxy.GetBranchDetailsAsync(repository, ReleaseBranch);

        return branchDetails != null;
    }

    private async Task<int> GetOpenPullRequestCountAsync(string repository)
    {
        var pullRequests = await _githubProxy.GetPullRequestsAsync(repository);

        return pullRequests.Count();
    }

    private async Task<DateTime?> GetLastCommitDateAsync(string repository)
    {
        var branchDetails = await _githubProxy.GetBranchDetailsAsync(repository, "develop");

        return branchDetails?.commit.commit.author.date;
    }
}