namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using SharedCode.Comparers;
	using SharedCode.SLNet.Alarm;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Net.Messages;

	public class ChooseAlarmModel
	{
		private Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> availableAlarmFilters;
		private Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> alarmsToExport;

		public ChooseAlarmModel(IEngine engine)
		{
			this.Engine = engine;
		}

		public IEngine Engine { get; set; }

		public string ExportName { get; internal set; }

		/// <summary>
		/// Gets or Sets the alarm filters that the user has chosen to export.
		/// Initially loaded as all available filters.
		/// </summary>
		public Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> AlarmsToExport
		{
			get
			{
				return alarmsToExport ?? new Dictionary<AlarmFilterMeta, GetAlarmFilterResponse>(AvailableAlarmFilters, new AlarmFilterComparer());
			}

			set
			{
				alarmsToExport = value;
			}
		}

		/// <summary>
		/// Gets the collection of all available alarm filters from the system.
		/// </summary>
		public Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> AvailableAlarmFilters
		{
			get
			{
				return availableAlarmFilters ?? GetAvailableAlarmFilters();
			}
		}

		private Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> GetAvailableAlarmFilters()
		{
			try
			{
				var response = AlarmFilters.GetAvailableAlarmFilters(Engine);
				if (response == null)
				{
					Engine.GenerateInformation("[GetAvailableAlarmFilters] No alarm filters received.");
					return new Dictionary<AlarmFilterMeta, GetAlarmFilterResponse>();
				}

				var filters = response.Filters ?? Array.Empty<AlarmFilterMeta>();
				var sharedFilters = response.SharedFilters ?? Array.Empty<AlarmFilterMeta>();

				var allMetadataFilters = filters.Concat(sharedFilters);
				availableAlarmFilters = BuildAvailableFilters(allMetadataFilters);

				return availableAlarmFilters;
			}
			catch (Exception ex)
			{
				Engine.GenerateInformation($"[GetAvailableAlarmFilters] Failed to retrieve alarm filters: {ex}");
				throw;
			}
		}

		private Dictionary<AlarmFilterMeta, GetAlarmFilterResponse> BuildAvailableFilters(IEnumerable<AlarmFilterMeta> allFilters)
		{
			var availableFilters = new Dictionary<AlarmFilterMeta, GetAlarmFilterResponse>(new AlarmFilterComparer());

			foreach (var alarm in allFilters)
			{
				try
				{
					var filterResponse = AlarmFilters.GetAlarmFilterResponse(Engine, alarm.Key);
					if (filterResponse != null)
					{
						availableFilters[alarm] = filterResponse;
					}
					else
					{
						Engine.GenerateInformation($"[BuildAvailableFilters] Null response for filter '{alarm.Name}' (Key={alarm.Key}).");
					}
				}
				catch (Exception innerEx)
				{
					Engine.GenerateInformation($"[BuildAvailableFilters] Error retrieving details for filter '{alarm.Name}': {innerEx}");
				}
			}

			return availableFilters;
		}
	}
}