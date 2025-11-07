namespace Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm
{
	using SharedCode.Helpers.Exceptions;
	using Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm.Interfaces;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ChooseAlarmWizard
	{
		private readonly ChooseAlarmModel model;
		private readonly IChooseAlarmView view;
		private readonly ChooseAlarmPresenter presenter;

		private IEngine engine;

		public ChooseAlarmWizard(IEngine engine)
		{
			this.engine = engine;

			model = new ChooseAlarmModel(engine);
			view = new ChooseAlarmView(engine);
			presenter = new ChooseAlarmPresenter(view, model);

			InitializeWizardEvents();
		}

		public Dialog Dialog => view.Dialog;

		private void InitializeWizardEvents()
		{
			presenter.Cancel += (sender, args) =>
			{
				engine.ExitSuccess("Script closed by user.");
			};

			presenter.Create += (sender, args) =>
			{
				throw new CloseUserInteractionException();
			};
		}
	}
}