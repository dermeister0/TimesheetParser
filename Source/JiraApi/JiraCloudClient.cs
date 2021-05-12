using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace JiraApi
{
    public class JiraCloudClient : ICrm
    {
        private string login;
        private string password;
        private readonly Regex taskRegex = new Regex(@"^([A-z0-9]+)-\d+$", RegexOptions.Compiled);

        private JiraSettings jiraSettings;

        public string GetName()
        {
            return "JIRA Cloud";
        }

        public bool IsValidTask(string taskId)
        {
            return taskRegex.IsMatch(taskId);
        }

        private HttpClient GetClient()
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}")));
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };
            return client;
        }

        public async Task<bool> Login(string login, string password)
        {
            LoadSettings();

            this.login = login;
            this.password = password;

            foreach (var projectPattern in jiraSettings.Projects)
            {
                using (var client = GetClient())
                {
                    var response = await client.GetAsync(projectPattern.JiraInstance + "myself");
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Task<bool> Login(string token)
        {
            return Task.FromResult(false);
        }

        private JiraInstanceBinding GetJiraBinding(string taskId)
        {
            var project = taskRegex.Match(taskId).Groups[1].Value;

            // TODO: Add cache.

            foreach (var projectPattern in jiraSettings.Projects)
            {
                if (projectPattern.CachedRegex.IsMatch(project))
                {
                    return projectPattern;
                }
            }

            throw new Exception($"Jira binding not defined: {project}");
        }

        private string GetApiUrl(string taskId)
        {
            return GetJiraBinding(taskId).JiraInstance;
        }

        public async Task<bool> AddJob(JobDefinition job)
        {
            var poster = GetJobPoster(job.TaskId);

            var url = GetApiUrl(job.TaskId) + $"issue/{job.TaskId}/worklog";
            var body = new WorkLog() { timeSpent = $"{job.Duration}m", comment = job.Description, started = job.Date.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00") };

            using (var client = GetClient())
            {
                var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = JsonConvert.DeserializeObject<WorkLogResponse>(await response.Content.ReadAsStringAsync());
                    var id = content.id; // @@

                    return true;
                }
            }

            return false;
        }

        private IJobPoster GetJobPoster(string taskId)
        {
            var binding = GetJiraBinding(taskId);

            if (!string.IsNullOrEmpty(binding.TempoToken))
            {
                return new TempoJobPoster();
            }
            else
            {
                return new JiraJobPoster(binding.JiraInstance);
            }
        }

        public async Task<TaskHeader> GetTaskHeader(string taskId)
        {
            var url = GetApiUrl(taskId) + $"issue/{taskId}?fields=summary";
            var title = string.Empty;

            using (var client = GetClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = JsonConvert.DeserializeObject<IssueSummaryResponse>(await response.Content.ReadAsStringAsync());
                    title = content.fields["summary"];
                }
            }

            return new TaskHeader() { Title = title };
        }

        private string GetSettingsFileName()
        {
            return Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "TimesheetParser", "JiraApi.json");
        }

        private void LoadSettings()
        {
            var fileName = GetSettingsFileName();
            if (!File.Exists(fileName))
            {
                jiraSettings = new JiraSettings { Projects = new List<JiraInstanceBinding>() };
                return;
            }

            jiraSettings = JsonConvert.DeserializeObject<JiraSettings>(File.ReadAllText(fileName));

            foreach (var projectPattern in jiraSettings.Projects)
            {
                projectPattern.CachedRegex = new Regex(projectPattern.ProjectRegex, RegexOptions.Compiled);
            }
        }

        private void SaveSettings()
        {
            File.WriteAllText(GetSettingsFileName(), JsonConvert.SerializeObject(jiraSettings));
        }
    }
}
