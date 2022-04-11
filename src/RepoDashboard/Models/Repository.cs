using System;

namespace RepoDashboard.Models;

public class Repository
{
    public string Name { get; set; }
    public DateTime? LastCommitDate { get; set; }
    public int OpenPullRequestCount { get; set; }
    public string Url { get; set; }
    public bool HasReleaseBranch { get; set; }
}