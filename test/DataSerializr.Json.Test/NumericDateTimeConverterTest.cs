using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace DataSerializr.Json.Test
{
    public class NumericDateTimeConverterTest
    {
        private static JsonSerializerSettings Settings;

        public NumericDateTimeConverterTest()
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            TimeSpan diff = tzi.GetUtcOffset(new DateTime(2018, 7, 1));
            int totalMinutes = -(int)diff.TotalMinutes;
            Settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new NumericDateTimeConverter(totalMinutes) },
                NullValueHandling = NullValueHandling.Include
            };
        }

        [Fact]
        public void Should_Serialize()
        {
            var model = new TestModel { StartDate = new DateTime(2017, 1, 1, 1, 1, 1) };
            string json = JsonConvert.SerializeObject(model, Formatting.None, Settings);

            Assert.Equal("{\"StartDate\":1483225261000}", json);
        }

        [Fact]
        public void Should_Deserialize()
        {
            const string json = "{\"StartDate\":1483225261000}";
            var model = JsonConvert.DeserializeObject<TestModel>(json, Settings);

            Assert.NotNull(model);
            Assert.Equal(new DateTime(2017, 1, 1, 1, 1, 1), model.StartDate);
        }

        public class TestModel
        {
            public DateTime StartDate { get; set; }
        }
    }
}
