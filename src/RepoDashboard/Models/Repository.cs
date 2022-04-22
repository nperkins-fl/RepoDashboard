using System;

namespace RepoDashboard.Models;

public class Repository
{
    public string Name { get; init; }
    public DateTime? LastCommitDate { get; init; }
    public int OpenPullRequestCount { get; init; }
    public string Url { get; init; }
    public bool HasReleaseBranch { get; init; }
    public int BranchCount { get; init; }
}