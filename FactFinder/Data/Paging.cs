﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class Paging : List<Item>
    {
        public int CurrentPage { get; private set; }
        public int PageCount { get; private set; }

        public int DisplayPageCount { get; set; }

        private ParametersHandler ParametersHandler;

        public Paging(int currentPage, int pageCount, ParametersHandler parametersHandler)
            : base()
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            ParametersHandler = parametersHandler;
        }

        public string GetPageLink(int pageNumber, string linkTarget = null)
        {
            if (pageNumber > PageCount || pageNumber < 1)
                return "";

            var parameters = new Dictionary<string, string>();
            parameters["page"] = pageNumber.ToString();

            return ParametersHandler.GeneratePageLink(parameters, linkTarget);
        }

        public string GetPreviousPageLink(string linkTarget = null)
        {
            if (CurrentPage == 1)
                return "";

            return GetPageLink(CurrentPage - 1, linkTarget);
        }

        public string GetNextPageLink(string linkTarget = null)
        {
            if (CurrentPage == PageCount)
                return "";

            return GetPageLink(CurrentPage + 1, linkTarget);
        }

        public int GetFirstPageNumberShown()
        {
            if (CurrentPage <= DisplayPageCount / 2 || PageCount < DisplayPageCount)
                return 1;
            else if (CurrentPage > (PageCount - DisplayPageCount + 1))
                return PageCount - DisplayPageCount + 1;
            else
                return CurrentPage - DisplayPageCount / 2;
        }

        public int GetLastPageNumberShown()
        {
            if (PageCount < DisplayPageCount)
                return PageCount;

            int firstPageNumber = GetFirstPageNumberShown();
            if (firstPageNumber + DisplayPageCount >= PageCount)
                return PageCount;
            else
                return firstPageNumber + DisplayPageCount;
        }
    }
}