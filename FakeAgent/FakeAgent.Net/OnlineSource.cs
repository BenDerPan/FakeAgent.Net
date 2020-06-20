using AngleSharp;
using FakeAgent.Net.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAgent.Net
{
    public class OnlineSource
    {
        public static readonly string EmbedCachePath = Path.GetFullPath("./Cache/herokuapp.source-0.1.11.json");
        public static readonly string OnlineCachePath = Path.GetFullPath("./Cache/online-source.json");
        public static readonly Dictionary<string, string> DefaultBrowserNameDict = new Dictionary<string, string>() {
            { "chrome","chrome" },
            {"opera","opera" },
            {"firefox","firefox" },
            { "edge","edge" },
            { "safari","safari" },
            { "ie","Internet+Explorer" }
        };
        const string DefaultSourceUrlFormat = "http://useragentstring.com/pages/useragentstring.php?name={0}";

        const string SecondarySourceUrl = "https://fake-useragent.herokuapp.com/browsers/0.1.11";

        public Agent Source { get; private set; }

        readonly static OnlineSource _instance;
        public static OnlineSource Instance => _instance;
        static OnlineSource()
        {
            _instance = new OnlineSource();
        }
        private OnlineSource()
        {
            TryOfflineLoad();
        }

        /// <summary>
        /// Try update agent source online
        /// </summary>
        /// <returns>success returns true, otherwise false.</returns>
        public async Task<bool> TryUpdateSourceAsync()
        {
            var tempSource = new Agent();
            tempSource.Randomize = new Dictionary<string, string>();

            tempSource.Browsers = new Dictionary<string, List<string>>();
            try
            {
                foreach (var browserKey in DefaultBrowserNameDict.Keys)
                {
                    if (!tempSource.Browsers.ContainsKey(browserKey))
                    {
                        tempSource.Browsers.Add(browserKey, new List<string>());
                    }
                    //Crawl from online source.
                    var config = Configuration.Default.WithDefaultLoader();
                    var address = GetDefaultSourceUrlByBrowser(DefaultBrowserNameDict[browserKey]);
                    var context = BrowsingContext.New(config);
                    var document = await context.OpenAsync(address);
                    var cellSelector = "li a";
                    var cells = document.QuerySelectorAll(cellSelector);
                    var agentList = cells.Select(m => m.TextContent).ToList();
                    tempSource.Browsers[browserKey].AddRange(agentList);
                    var startIndexKey = tempSource.Randomize.Count;

                    for (int i = startIndexKey; i < startIndexKey + agentList.Count; i++)
                    {
                        tempSource.Randomize.Add($"{i}", browserKey);
                    }
                }

                Source = tempSource;

                using (FileStream stream = new FileStream(OnlineCachePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(JsonConvert.SerializeObject(tempSource));
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Try update agent source offline
        /// </summary>
        /// <returns>success returns true, otherwise false.</returns>
        public bool TryOfflineLoad()
        {
            try
            {
                if (File.Exists(OnlineCachePath))
                {
                    var tempSource = Agent.LoadFromJson(File.ReadAllText(OnlineCachePath));
                    if (tempSource != null)
                    {
                        Source = tempSource;
                        return true;
                    }
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                if (File.Exists(EmbedCachePath))
                {
                    var tempSource = Agent.LoadFromJson(File.ReadAllText(EmbedCachePath));
                    if (tempSource != null)
                    {
                        Source = tempSource;
                        return true;
                    }
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        public bool TryLoadCustom(string jsonFile)
        {
            try
            {
                if (File.Exists(jsonFile))
                {
                    var tempSource = Agent.LoadFromJson(File.ReadAllText(jsonFile));
                    if (tempSource != null)
                    {
                        Source = tempSource;
                        return true;
                    }
                }
            }

            catch (Exception e)
            {

            }

            return false;
        }

        string GetDefaultSourceUrlByBrowser(string browser)
        {
            return string.Format(DefaultSourceUrlFormat, browser);
        }




    }
}
