namespace Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.Context
{
	using System;
	using System.IO;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.JsonClass;
	using Skyline.DataMiner.Utils.SecureCoding.SecureIO;
	using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

	public class ScriptContext
	{
		private readonly string jsonFilePath;

		public ScriptContext(IEngine engine)
		{
			Engine = engine;
			jsonFilePath = GetScriptInputData(engine);
		}

		public IEngine Engine { get; }

		public AlarmExport LoadExportFromFile()
		{
			var securePath = SecurePath.CreateSecurePath(jsonFilePath);
			if (!File.Exists(securePath))
			{
				throw new FileNotFoundException($"File not found: {securePath}");
			}

			var json = File.ReadAllText(securePath);

			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
			};

			try
			{
				var export = SecureNewtonsoftDeserialization.DeserializeObject<AlarmExport>(json, settings)
							   ?? throw new InvalidOperationException("Failed to deserialize the export file.");

				export.AlarmCount = export.Alarms?.Count ?? 0;

				return export;
			}
			catch (JsonException ex)
			{
				throw new InvalidOperationException("Invalid JSON format in export file.", ex);
			}
		}

		private string GetScriptInputData(IEngine engine)
		{
			return Convert.ToString(engine.GetScriptParam("FilePathToImport")?.Value);
		}
	}
}