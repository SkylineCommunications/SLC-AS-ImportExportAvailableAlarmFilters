/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

10-11-2025	1.0.0.1		JSL, Skyline	Initial version
****************************************************************************
*/

namespace ImportAvailableAlarmFilters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using SharedCode.SLNet.Alarm;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.Context;
	using Skyline.DataMiner.Automation.ImportAvailableAlarmFilters.JsonClass;
	using Skyline.DataMiner.Net.Filters;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				var context = new ScriptContext(engine);
				RunSafe(context);
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private static HashSet<string> GetAlarmFilterNames(ScriptContext context)
		{
			var response = AlarmFilters.GetAvailableAlarmFilters(context.Engine);
			if (response == null)
			{
				return new HashSet<string>();
			}

			var filters = response.Filters ?? Array.Empty<AlarmFilterMeta>();
			var sharedFilters = response.SharedFilters ?? Array.Empty<AlarmFilterMeta>();

			return filters.Concat(sharedFilters)
						  .Select(f => f.Name)
						  .Where(name => !string.IsNullOrEmpty(name))
						  .ToHashSet(StringComparer.OrdinalIgnoreCase);
		}

		private static AlarmFilter GenerateAlarmFilter(AlarmFilterInfo alarm)
		{
			return new AlarmFilter
			{
				Name = alarm.Name,
				Description = alarm.Description,
				Key = alarm.Key,
				AccessType = alarm.AccessType,
				CreatedBy = alarm.CreatedBy,
				Version = alarm.Version.ToString(),
			};
		}

		private void RunSafe(ScriptContext context)
		{
			var alarmExport = context.LoadExportFromFile();
			if (alarmExport.AlarmCount == 0)
			{
				context.Engine.ExitSuccess("No alarms found in the import file.");
				return;
			}

			ImportAlarms(context, alarmExport);
		}

		private void ImportAlarms(ScriptContext context, AlarmExport alarmExport)
		{
			var currentAlarmFilterNames = GetAlarmFilterNames(context);

			foreach (var alarm in alarmExport.Alarms)
			{
				var alarmFilter = GenerateAlarmFilter(alarm);

				var updateType = currentAlarmFilterNames.Contains(alarmFilter.Name) ?
									Skyline.DataMiner.Net.Messages.UpdateAlarmFilterMessage.UpdateType.Update :
									Skyline.DataMiner.Net.Messages.UpdateAlarmFilterMessage.UpdateType.New;

				AlarmFilters.UpdateAlarmFilter(
					context.Engine,
					alarmFilter,
					null,
					null,
					updateType);
			}
		}
	}
}