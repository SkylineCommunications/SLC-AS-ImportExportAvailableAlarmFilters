namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Components
{
	using System;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Helpers;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class AlarmSelectionSection : Section
	{
		private readonly Label filterName;
		private readonly AlarmFilterMeta alarmFilter;

		public AlarmSelectionSection(AlarmFilterMeta alarmFilter, int row, int col)
		{
			if (string.IsNullOrWhiteSpace(alarmFilter.Name))
			{
				throw new ArgumentNullException(nameof(alarmFilter));
			}

			this.alarmFilter = alarmFilter;
			filterName = alarmFilter.IsShared ?
							new Label($"{alarmFilter.Name} (shared filter)") :
							new Label($"{alarmFilter.Name}");

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