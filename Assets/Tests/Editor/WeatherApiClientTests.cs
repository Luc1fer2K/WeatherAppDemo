using NUnit.Framework;
using UnityEngine;
using WeatherApp;

public class WeatherApiClientTests
{
    [Test]
    public void BuildUrl_ContainsRequiredQueryParams()
    {
        var client = new WeatherApiClient();
        var url = client.BuildUrl(19.07f, 72.87f);

        Assert.That(url, Does.Contain("latitude=19.07"));
        Assert.That(url, Does.Contain("longitude=72.87"));
        Assert.That(url, Does.Contain("daily=temperature_2m_max"));
        Assert.That(url, Does.Contain("timezone=Asia%2FCalcutta"));
    }

    [Test]
    public void JsonUtility_CanParseSampleResponse_FirstTempMatches()
    {
        const string sampleJson =
@"{
  ""latitude"": 19.125,
  ""longitude"": 72.875,
  ""daily_units"": { ""time"": ""iso8601"", ""temperature_2m_max"": ""°C"" },
  ""daily"": {
    ""time"": [""2022-11-29"", ""2022-11-30""],
    ""temperature_2m_max"": [32, 34.5]
  }
}";

        var data = JsonUtility.FromJson<WeatherApiResponse>(sampleJson);

        Assert.IsNotNull(data);
        Assert.IsNotNull(data.daily);
        Assert.IsNotNull(data.daily.temperature_2m_max);
        Assert.AreEqual(32f, data.daily.temperature_2m_max[0]);
    }

    [Test]
    public void Formatter_ReturnsReadableMessage()
    {
        var msg = WeatherToastFormatter.Format(33.5f, 19.07f, 72.87f);
        Assert.That(msg, Does.Contain("33.5"));
        Assert.That(msg, Does.Contain("°C"));
        Assert.That(msg, Does.Contain("19.07"));
        Assert.That(msg, Does.Contain("72.87"));
    }
}
