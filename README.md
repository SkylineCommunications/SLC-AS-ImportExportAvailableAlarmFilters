# Import-Export Available Alarm Filters

This repository contains two DataMiner Automation scripts designed to easily export and import/update alarm filters.

📤 ExportAvailableAlarmFilters — Exports all available user and/or shared alarm filters from a DataMiner system into a structured JSON file.

📥 ImportAvailableAlarmFilters — Imports those filters back into the system (or updates existing ones if the same name already exists).

Both scripts help you quickly migrate, backup, or debug alarm filter configurations between environments.

## ExportAvailableAlarmFilters

🧩 Overview

Exports selected alarm filters and/or shared alarm filters from DataMiner to a JSON file.
The exported file is stored under: **C:\DataMiner\Documents\AlarmFilterExports\\{ExportName}.json**

⚙️ How It Works

The script loads all available alarm filters.

User selects the filters to export.

The script creates a JSON file containing the selected filters.

🧰 Output Example

```json
{
  "ExportName": "SharedFilters",
  "Timestamp": "2025-11-07T18:33:28.3872508Z",
  "AlarmCount": 1,
  "Alarms": [
    {
      "Name": "Critical_Only",
      "Description": "Shows only critical alarms",
      "Key": "sharedusersettings:critical_only",
      "AccessType": "public",
      "CreatedBy": "admin",
      "Version": 2,
      "UserContext": "sharedusersettings",
      "IsShared": true,
      "FilterBase64": "AgART1BFUkFUSU9OU1xzcHJpbm8RT1BFUkFUSU9OU1xzcHJpbm8DMy4wBnB1YmxpYxJzaGFyZWR1c2Vyc2V0dGluZ3Mlc2hhcmVkdXNlcnNldHRpbmdzOmVnbm9zX2FsYXJtc2ZpbHRlchJFR05PU19BbGFybXNGaWx0ZXIHAAAAEBBFbGVtZW50LlByaW9yaXR5AwEAAAAMKkVHTk9TX0dFTzMqIgMBAgMAAAANAAAAHAAAABEAAAAiAwICAgAAABMAAAAZAAAAIgMDAgEAAAAZAAAA"
    }
  ]
}
```

## ImportAvailableAlarmFilters
🧩 Overview

Imports alarm filters from a JSON file previously exported by the ExportAvailableAlarmFilters script.

⚙️ How It Works

User specifies the file path through the input parameter:

**FilePathToImport = C:\DataMiner\Documents\AlarmFilterExports\SharedFilters.json**


The script loads the file and recreates the filters in DataMiner.

Existing filters are updated; missing ones are created automatically.

🧱 Key Details

Automatically distinguishes between Update and New filters.

Ignores invalid entries safely.

Uses the same shared data structure as the export script.

Includes unit tests under ImportAvailableAlarmFiltersTests.
