namespace Skyline.DataMiner.Automation.ExportAvailableAlarmFilters.IAS.Wizards.ChooseAlarm.Tests
{
	[TestClass]
	public class ChooseAlarmPresenterTests
	{
		[TestMethod]
		[DataRow("Valid_Name-123", true)]
		[DataRow("Another.Valid Name", true)]
		[DataRow("Invalid/Name", false)]
		[DataRow("Invalid\\Name", false)]
		[DataRow("With*Star", false)]
		[DataRow("", false)]
		[DataRow("Test@1", false)]
		public void ValidateExportName_ShouldWorkAsExpected(string input, bool expected)
		{
			// Act
			bool result = ChooseAlarmPresenter.ValidateExportName(input);

			// Assert
			Assert.AreEqual(expected, result);
		}
	}
}