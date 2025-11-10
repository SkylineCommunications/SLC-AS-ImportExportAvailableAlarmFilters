namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Components
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Helpers;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class AlarmSelectionSection : Section
	{
		private readonly Label filterName;
		private readonly KeyValuePair<AlarmFilterMeta, GetAlarmFilterResponse> alarmFilter;

		public AlarmSelectionSection(KeyValuePair<AlarmFilterMeta, GetAlarmFilterResponse> alarmFilter, int row, int col)
		{
			if (string.IsNullOrWhiteSpace(alarmFilter.Key.Name))
			{
				throw new ArgumentNullException(nameof(alarmFilter));
			}

			this.alarmFilter = alarmFilter;
			filterName = alarmFilter.Key.IsShared ?
							new Label($"{alarmFilter.Key.Name} (shared filter)") :
							new Label($"{alarmFilter.Key.Name}");

			Layout = new SectionLayout(row, col);

			InitializeWidgets();
			CreateLayout();
		}

		/// <summary>
		/// Event that is triggered when the selection checkbox is toggled.
		/// </summary>
		public event EventHandler<SelectionChangeEventArgs> OnToggleSelection;

		public SectionLayout Layout { get; }

		/// <summary>
		/// Gets the checkbox used to confirm if the export should include the alarm filter that this section represents.
		/// </summary>
		public CheckBox SelectionCheckBox { get; private set; }

		private void InitializeWidgets()
		{
			SelectionCheckBox = new CheckBox("Include") { IsChecked = true };
			SelectionCheckBox.Changed += ObjectSelectionSection_OnToggleSelection;
		}

		private void ObjectSelectionSection_OnToggleSelection(object sender, CheckBox.CheckBoxChangedEventArgs e)
		{
			var eventArgs = new SelectionChangeEventArgs(alarmFilter, e);
			OnToggleSelection?.Invoke(this, eventArgs);
		}

		private void CreateLayout()
		{
			AddWidget(SelectionCheckBox, new WidgetLayout(0, 0));
			AddWidget(filterName, new WidgetLayout(0, 1, HorizontalAlignment.Left));
		}
	}
}