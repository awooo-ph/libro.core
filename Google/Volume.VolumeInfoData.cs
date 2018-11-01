using System.Collections.Generic;
using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class Volume 
    {
        public partial class VolumeInfoData
        {
            [JsonProperty("allowAnonLogging")]
            public virtual bool? AllowAnonLogging { get; set; }

            [JsonProperty("authors")]
            public virtual IList<string> Authors { get; set; }

            [JsonProperty("averageRating")]
            public virtual double? AverageRating { get; set; }

            [JsonProperty("canonicalVolumeLink")]
            public virtual string CanonicalVolumeLink { get; set; }

            [JsonProperty("categories")]
            public virtual IList<string> Categories { get; set; }

            [JsonProperty("contentVersion")]
            public virtual string ContentVersion { get; set; }

            [JsonProperty("description")]
            public virtual string Description { get; set; }

            [JsonProperty("dimensions")]
            public virtual DimensionsData Dimensions { get; set; }

            [JsonProperty("imageLinks")]
            public virtual ImageLinksData ImageLinks { get; set; }

            [JsonProperty("industryIdentifiers")]
            public virtual IList<IndustryIdentifiersData> IndustryIdentifiers { get; set; }

            [JsonProperty("infoLink")]
            public virtual string InfoLink { get; set; }

            [JsonProperty("language")]
            public virtual string Language { get; set; }

            [JsonProperty("mainCategory")]
            public virtual string MainCategory { get; set; }

            [JsonProperty("maturityRating")]
            public virtual string MaturityRating { get; set; }

            [JsonProperty("pageCount")]
            public virtual int? PageCount { get; set; }

            [JsonProperty("panelizationSummary")]
            public virtual PanelizationSummaryData PanelizationSummary { get; set; }

            [JsonProperty("previewLink")]
            public virtual string PreviewLink { get; set; }

            [JsonProperty("printType")]
            public virtual string PrintType { get; set; }

            [JsonProperty("printedPageCount")]
            public virtual int? PrintedPageCount { get; set; }

            [JsonProperty("publishedDate")]
            public virtual string PublishedDate { get; set; }

            [JsonProperty("publisher")]
            public virtual string Publisher { get; set; }

            [JsonProperty("ratingsCount")]
            public virtual int? RatingsCount { get; set; }

            [JsonProperty("readingModes")]
            public virtual object ReadingModes { get; set; }

            [JsonProperty("samplePageCount")]
            public virtual int? SamplePageCount { get; set; }

            [JsonProperty("seriesInfo")]
            public virtual VolumeSeriesInfo SeriesInfo { get; set; }

            [JsonProperty("subtitle")]
            public virtual string Subtitle { get; set; }

            [JsonProperty("title")]
            public virtual string Title { get; set; }
        }
    }

}
