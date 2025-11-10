namespace Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.JsonClass
{
	using System;
	using System.Collections.Generic;

	public class AlarmExport
	{
		public string ExportName { get; set; }

		public DateTime Timestamp { get; set; }

		public int AlarmCount { get; set; }

		public List<AlarmFilterInfo> Alarms { get; set; } = new List<AlarmFilterInfo>();
	}

	public class AlarmFilterInfo
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string Owner { get; set; }

		public int Version { get; set; }

		public string Key { get; set; }

		public string AccessType { get; set; }

		public string CreatedBy { get; set; }

		public bool IsShared { get; set; }
	}
}