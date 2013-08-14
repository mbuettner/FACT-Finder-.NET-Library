﻿using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF69;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF69
{
    [TestClass]
    public class JsonSuggestAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonSuggestAdapter SuggestAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json69/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            SuggestAdapter = new JsonSuggestAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetSuggestions()
        {
            SuggestAdapter.SetParameter("query", "bmx");
            var suggestions = SuggestAdapter.Suggestions;

            Assert.AreEqual(3, suggestions.Count);
            Assert.AreEqual("Verde BMX", suggestions[0].Query);
            Assert.AreEqual("8blKVw-P5", suggestions[0].RefKey);
            Assert.AreEqual("brand", suggestions[0].Type);
            Assert.AreEqual("category", suggestions[1].Type);
            Assert.AreEqual("productName", suggestions[2].Type);
        }
    }
}