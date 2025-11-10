namespace SharedCode.SLNet.Alarm
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Net.Messages;

	public class AlarmFilters
	{
		public static AvailableAlarmFiltersResponse GetAvailableAlarmFilters(IEngine engine)
		{
			var availableAlarmFilters = new GetAvailableAlarmFiltersMessage();
			return engine.SendSLNetSingleResponseMessage(availableAlarmFilters) as AvailableAlarmFiltersResponse;
		}

		public static GetAlarmFilterResponse GetAlarmFilterResponse(IEngine engine, string key)
		{
			var alarmFilter = new GetAlarmFilterMessage
			{
				Key = key,
			};

			return engine.SendSLNetSingleResponseMessage(alarmFilter) as GetAlarmFilterResponse;
		}

		public static void UpdateAlarmFilter(
			IEngine engine,
			AlarmFilter filterDefinition,
			string oldName,
			string oldUserContext,
			UpdateAlarmFilterMessage.UpdateType type)
		{
			var alarmFilterToUpdate = new UpdateAlarmFilterMessage
			{
				FilterDefinition = filterDefinition,
				Type = type,
				OldName = oldName,
				OldUserContext = oldUserContext,
			};

			engine.SendSLNetSingleResponseMessage(alarmFilterToUpdate);
		}
	}
}