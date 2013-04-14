﻿using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Json.Helper;
using log4net;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonScicAdapter : ScicAdapter
    {
        private static ILog log;

        static JsonScicAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonScicAdapter));
        }

        public JsonScicAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.ShoppingCartInformationCollector;
        }

        public override bool ApplyTracking()
        {
            return Data.Trim() == "true";
        }
    }
}