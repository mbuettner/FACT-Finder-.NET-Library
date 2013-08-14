﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Json.FF69
{
    public class JsonSearchAdapter : Omikron.FactFinder.Json.FF68.JsonSearchAdapter
    {
        public JsonSearchAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected override AsnGroup CreateGroupInstance(dynamic groupData)
        {
            string groupName = (string)groupData.name;
            string groupUnit = (string)groupData.unit;

            return new AsnGroup(
                new List<AsnFilterItem>(),
                groupName,
                (int)groupData.detailedLinks,
                groupUnit,
                GetAsnGroupStyleFromString((string)groupData.filterStyle)
            );
        }

        protected override AsnFilterItem CreateFilter(dynamic asnGroup, dynamic element)
        {
            Uri filterLink = CreateLink(element);

            AsnFilterItem filter;

            if (asnGroup.Style == AsnGroupStyle.Slider)
            {
                NameValueCollection parameters = ParametersHandler.ParseParametersFromString((string)element.searchParams);

                filter = new AsnSliderItem(
                    filterLink,
                    (float)element.absoluteMinValue,
                    (float)element.absoluteMaxValue,
                    (float)element.selectedMinValue,
                    (float)element.selectedMaxValue,
                    (string)element.associatedFieldName
                );
            }
            else
            {
                filter = new AsnFilterItem(
                    (string)element.name,
                    filterLink,
                    (bool)element.selected,
                    (int)element.recordCount,
                    (int)element.clusterLevel,
                    (string)(element.previewImageURL ?? ""),
                    (string)(element.associatedFieldName ?? "")
                );
            }
            return filter;
        }

        protected override Uri CreateLink(dynamic element)
        {
            var additionalParameters = new NameValueCollection();
            additionalParameters["sourceRefKey"] = Result.RefKey;
            return ParametersHandler.GeneratePageLink(
                ParametersHandler.ParseParametersFromString((string)element.searchParams),
                additionalParameters
            );
        }

        protected override ResultRecords CreateResult()
        {
            var result = new List<Record>();
            int resultCount = 0;

            var searchResultData = JsonData.searchResult;

            if (searchResultData.records.Count > 0)
            {
                resultCount = (int)searchResultData.resultCount;

                int positionOffset = (Paging.CurrentPage - 1) * Int32.Parse(ProductsPerPageOptions.SelectedOption.Label);

                int positionCounter = 1;

                foreach (var recordData in searchResultData.records)
                {
                    int position = positionCounter + positionOffset;
                    ++positionCounter;

                    result.Add(GetRecordFromRawData(recordData, position));
                }
            }

            return new ResultRecords(result, resultCount, (string)searchResultData.refKey);
        }

        protected override Record GetRecordFromRawData(dynamic recordData, int position)
        {

            int originalPosition = position;

            Dictionary<string, object> fieldValues = recordData.record.AsDictionary();

            if (fieldValues.ContainsKey("__ORIG_POSITION__"))
            {
                originalPosition = Int32.Parse((string)fieldValues["__ORIG_POSITION__"]);
                fieldValues.Remove("__ORIG_POSITION__");
            }

            var keywords = new List<string>();

            foreach (var keyword in recordData.keywords)
            {
                keywords.Add((string)keyword);
            }

            return new Record(
                recordData.id.ToString(),
                (float)recordData.searchSimilarity,
                position,
                originalPosition,
                fieldValues,
                (string)recordData.seoPath,
                keywords
            );
        }
    }
}