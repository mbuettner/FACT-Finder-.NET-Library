﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class AsnFilterItem : Item
    {
        public int MatchCount { get; private set; }
        public int ClusterLevel { get; private set; }
        public string PreviewImage { get; private set; }
        public string Field { get; private set; }

        public AsnFilterItem(
            string value,
            string url,
            bool selected = false,
            int matchCount = 0,
            int clusterLevel = 0,
            string previewImage = "",
            string field = ""
        )
            : base(value, url, selected)
        {
            MatchCount = matchCount;
            ClusterLevel = clusterLevel;
            PreviewImage = previewImage;
            Field = field;
        }

        public bool HasPreviewImage()
        {
            return PreviewImage.Length > 0;
        }
    }
}
