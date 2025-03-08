using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Waveshare.RoArm_m3
{
	public static class CommandDefinitions
	{
		public static List<string> GetAvailableCommands()
		{
			return typeof(CommandDefinitions)
		.GetMethods(BindingFlags.Public | BindingFlags.Static)
		.Where(m => m.Name != nameof(GetAvailableCommands) && m.Name != nameof(ExecuteCommandByName)) // Exclude itself
		.Select(m => m.Name)
		.ToList();
		}

		public static JsonCommand ExecuteCommandByName(string commandName, params object[] parameters)
		{
			MethodInfo method = typeof(CommandDefinitions)
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.FirstOrDefault(m => m.Name == commandName);

			if (method == null)
			{
				throw new ArgumentException($"No command found with the name: {commandName}");
			}

			// Check if the method requires parameters
			ParameterInfo[] methodParams = method.GetParameters();
			if (methodParams.Length != parameters.Length)
			{
				throw new ArgumentException($"Incorrect number of parameters for method {commandName}. Expected {methodParams.Length}, got {parameters.Length}");
			}

			// Invoke the method dynamically
			return (JsonCommand)method.Invoke(null, parameters);
		}

		public static JsonCommand EmergencyStop()
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 0);

			return new JsonCommand("Emergency Stop", xTemp);
		}

		public static JsonCommand EmergencyReset()
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 999);

			return new JsonCommand("Emergency Reset", xTemp);
		}

		public static JsonCommand InitPosition()
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 100);

			return new JsonCommand("Init Position", xTemp);
		}

		public static JsonCommand LightContol(int brightness)
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 114);
			xTemp.Add("led", Math.Clamp(brightness, 0, 255));

			return new JsonCommand("Light Control", xTemp);
		}

		public static JsonCommand SingleJointMovement(int joint, double degrees, int speed, int acceleration)
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 101);
			xTemp.Add("joint", Math.Clamp(joint, 1, 4));
			xTemp.Add("rad", AngleConverter.DegreesToRadians(degrees));
			xTemp.Add("spd", speed);
			xTemp.Add("acc", acceleration);

			return new JsonCommand("Single Joint Command", xTemp);
		}

		public static JsonCommand HandControl(int degrees, int speed, int acceleration)
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 106);
			xTemp.Add("cmd", AngleConverter.DegreesToRadians(Math.Clamp(degrees, 90, 180)));
			xTemp.Add("spd", speed);
			xTemp.Add("acc", acceleration);

			return new JsonCommand("Hand Control", xTemp);
		}

		public static JsonCommand HandTorque(int torque)
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 107);
			xTemp.Add("tor", Math.Clamp(torque, 1, 1000));

			return new JsonCommand("Hand Torque", xTemp);
		}

		public static JsonCommand Delay(int miliseconds)
		{
			JObject xTemp = new JObject();
			xTemp.Add("T", 111);
			xTemp.Add("cmd", miliseconds);

			return new JsonCommand("Delay", xTemp);
		}
	}
}
