﻿using System;

namespace RepoDashboard.Services.Models;

public class BranchDetails
{
    public class Branch
    {
        public string name { get; set; }
        public Commit commit { get; set; }
        public _Links _links { get; set; }
        public bool _protected { get; set; }
        public Protection protection { get; set; }
        public string protection_url { get; set; }
    }

    public class Commit
    {
        public string sha { get; set; }
        public string node_id { get; set; }
        public Commit1 commit { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string comments_url { get; set; }
        public Author1 author { get; set; }
        public Committer1 committer { get; set; }
        public Parent[] parents { get; set; }
    }

    public class Commit1
    {
        public Author author { get; set; }
        public Committer committer { get; set; }
        public string message { get; set; }
        public Tree tree { get; set; }
        public string url { get; set; }
        public int comment_count { get; set; }
        public Verification verification { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime date { get; set; }
    }

    public class Committer
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime date { get; set; }
    }

    public class Tree
    {
        public string sha { get; set; }
        public string url { get; set; }
    }

    public class Verification
    {
        public bool verified { get; set; }
        public string reason { get; set; }
        public string signature { get; set; }
        public string payload { get; set; }
    }

    public class Author1
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Committer1
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Parent
    {
        public string sha { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
    }

    public class _Links
    {
        public string self { get; set; }
        public string html { get; set; }
    }

    public class Protection
    {
        public bool enabled { get; set; }
        public Required_Status_Checks required_status_checks { get; set; }
    }

    public class Required_Status_Checks
    {
        public string enforcement_level { get; set; }
        public string[] contexts { get; set; }
        public Check[] checks { get; set; }
    }

    public class Check
    {
        public string context { get; set; }
        public object app_id { get; set; }
    }
}