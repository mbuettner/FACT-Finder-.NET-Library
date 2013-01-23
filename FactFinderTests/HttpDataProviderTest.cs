﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using System.Net;

namespace Omikron.FactFinderTests
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class HttpDataProviderTest
    {
        private static XmlConfiguration Configuration { get; set; }
        private UnixClock Clock { get; set; }
        private HttpDataProvider DataProvider { get; set; }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Configuration = new XmlConfiguration(@"Resources\configuration.xml");
            Clock = new UnixClock();
            DataProvider = new HttpDataProvider(Configuration);

            //WebRequest.RegisterPrefix("test", new TestWebRequestCreate());
            //TestWebRequestCreate.CreateTestRequest("lol?");
        }

        [TestMethod()]
        public void TestGetData()
        {
            DataProvider.Type = RequestType.TagCloud;
            DataProvider.SetParameter("do", "getTagCloud");
            DataProvider.SetParameter("format", "json");
            string test = DataProvider.Data;

            Assert.AreEqual(HttpStatusCode.OK, DataProvider.LastStatusCode);
        }
    }
}