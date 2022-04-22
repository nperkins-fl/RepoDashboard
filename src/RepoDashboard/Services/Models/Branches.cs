namespace RepoDashboard.Services.Models
{
    public class Branches
    {
        public class Rootobject
        {
            public Branch[] Property1 { get; set; }
        }

        public class Branch
        {
            public string name { get; set; }
            public Commit commit { get; set; }
            public bool _protected { get; set; }
            public Protection protection { get; set; }
            public string protection_url { get; set; }
        }

        public class Commit
        {
            public string sha { get; set; }
            public string url { get; set; }
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
}
