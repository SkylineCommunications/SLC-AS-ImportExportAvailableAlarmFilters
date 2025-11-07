namespace Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.IAS.Helpers;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public interface IChooseAlarmView
	{
		Dialog Dialog { get; }

		Label ExportNameLabel { get; }

		TextBox ExportNameTextBox { get; }

		Button CreateButton { get; }

		Button CancelButton { get; }

		void SetupLayout(ICollection<AlarmFilterMeta> alarmFilters, EventHandler<SelectionChangeEventArgs> selectionChangeEventHandler);
	}
}