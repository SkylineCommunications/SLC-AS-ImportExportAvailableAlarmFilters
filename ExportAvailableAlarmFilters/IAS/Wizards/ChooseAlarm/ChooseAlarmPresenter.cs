namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm
{
	using System;
	using System.Text.RegularExpressions;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.Exporter;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Helpers;
	using Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm.Interfaces;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ChooseAlarmPresenter
	{
		private readonly IChooseAlarmView view;
		private readonly ChooseAlarmModel model;

		public ChooseAlarmPresenter(IChooseAlarmView view, ChooseAlarmModel model)
		{
			this.view = view;
			this.model = model;

			InitPresenterEvents();
			LoadFromModel();
		}

		public event EventHandler<EventArgs> Create;

		public event EventHandler<EventArgs> Cancel;

		internal static bool ValidateExportName(string text)
		{
			return Regex.IsMatch(text, @"^[\w\-. ]+$");
		}

		private void LoadFromModel()
		{
			view.SetupLayout(model.AvailableAlarmFilters, OnSelectionChangeEventArgs);
		}

		private void StoreToModel()
		{
			model.ExportName = view.ExportNameTextBox.Text;
		}

		private void OnSelectionChangeEventArgs(object sender, SelectionChangeEventArgs e)
		{
			if (e.IsChecked)
			{
				if (!this.model.AlarmsToExport.Contains(e.AlarmFilter))
				{
					this.model.AlarmsToExport.Add(e.AlarmFilter);
				}
			}
			else
			{
				this.model.AlarmsToExport.Remove(e.AlarmFilter);
			}
		}

		private void CreateExport()
		{
			Exporter.CreateExport(model.Engine, model.ExportName, model.AlarmsToExport);
		}

		private void InitPresenterEvents()
		{
			view.CancelButton.Pressed += OnCancelButtonPressed;
			view.CreateButton.Pressed += OnCreateButtonPressed;

			view.ExportNameTextBox.Changed += OnExportNameTextBoxChanged;
		}

		private void OnCancelButtonPressed(object sender, EventArgs e)
		{
			Cancel?.Invoke(this, EventArgs.Empty);
		}

		private void OnCreateButtonPressed(object sender, EventArgs e)
		{
			StoreToModel();
			CreateExport();

			Create?.Invoke(this, EventArgs.Empty);
		}

		private void OnExportNameTextBoxChanged(object sender, TextBox.TextBoxChangedEventArgs e)
		{
			var textBox = sender as TextBox;
			textBox.ValidationState = ValidateExportName(textBox.Text) ? UIValidationState.Valid : UIValidationState.Invalid;

			if(textBox.ValidationState == UIValidationState.Valid)
			{
				view.CreateButton.IsEnabled = true;
			}
			else
			{
				view.CreateButton.IsEnabled = false;
			}
		}
	}
}