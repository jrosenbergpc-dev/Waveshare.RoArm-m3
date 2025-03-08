using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waveshare.RoArm_m3
{
	public class JsonCommand
	{
		private string _rawjson;

		public string Command { get; set; }
		public string RawOutput { get => _rawjson; }
		public JObject JsonOutput { get => JObject.Parse(_rawjson); }

		public JsonCommand(string commandName, string rawjson)
		{
			Command = commandName;
			_rawjson = rawjson;
		}

		public JsonCommand(string commandName, JObject json)
		{
			Command = commandName;
			_rawjson = json.ToString();
		}
	}
}
