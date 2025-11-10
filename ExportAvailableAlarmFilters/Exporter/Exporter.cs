namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.Exporter
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Newtonsoft.Json;
	using SharedCode.JsonClass;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Utils.SecureCoding.SecureIO;

	public class Exporter
	{
		private static readonly string ExportFolderPath = "C:\\DataMiner\\Documents\\AlarmFilterExports";

		public static void CreateExport(IEngine engine, string exportName, Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> alarmsToExport)
		{
			if (alarmsToExport == null || alarmsToExport.Count == 0)
			{
				engine.GenerateInformation("[CreateExport] No alarms to export — skipping JSON generation.");
				return;
			}

			GenerateFile(engine, exportName, alarmsToExport);
		}

		private static void GenerateFile(IEngine engine, string exportName, Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> alarmsToExport)
		{
			string exportFolder = SecurePath.CreateSecurePath(ExportFolderPath);

			try
			{
				Directory.CreateDirectory(exportFolder);
				engine.GenerateInformation($"[GenerateFile] Created export folder: {exportFolder}");

				string jsonFilePath = SecurePath.ConstructSecurePath(exportFolder, $"{exportName}.json");

				var exportObject = new AlarmExport
				{
					ExportName = exportName,
					Timestamp = DateTime.UtcNow,
					AlarmCount = alarmsToExport.Count,
					Alarms = alarmsToExport.Select(a => new AlarmExportEntry
					{
						Name = a.Key.Name,
						UserContext = a.Key.Owner,
						IsShared = a.Key.IsShared,
						FilterJson = JsonConvert.SerializeObject(a.Value.Filter, Formatting.None),
					}).ToList(),
				};

				string jsonContent = JsonConvert.SerializeObject(exportObject, Formatting.Indented);
				File.WriteAllText(jsonFilePath, jsonContent);

				engine.GenerateInformation($"[GenerateFile] Successfully exported {alarmsToExport.Count} alarms to '{jsonFilePath}'.");
			}
			catch (Exception e)
			{
				engine.GenerateInformation($"[GenerateFile] Failed to create export directory or export alarms: {e}");
			}
		}
	}
}