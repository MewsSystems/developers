using Infrastructure.Extensions;

namespace Infrastructure.Test.Extensions
{
    public class DateTimeExtensionsTest
    {

        [Fact]
        public void DateTimeExtensions_ToPragueDateTime_ConvertFromEasternStandardTime()
        {

            var inputTime = DateTime.SpecifyKind(new DateTime(2022, 10, 17, 8, 35, 0), DateTimeKind.Unspecified);
            var to = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var fromTimeOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            var offset = new DateTimeOffset(inputTime, fromTimeOffset);
            var destination = TimeZoneInfo.ConvertTime(offset, to); 
            
            DateTime action = destination.ToPragueDateTime();

            Assert.True(action.ToString("yyyy/MM/dd HH:mm:ss") == inputTime.ToString("yyyy/MM/dd HH:mm:ss"));

        }

    }
}
