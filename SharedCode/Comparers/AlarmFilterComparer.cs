namespace SharedCode.Comparers
{
	using System.Collections.Generic;
	using Skyline.DataMiner.Net.Filters;

	public class AlarmFilterComparer : IEqualityComparer<AlarmFilterMeta>
	{
		public bool Equals(AlarmFilterMeta x, AlarmFilterMeta y)
		{
			return x.Name.Equals(y.Name) && x.IsShared == y.IsShared;
		}

		public int GetHashCode(AlarmFilterMeta obj)
		{
			unchecked
			{
				int hash = 17;
				hash = (hash * 23) + (obj.Name?.ToLowerInvariant()?.GetHashCode() ?? 0);
				hash = (hash * 23) + obj.IsShared.GetHashCode();
				return hash;
			}
		}
	}
}