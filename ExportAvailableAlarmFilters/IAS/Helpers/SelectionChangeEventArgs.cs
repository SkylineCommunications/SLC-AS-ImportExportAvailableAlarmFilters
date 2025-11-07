namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Helpers
{
	using System;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class SelectionChangeEventArgs : EventArgs
	{
		public SelectionChangeEventArgs(AlarmFilterMeta alarmFilter, CheckBox.CheckBoxChangedEventArgs checkBoxEventArgs)
		{
			AlarmFilter = alarmFilter;
			IsChecked = checkBoxEventArgs.IsChecked;
		}

		public AlarmFilterMeta AlarmFilter { get; private set; }

		public bool IsChecked { get; private set; }

		public override string ToString()
		{
			return AlarmFilter.IsShared ?
					$"AlarmFilter: {AlarmFilter.Name} (shared filter), IsChecked: {IsChecked}" :
					$"AlarmFilter: {AlarmFilter.Name}, IsChecked: {IsChecked}";
		}
	}
}