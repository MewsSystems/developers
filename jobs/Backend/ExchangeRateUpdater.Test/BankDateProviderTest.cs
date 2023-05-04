using NUnit.Framework;

namespace ExchangeRateUpdater.Test
{
    public class BankDateProviderTest
    {
        [Test]
        public void TestGetDailyListingBankDateForDateTimeOnWeekendDay() {
            var weekendDateTime = new DateTime(2023, 4, 30);
            var testObj = new BankDateProvider();

            var result = testObj.GetDailyListingBankDateForDateTime(weekendDateTime);

            Assert.AreEqual(new DateOnly(2023, 4, 28), result);
        }

        [Test]
        public void TestGetDailyListingBankDateForDateTimeOnNationalHolidayDay() {
            var nationalHolidayDateTime = new DateTime(2023, 5, 1);
            var testObj = new BankDateProvider();

            var result = testObj.GetDailyListingBankDateForDateTime(nationalHolidayDateTime);

            Assert.AreEqual(new DateOnly(2023, 4, 28), result);
        }

        [Test]
        public void TestGetDailyListingBankDateForDateTimeOnWorkingDayBeforeNewListingTime() {
            var workingDayTimeDate = new DateTime(2023, 5, 1, 14, 00, 1);
            var testObj = new BankDateProvider();

            var result = testObj.GetDailyListingBankDateForDateTime(workingDayTimeDate);

            Assert.AreEqual(new DateOnly(2023, 4, 28), result);
        }

        [Test]
        public void TestGetDailyListingBankDateForDateTimeOnWorkingDayAfterNewListingTime() {
            var workingDayTimeDate = new DateTime(2023, 5, 2, 14, 30, 1);
            var testObj = new BankDateProvider();

            var result = testObj.GetDailyListingBankDateForDateTime(workingDayTimeDate);

            Assert.AreEqual(new DateOnly(2023, 5, 2), result);
        }

        [Test]
        public void TestGetMonthlyListingBankDateForPreviousMonthEndingOnWeekend() {
            var dateTime = new DateTime(2023, 5, 2);
            var testObj = new BankDateProvider();

            var result = testObj.GetMonthlyListingBankDateForDateTime(dateTime);

            Assert.AreEqual(new DateOnly(2023, 4, 28), result);
        }

        [Test]
        public void TestGetMonthlyListingBankDateForPreviousMonthEndingOnWorkday() {
            var dateTime = new DateTime(2023, 4, 1);
            var testObj = new BankDateProvider();

            var result = testObj.GetMonthlyListingBankDateForDateTime(dateTime);

            Assert.AreEqual(new DateOnly(2023, 3, 31), result);
        }
    }
}
