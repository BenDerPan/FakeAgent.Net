using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeAgent.Net
{
    public class FakeAgent
    {
        static readonly Random _random = new Random();

        /// <summary>
        /// Try load source
        /// </summary>
        /// <param name="useLocal">true: use local data source, false: use online data source</param>
        /// <returns></returns>
        public static async Task<bool> TryLoadSource(bool useLocal = false)
        {
            if (useLocal)
            {
                return OnlineSource.Instance.TryOfflineLoad();
            }
            else
            {
                return await OnlineSource.Instance.TryUpdateSourceAsync();
            }
        }

        /// <summary>
        /// Try load user defined source form json file
        /// </summary>
        /// <param name="jsonFile">custom json file</param>
        /// <returns></returns>
        public static async Task<bool> TryLoadCustomSource(string jsonFile)
        {
            bool isOk= OnlineSource.Instance.TryLoadCustom(jsonFile);
            return await Task.FromResult(isOk);
        }

        /// <summary>
        /// All agents with browser
        /// </summary>
        public static Dictionary<string, List<string>> BrowerAgents => OnlineSource.Instance.Source.Browsers;

        /// <summary>
        /// get a random agent
        /// </summary>
        public static string RandomAgent 
        {
            get
            {
                if (OnlineSource.Instance.Source==null)
                {
                    throw new NullReferenceException("Fake Agent not can not get data source from local file or update onlie, use TryLoadSource(true) to update online.");
                }
                var browserKey = _random.Next(0, OnlineSource.Instance.Source.Randomize.Count).ToString();
                var browser = OnlineSource.Instance.Source.Randomize[browserKey];
                var agentIndex = _random.Next(0, OnlineSource.Instance.Source.Browsers[browser].Count);
                return OnlineSource.Instance.Source.Browsers[browser][agentIndex];
            }
        }
    }
}
