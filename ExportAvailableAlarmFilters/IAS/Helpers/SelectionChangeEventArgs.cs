namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Helpers
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class SelectionChangeEventArgs : EventArgs
	{
		public SelectionChangeEventArgs(
			KeyValuePair<AlarmFilterMeta, GetAlarmFilterResponse> alarmFilter,
			CheckBox.CheckBoxChangedEventArgs checkBoxEventArgs)
		{
			AlarmFilter = alarmFilter;
			IsChecked = checkBoxEventArgs.IsChecked;
		}

		public KeyValuePair<AlarmFilterMeta, GetAlarmFilterResponse> AlarmFilter { get; private set; }

		public bool IsChecked { get; private set; }

		public override string ToString()
		{
			return AlarmFilter.Key.IsShared ?
					$"AlarmFilter: {AlarmFilter.Key.Name} (shared filter), IsChecked: {IsChecked}" :
					$"AlarmFilter: {AlarmFilter.Key.Name}, IsChecked: {IsChecked}";
		}
	}
}