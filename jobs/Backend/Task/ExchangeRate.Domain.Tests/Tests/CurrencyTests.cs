using System.ComponentModel.DataAnnotations;
using ExchangeRate.Models;
using ExchangeRate.Models.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRate.Domain.Tests.Tests;

[TestClass]
public class CurrencyTests
{
    [TestMethod]
    [DataRow("A")]
    [DataRow("AA")]
    [DataRow("AAAA")]
    [DataRow("AAAAA")]
    [DataRow("1")]
    [DataRow("2")]
    [DataRow("123")]
    [DataRow("AA1")]
    [DataRow("AA*")]
    [DataRow("%%%")]
    [ExpectedException(typeof(ValidationException), "Currency failed validation test")]
    public void CurrencyUtils_Invalid_Input(string testCase)
    {
        CurrencyUtils.Validate(testCase);
    }

    [TestMethod]
    [DataRow("CZK")]
    [DataRow("USD")]
    [DataRow("XYZ")]
    [DataRow("JPY")]
    public void CurrencyUtils_Valid_Input(string testCase)
    {
        CurrencyUtils.Validate(testCase);
    }

    [TestMethod]
    [DataRow("A")]
    [DataRow("AA")]
    [DataRow("AAAA")]
    [DataRow("AAAAA")]
    [DataRow("1")]
    [DataRow("2")]
    [DataRow("123")]
    [DataRow("AA1")]
    [DataRow("AA*")]
    [DataRow("%%%")]
    [ExpectedException(typeof(ValidationException), "Currency failed validation test")]
    public void Currency_Invalid_Input(string testCase)
    {
        new Currency(testCase);
    }

    [TestMethod]
    [DataRow("CZK")]
    [DataRow("USD")]
    [DataRow("XYZ")]
    [DataRow("JPY")]
    public void Currency_Valid_Input(string testCase)
    {
        new Currency(testCase);
    }
}