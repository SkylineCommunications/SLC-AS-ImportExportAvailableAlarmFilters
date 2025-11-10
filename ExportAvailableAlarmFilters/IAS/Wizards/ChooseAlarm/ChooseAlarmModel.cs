namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using SharedCode.Comparers;
	using SharedCode.SLNet.Alarm;
	using Skyline.DataMiner.Net.Filters;

	public class ChooseAlarmModel
	{
		private ICollection<AlarmFilterMeta> availableAlarmFilters;

		public ChooseAlarmModel(IEngine engine)
		{
			this.Engine = engine;
			this.AlarmsToExport = AvailableAlarmFilters.ToHashSet(new AlarmFilterComparer());
		}

		public IEngine Engine { get; set; }

		public string ExportName { get; internal set; }

		public HashSet<AlarmFilterMeta> AlarmsToExport { get; set; }

		public ICollection<AlarmFilterMeta> AvailableAlarmFilters
		{
			get
			{
				return availableAlarmFilters ?? GetAvailableAlarmFilters();
			}
		}

		private ICollection<AlarmFilterMeta> GetAvailableAlarmFilters()
		{
			try
			{
				var response = AlarmFilters.GetAvailableAlarmFilters(Engine);
				if (response == null)
				{
					Engine.GenerateInformation("[GetAvailableAlarmFilters] No alarm filters received.");
					return Array.Empty<AlarmFilterMeta>();
				}

				var filters = response.Filters ?? Array.Empty<AlarmFilterMeta>();
				var sharedFilters = response.SharedFilters ?? Array.Empty<AlarmFilterMeta>();

				availableAlarmFilters = filters.Concat(sharedFilters).ToList();
				return availableAlarmFilters;
			}
			catch (Exception ex)
			{
				Engine.GenerateInformation($"[GetAvailableAlarmFilters] Failed to retrieve alarm filters: {ex}");
				throw;
			}
		}
	}
}