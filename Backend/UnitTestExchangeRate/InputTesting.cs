using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestExchangeRate
{
    [TestClass]
    public class InputTesting
    {
        /// <summary>
        /// No input means no action, null expected.
        /// </summary>
        [TestMethod]
        public void NoInput()
        {
            var emptyImput = string.Empty;
            object expectedOutput = null;

            var cachedInstance = ExchangeRateCacher.Instance;
            var actualOutput = cachedInstance.GetExchangeRate(emptyImput);
            cachedInstance.Dispose();

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        /// <summary>
        /// Lizard is not a currencty, null expected
        /// </summary>
        [TestMethod]
        public void NonsenseInput1()
        {
            var nonsence = "Lizard";
            object expectedOutput = null;

            var cachedInstance = ExchangeRateCacher.Instance;
            var actualOutput = cachedInstance.GetExchangeRate(nonsence);
            cachedInstance.Dispose();

            Assert.AreEqual(actualOutput, expectedOutput);
        }


        /// <summary>
        /// Nonsense currenct. Null expected
        /// </summary>
        [TestMethod]
        public void NonsenseInput2()
        {
            var nonsense = "XYZ";
            object expectedOutput = null;

            var cachedInstance = ExchangeRateCacher.Instance;
            var actualOutput = cachedInstance.GetExchangeRate(nonsense);
            cachedInstance.Dispose();

            Assert.AreEqual(actualOutput, expectedOutput);
        }

        /// <summary>
        /// Not returning self. Null expected 
        /// </summary>
        [TestMethod]
        public void SelfTest()
        {
            var currency = "CZK";
            object expectedOutput = null;

            var cachedInstance = ExchangeRateCacher.Instance;
            var actualOutput = cachedInstance.GetExchangeRate(currency);
            cachedInstance.Dispose();

            Assert.AreEqual(actualOutput, expectedOutput);
        }

        /// <summary>
        /// We should get some real results
        /// </summary>
        [TestMethod]
        public void RealInputTest()
        {
            var currency = "USD";
            object invalidOutput = null;

            var cachedInstance = ExchangeRateCacher.Instance;
            var actualOutput = cachedInstance.GetExchangeRate(currency);
            cachedInstance.Dispose();

            Assert.AreNotEqual(actualOutput, invalidOutput);
            Assert.IsTrue(actualOutput.Value > 0);
            Assert.AreEqual(actualOutput.SourceCurrency.ToString(), "CZK") ;
            Assert.AreEqual(actualOutput.TargetCurrency.ToString(), currency);
        }


        /// <summary>
        /// At one point of time, the values should be cached and further returns should be quick
        /// </summary>
        [TestMethod]
        public void MultiThreadCacheTest()
        {
            List<Task<Stopwatch>> taskList = new List<Task<Stopwatch>>();

            for(int launchIndex =0; launchIndex < 25; launchIndex++)
            {
                var task = Task.Run(delegate
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    var rate = ExchangeRateCacher.Instance.GetExchangeRate("USD");
                    timer.Stop();

                    return timer;
                });

                Thread.Sleep(100);
                taskList.Add(task);
            }

            Task.WaitAll(taskList.ToArray());

            long previousTime =0;
            long mark = 0;


            for(var indexer =0; indexer > taskList.Count; indexer++)
            {
                var task = taskList[indexer];
                var time = task.Result.ElapsedMilliseconds;

                if (mark == 0)
                {
                    if(time < 0.3 * previousTime)
                    {
                        mark = previousTime;
                    }
                    previousTime = time;
                }
                else
                {
                    Assert.IsTrue(taskList[indexer].Result.ElapsedMilliseconds < mark);
                }
            }               
        }
    }
}
