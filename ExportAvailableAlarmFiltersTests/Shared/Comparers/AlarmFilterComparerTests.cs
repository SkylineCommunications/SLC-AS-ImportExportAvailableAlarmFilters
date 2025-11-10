namespace ExportAvailableAlarmFiltersTests.Shared.Comparers
{
	using SharedCode.Comparers;
	using Skyline.DataMiner.Net.Filters;

	[TestClass]
	public class AlarmFilterComparerTests
	{
		private AlarmFilterComparer comparer = null!; // Initialize with null-forgiving operator

		[TestInitialize]
		public void Setup()
		{
			comparer = new AlarmFilterComparer();
		}

		[TestMethod]
		public void Equals_ShouldReturnTrue_ForSameNameAndSharedStatus()
		{
			var alarmA1 = new AlarmFilterMeta { Name = "FilterA", Owner = "SharedUserSettings" }; // IsShared = true
			var alarmA2 = new AlarmFilterMeta { Name = "FilterA", Owner = "SharedUserSettings" };

			Assert.IsTrue(comparer.Equals(alarmA1, alarmA2));
		}

		[TestMethod]
		public void Equals_ShouldReturnFalse_ForSameNameButDifferentSharedStatus()
		{
			var alarmA1 = new AlarmFilterMeta { Name = "FilterA", Owner = "SharedUserSettings" }; // IsShared = true
			var alarmA2 = new AlarmFilterMeta { Name = "FilterA" };

			Assert.IsFalse(comparer.Equals(alarmA1, alarmA2));
		}

		[TestMethod]
		public void Equals_ShouldReturnFalse_ForDifferentNames()
		{
			var alarmA = new AlarmFilterMeta { Name = "FilterA" };
			var alarmB = new AlarmFilterMeta { Name = "FilterB" };

			Assert.IsFalse(comparer.Equals(alarmA, alarmB));
		}

		[TestMethod]
		public void GetHashCode_ShouldBeSame_ForEqualObjects()
		{
			var alarmA1 = new AlarmFilterMeta { Name = "FilterA", Owner = "SharedUserSettings" };
			var alarmA2 = new AlarmFilterMeta { Name = "FilterA", Owner = "SharedUserSettings" };

			Assert.AreEqual(comparer.GetHashCode(alarmA1), comparer.GetHashCode(alarmA2));
		}
	}
}