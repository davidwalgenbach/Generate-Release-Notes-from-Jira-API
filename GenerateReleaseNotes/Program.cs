using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
namespace IOnotification_System
{
    public class Issue
    {
        public string key { get; set; }
        public Fields fields { get; set; }
        public Issue()
        {
            fields = new Fields();
        }
    }
    /// <summary>
    /// Getter and Setter methods for the components of a Jira item.
    /// </summary>
    public class Fields
    {
        public Fields()
        {
            issuetype = new IssueType();
        }
        public string summary { get; set; }
        public IssueType issuetype { get; set; }
        public string description { get; set; }
    }
    public class IssueType
    {
        public string name { get; set; }
    }

    public class IssueList
    {
        public List<Issue> issues { get; set; }
    }
    /// <summary>
    /// The main class used to run the program. 
    /// Set your values below to the intended values for the Jira Item you would like to create. 
    /// </summary>
    public class Version
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class VersionList
    {
        public IList<Version> versions { get; set; }
    }

    public class run
    {
        public static void Main()
        {
            Console.WriteLine(run.GetItems(run.GetVersions()));
        }
        /// <summary>
        /// This method performs a GET request on the Jira Server to pull down each version and its associated ID. 
        /// </summary>
        /// <returns>
        /// This method returns the latest Version ID listed within Jira.
        /// </returns>
        public static string GetVersions()
        {
            string getUrl = "getURL";
            string requestUrl = $"requestURL";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(getUrl);
            byte[] cred = UTF8Encoding.UTF8.GetBytes("EMAIL:API_Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            HttpResponseMessage response = client.GetAsync(requestUrl).Result;
            if(response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<dynamic>(jsonString);
                //Console.WriteLine("These are the versions contained within the sampleTrax project");
                //Console.WriteLine("");
                List<string> versionIDs = new List<string>();
                foreach(var d in json)
                {
                    versionIDs.Add((string)d.id);
                    //Console.WriteLine("ID: {0}", d.id);
                    //Console.WriteLine("Name: {0}", d.name);
                    //Console.WriteLine("");
                }
                int listSize = (versionIDs.Count-1);
                return (versionIDs[listSize]);
            }
            else
            {
                return ("Get has failed");
            }

        }
        /// <summary>
        /// This method performs a GET request on the Jira server to pull down all issues that have a fixVersion that match the input fixVersion (string versionID).
        /// </summary>
        /// <param name="versionID"></param>
        /// <returns>
        /// This method returns a formatted string to the console which depicts the Identifier, Summary, and Issue Type of the issues linked to the input fixVersion (Primitive version of Release Notes). 
        /// </returns>
        public static string GetItems(string versionID)
        {
            string getUrl = "getURL";
            string requestUrl = $"requestURL";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(getUrl);
            byte[] cred = UTF8Encoding.UTF8.GetBytes("EMAIL:API_Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            HttpResponseMessage response = client.GetAsync(requestUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<IssueList>(jsonString);
                Console.WriteLine("Release Notes for sampleTrax V4.5.12");
                File.WriteAllText("ReleaseNotes.txt", "Release Notes for sampleTrax V4.5.12");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
                File.AppendAllText("ReleaseNotes.txt", "\n-------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("");
                File.AppendAllText("ReleaseNotes.txt", "\n");
                foreach (var d in json.issues)
                {
                    Console.WriteLine("Identifier: {0}", d.key);
                    File.AppendAllText("ReleaseNotes.txt", "\nIdentifier: " + d.key);
                    Console.WriteLine("");
                    File.AppendAllText("ReleaseNotes.txt", "\n");
                    Console.WriteLine("Summary: {0}", d.fields.summary);
                    File.AppendAllText("ReleaseNotes.txt", "\nSummary: " + d.fields.summary);
                    Console.WriteLine("");
                    File.AppendAllText("ReleaseNotes.txt", "\n");
                    Console.WriteLine("Issue Type: {0}", d.fields.issuetype.name);
                    File.AppendAllText("ReleaseNotes.txt", "\nIssue Type: " + d.fields.issuetype.name);
                    Console.WriteLine("");
                    File.AppendAllText("ReleaseNotes.txt", "\n");
                    //Console.WriteLine("Description: {0}", d.fields.description);
                    //Console.WriteLine("");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
                    File.AppendAllText("ReleaseNotes.txt", "\n-------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("");
                    File.AppendAllText("ReleaseNotes.txt", "\n");
                }
                return null;
            }
            else
            {
                return ("GET has failed.");
            }
        }
    }
}