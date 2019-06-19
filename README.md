DataSerializr.Json
==================

A JsonConverter for Json.Net to serialize and deserialize DataTable and DataView into a smaller Json.

[![NuGet](https://img.shields.io/nuget/v/DataSerializr.Json.svg)](https://nuget.org/packages/DataSerializr.Json)

## Example
```C#
var settings = new JsonSerializerSettings
{
    Converters = new List<JsonConverter> {  new DataTableJsonConverter() }
};
string json = JsonConvert.SerializeObject(datatable, Formatting.None, settings);
```

### Sample Output
```JSON
{
    "name":"Table1",
    "columns":[{"name":"id","type":"number"},{"name":"fullname","type":"string"}],
    "keys":["id"],
    "rows":[[1,"John Weak"],[2,"El Chupacabra"]]
}
```