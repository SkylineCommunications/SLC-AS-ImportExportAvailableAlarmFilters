namespace Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.Context.Tests
{
	using Moq;

	[TestClass]
	public class AlarmExportProcessorTests
	{
		[TestClass]
		public class ScriptContextTests
		{
			[TestMethod]
			public void LoadExportFromFile_ValidJson_ShouldDeserializeCorrectly()
			{
				// Arrange
				var filePath = GetTestFilePath("valid.json");

				var scriptParamMock = new Mock<ScriptParam>();
				scriptParamMock.Setup(sp => sp.Value).Returns(filePath);

				var engineMock = new Mock<IEngine>();
				engineMock.Setup(e => e.GetScriptParam("FilePathToImport"))
						  .Returns(scriptParamMock.Object);

				var context = new ScriptContext(engineMock.Object);

				// Act
				var export = context.LoadExportFromFile();

				// Assert
				Assert.IsNotNull(export);
				Assert.AreEqual("Export1", export.ExportName);
				Assert.AreEqual(7, export.AlarmCount);
				Assert.AreEqual(7, export.Alarms.Count);
				Assert.AreEqual("Alarm filter", export.Alarms[0].Name);
				Assert.AreEqual("Test SNMP Trap V9", export.Alarms[6].Name);
			}

			[TestMethod]
			public void LoadExportFromFile_EmptyAlarms_ShouldReturnZeroCount()
			{
				// Arrange
				var filePath = GetTestFilePath("empty_alarms.json");

				var scriptParamMock = new Mock<ScriptParam>();
				scriptParamMock.Setup(sp => sp.Value).Returns(filePath);

				var engineMock = new Mock<IEngine>();
				engineMock.Setup(e => e.GetScriptParam("FilePathToImport"))
						  .Returns(scriptParamMock.Object);

				var context = new ScriptContext(engineMock.Object);

				// Act
				var export = context.LoadExportFromFile();

				// Assert
				Assert.IsNotNull(export);
				Assert.AreEqual("Empty_Alarms", export.ExportName);
				Assert.AreEqual(0, export.AlarmCount);
				Assert.IsNotNull(export.Alarms);
				Assert.AreEqual(0, export.Alarms.Count);
			}

			[TestMethod]
			[ExpectedException(typeof(InvalidOperationException))]
			public void LoadExportFromFile_InvalidJson_ShouldThrow()
			{
				// Arrange
				var filePath = GetTestFilePath("invalid.json");

				var scriptParamMock = new Mock<ScriptParam>();
				scriptParamMock.Setup(sp => sp.Value).Returns(filePath);

				var engineMock = new Mock<IEngine>();
				engineMock.Setup(e => e.GetScriptParam("FilePathToImport"))
						  .Returns(scriptParamMock.Object);

				var context = new ScriptContext(engineMock.Object);

				// Act
				context.LoadExportFromFile();
			}

			[TestMethod]
			[ExpectedException(typeof(FileNotFoundException))]
			public void LoadExportFromFile_FileDoesNotExist_ShouldThrow()
			{
				// Arrange
				var filePath = GetTestFilePath("missing.json"); // File does not exist

				var scriptParamMock = new Mock<ScriptParam>();
				scriptParamMock.Setup(sp => sp.Value).Returns(filePath);

				var engineMock = new Mock<IEngine>();
				engineMock.Setup(e => e.GetScriptParam("FilePathToImport"))
						  .Returns(scriptParamMock.Object);

				var context = new ScriptContext(engineMock.Object);

				// Act
				context.LoadExportFromFile();
			}

			private string GetTestFilePath(string fileName)
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsonFiles", fileName);
			}
		}
	}
}