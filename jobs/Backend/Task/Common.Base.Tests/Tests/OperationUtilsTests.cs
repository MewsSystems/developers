using Common.Base.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Base.Tests.Tests;

[TestClass]
public class OperationUtilsTests
{
    [TestMethod]
    [DataRow(-1)]
    [DataRow(1, 2, 3, 4, -1)]
    [DataRow(-1, -1, -1, -1, -1)]
    public void EqualAll_Ok(params int[] values)
    {
        // Act
        var actualResult = OperationUtils.EqualsAny(-1, values);

        // Assert
        Assert.AreEqual(true, actualResult);
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(1, 2, 3, 4, 5)]
    [DataRow]
    public void EqualAll_Not_Ok(params int[] values)
    {
        // Act
        var actualResult = OperationUtils.EqualsAny(-1, values);

        // Assert
        Assert.AreEqual(false, actualResult);
    }
}