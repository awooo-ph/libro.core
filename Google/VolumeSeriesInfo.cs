using System.Collections.Generic;
using Newtonsoft.Json;

namespace Libro.Google
{

    public partial class VolumeSeriesInfo
    {
        [JsonProperty("bookDisplayNumber")]
        public virtual string BookDisplayNumber { get; set; }

        [JsonProperty("kind")]
        public virtual string Kind { get; set; }

        [JsonProperty("shortSeriesBookTitle")]
        public virtual string ShortSeriesBookTitle { get; set; }

        [JsonProperty("volumeSeries")]
        public virtual IList<VolumeSeriesData> VolumeSeries { get; set; }

        public virtual string ETag { get; set; }
    }
}
