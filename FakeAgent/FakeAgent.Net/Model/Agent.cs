using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeAgent.Net.Model
{
    public class Agent
    {
        public Dictionary<string,List<string>> Browsers { get; set; }

        public Dictionary<string,string> Randomize { get; set; }

        public static Agent LoadFromJson(string jsonStr)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<Agent>(jsonStr);
                return item;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
