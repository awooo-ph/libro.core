using System.Collections.Generic;
using Newtonsoft.Json;

namespace Libro.Google
{

    public partial class VolumeSeriesInfo
    {
        public partial class VolumeSeriesData
        {
            [JsonProperty("issue")]
            public virtual IList<IssueData> Issue { get; set; }

            [JsonProperty("orderNumber")]
            public virtual int? OrderNumber { get; set; }

            [JsonProperty("seriesBookType")]
            public virtual string SeriesBookType { get; set; }

            [JsonProperty("seriesId")]
            public virtual string SeriesId { get; set; }
        }
    }
}
