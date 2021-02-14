using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestFunction;

namespace TestTests
{
    [TestClass]
    public class EventHubListenerTest
    {
        private int te;
        private Mock<ILogger> logger;

        [TestInitialize]
        public void Setup()
        {
            logger = new Mock<ILogger>();
        }

        [TestMethod]
        public void TestMethodTest()
        {
            var testClass = new EventHubListener();
            var jsonData = new
            {
                Name = "Olawale Adewoyin",
                Address = "Somewhere",
                Age = 56
            };

            var testData = JsonConvert.SerializeObject(jsonData);
            var testArray = Encoding.ASCII.GetBytes(testData);

            var events = new EventData[1];
            events[0] = new EventData(testArray);

            var result = testClass.EventHubListenerTest(events, out var message, logger.Object);
        }
    }
}
