﻿using System.Collections.Generic;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Core.Configuration;

namespace Omikron.FactFinderTests.Core
{
    [TestClass]
    public class ConfigurationTest : BaseTest
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(ConfigurationTest));
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void TestConnectionSection()
        {
            var section = ConnectionSection.GetSection();

            Assert.IsNotNull(section);
            
            Assert.AreEqual(ConnectionProtocol.Http, section.Protocol);
            Assert.AreEqual(@"demoshop.fact-finder.de", section.ServerAddress);
            Assert.AreEqual(80, section.Port);
            Assert.AreEqual(@"FACT-Finder", section.Context);
            Assert.AreEqual(@"de", section.Channel);
            Assert.AreEqual(@"de", section.Language);

            Assert.AreEqual(@"user", section.Authentication.UserName);
            Assert.AreEqual(@"userpw", section.Authentication.Password);
            Assert.AreEqual(AuthenticationType.Advanced, section.Authentication.Type);
            Assert.AreEqual(@"FACT-FINDER", section.Authentication.Prefix);
            Assert.AreEqual(@"FACT-FINDER", section.Authentication.Postfix);
        }

        [TestMethod]
        public void TestParametersSection()
        {
            var section = ParametersSection.GetSection();

            Assert.IsNotNull(section);

            #region Test server settings

            var expectedServerIgnore = new List<string>()
            {
                "sid",
                "password",
                "username",
                "timestamp",
            };

            Assert.AreEqual(expectedServerIgnore.Count, section.Server.IgnoreRules.Count);
            foreach (var rawRule in section.Server.IgnoreRules)
            {
                var rule = rawRule as IgnoreRuleElement;
                Assert.IsTrue(expectedServerIgnore.Contains(rule.Name));
            }

            var expectedServerRequire = new Dictionary<string, string>();

            Assert.AreEqual(expectedServerRequire.Count, section.Server.RequireRules.Count);
            foreach (var rawRule in section.Server.RequireRules)
            {
                var rule = rawRule as RequireRuleElement;
                Assert.IsTrue(expectedServerRequire[rule.Name] == rule.Default);
            }

            var expectedServerMappings = new Dictionary<string, string>()
            {
                {"keywords", "query"},
            };

            Assert.AreEqual(expectedServerMappings.Count, section.Server.MappingRules.Count);
            foreach (var rawRule in section.Server.MappingRules)
            {
                var rule = rawRule as MappingRuleElement;
                Assert.IsTrue(expectedServerMappings[rule.Name] == rule.MapTo);
            }

            #endregion

            #region Test client settings

            var expectedClientIgnore = new List<string>()
            {
                "xml",
                "format",
                "channel",
                "password",
                "username",
                "timestamp",
            };

            Assert.AreEqual(expectedClientIgnore.Count, section.Client.IgnoreRules.Count);
            foreach (var rawRule in section.Client.IgnoreRules)
            {
                var rule = rawRule as IgnoreRuleElement;
                Assert.IsTrue(expectedClientIgnore.Contains(rule.Name));
            }

            var expectedClientRequire = new Dictionary<string, string>()
            {
                {"test", "value"},
            };

            Assert.AreEqual(expectedClientRequire.Count, section.Client.RequireRules.Count);
            foreach (var rawRule in section.Client.RequireRules)
            {
                var rule = rawRule as RequireRuleElement;
                Assert.IsTrue(expectedClientRequire[rule.Name] == rule.Default);
            }

            var expectedClientMappings = new Dictionary<string, string>()
            {
                {"query", "keywords"},
            };

            Assert.AreEqual(expectedClientMappings.Count, section.Client.MappingRules.Count);
            foreach (var rawRule in section.Client.MappingRules)
            {
                var rule = rawRule as MappingRuleElement;
                Assert.IsTrue(expectedClientMappings[rule.Name] == rule.MapTo);
            }

            #endregion
        }

        [TestMethod]
        public void TestFieldsSection()
        {
            FieldsSection section = FieldsSection.getInstance();
            Assert.IsNotNull(section);
            Assert.AreEqual(@"uid", section.recordId);
            Assert.AreEqual(@"ArticleNumber", section.productNumber);
            Assert.AreEqual(@"MasterProductNumber", section.masterProductNumber);
            Assert.AreEqual(@"EAN", section.ean);
            Assert.AreEqual(@"Name", section.productName);
            Assert.AreEqual(@"Brand", section.brand);
            Assert.AreEqual(@"Description", section.description);
            Assert.AreEqual(@"Price", section.price);
            Assert.AreEqual(@"ImageURL", section.imageUrl);
            Assert.AreEqual(@"ProductURL", section.deeplink);
        }

        [TestMethod]
        public void TestModulesSection()
        {
            ModulesSection section = ModulesSection.getInstance();
            Assert.IsNotNull(section);
            Assert.AreEqual(true, section.useCampaigns);
            Assert.AreEqual(false, section.useRecommendations);
            Assert.AreEqual(true, section.useSimilarRecords);
            Assert.AreEqual(true, section.useSuggest);
            Assert.AreEqual(false, section.useTagcloud);
            Assert.AreEqual(true, section.useTracking);
        }
    }
}
