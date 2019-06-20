DataSerializr.Json
==================

Custom JsonConverters for Newtonsoft's Json.Net to serialize and deserialize various types.

[![NuGet](https://img.shields.io/nuget/v/DataSerializr.Json.svg)](https://nuget.org/packages/DataSerializr.Json)

# Motivation
## DataTableJsonConverter
The default Json.Net DataTableConverter produces large JSON string for large DataTables. In order to trim down the JSON string size, the DataTableJsonConverter has been implemented to avoid repeating the column names for every row. If the DataTable size is small, the default Json.Net DataTableConverter will still produce a smaller JSON string.

### Examples
```C#
var table = new DataTable("Table1");
table.Columns.Add("id", typeof(int));
table.Columns.Add("first_name", typeof(string));
table.Columns.Add("last_name", typeof(string));
table.Columns.Add("address", typeof(string));

table.Rows.Add(1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
table.Rows.Add(2, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
table.Rows.Add(3, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

var settings = new JsonSerializerSettings
{
    Converters = new List<JsonConverter> { new DataTableJsonConverter() }
};

string json = JsonConvert.SerializeObject(table, Formatting.Indented, settings);
```
The code snippet above will generate the following JSON outputs.

#### Default JSON.Net DataTableConveter Output
```JSON
[
  {
    "id": 1,
    "first_name": "07e84c8f-65bc-4b61-abc7-f199a9d468c8",
    "last_name": "ed724d44-2438-47f6-ae39-02e95b2dce0d",
    "address": "3960ef8a-0378-46b5-be9d-fa2a197ecc0b"
  },
  {
    "id": 2,
    "first_name": "ae57e74a-3f58-454e-9835-41a31012f256",
    "last_name": "6ebec54c-9745-41f2-bac0-a4689df070ec",
    "address": "e11183b3-7f3c-495c-b713-c37f5748265b"
  },
  {
    "id": 3,
    "first_name": "80ec97f5-fc06-417e-bd09-2737d228d4b2",
    "last_name": "42af53f6-bb5a-4b1c-a9b7-fc8c9a02a0cf",
    "address": "01898b60-3918-4cde-b659-13b4cefbb0f8"
  }
]
```

#### DataSerializer.Json Output
```JSON
{
  "name": "Table1",
  "columns": [
    {
      "name": "id",
      "type": "number"
    },
    {
      "name": "first_name",
      "type": "string"
    },
    {
      "name": "last_name",
      "type": "string"
    },
    {
      "name": "address",
      "type": "string"
    }
  ],
  "rows": [
    [
      1,
      "07e84c8f-65bc-4b61-abc7-f199a9d468c8",
      "ed724d44-2438-47f6-ae39-02e95b2dce0d",
      "3960ef8a-0378-46b5-be9d-fa2a197ecc0b"
    ],
    [
      2,
      "ae57e74a-3f58-454e-9835-41a31012f256",
      "6ebec54c-9745-41f2-bac0-a4689df070ec",
      "e11183b3-7f3c-495c-b713-c37f5748265b"
    ],
    [
      3,
      "80ec97f5-fc06-417e-bd09-2737d228d4b2",
      "42af53f6-bb5a-4b1c-a9b7-fc8c9a02a0cf",
      "01898b60-3918-4cde-b659-13b4cefbb0f8"
    ]
  ]
}
```