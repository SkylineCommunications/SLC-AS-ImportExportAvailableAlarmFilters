namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Components;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Helpers;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm.Interfaces;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ChooseAlarmView : Dialog, IChooseAlarmView
	{
		public ChooseAlarmView(IEngine engine) : base(engine)
		{
			Title = "Alarm Filter Export";
			MinWidth = 500;
			MinHeight = 200;

			InitializeLabels();
			InitializeTextBox();
			InitializeButtons();
		}

		public Dialog Dialog => this;

		public Label ExportNameLabel { get; private set; }

		public TextBox ExportNameTextBox { get; private set; }

		public Button CreateButton { get; private set; }

		public Button CancelButton { get; private set; }

		public void SetupLayout(ICollection<AlarmFilterMeta> alarmFilters, EventHandler<SelectionChangeEventArgs> selectionChangeEventHandler)
		{
			AddWidget(ExportNameLabel, 0, 0);
			AddWidget(ExportNameTextBox, 1, 0);
			AddWidget(new WhiteSpace(), 2, 0);

			int offset = 3;
			foreach (var alarmFilter in alarmFilters)
			{
				var alarmSelection = new AlarmSelectionSection(alarmFilter, offset, 0);
				AddSection(alarmSelection, alarmSelection.Layout);

				alarmSelection.OnToggleSelection += selectionChangeEventHandler;
				offset++;
			}

			AddWidget(new WhiteSpace(), RowCount, 0);

			var lastRow = RowCount;
			AddWidget(CancelButton, lastRow, 0, HorizontalAlignment.Left);
			AddWidget(CreateButton, lastRow, 1, HorizontalAlignment.Right);
		}

		private void InitializeLabels()
		{
			ExportNameLabel = new Label("Export Name:");
		}

		private void InitializeTextBox()
		{
			ExportNameTextBox = new TextBox { Width = 300, Height = 30, ValidationState = UIValidationState.Invalid, ValidationText = "Please choose a valid export name." };
		}

		private void InitializeButtons()
		{
			CreateButton = new Button("Create") { MinWidth = 200, Height = 30, IsEnabled = false };
			CancelButton = new Button("Cancel") { MinWidth = 200, Height = 30 };
		}
	}
}