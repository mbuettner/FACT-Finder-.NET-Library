﻿using System;
using System.Collections.Generic;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Json.FF68
{
    public class JsonSuggestAdapter : Omikron.FactFinder.Json.FF67.JsonSuggestAdapter
    {
        public JsonSuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        // TODO: Utilize all new Suggest features
        protected override IList<SuggestQuery> CreateSuggestions()
        {
            var suggestions = new List<SuggestQuery>();

            foreach (var suggestData in JsonData)
            {
                string query = (string)suggestData.name;
                var parameters = new Dictionary<string, string>() { { "query", query } };
                suggestions.Add(new SuggestQuery(
                    query,
                    ParametersHandler.GeneratePageLink(
                        ParametersHandler.ParseParametersFromString((string)suggestData.searchParams)
                    ),
                    (int)suggestData.hitCount,
                    (string)suggestData.type,
                    (string)suggestData.image
                ));
            }

            return suggestions;
        }
    }
}
