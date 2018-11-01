using Newtonsoft.Json;

namespace Libro.Google
{

    public partial class VolumeSeriesInfo
    {
        public partial class VolumeSeriesData
        {
            public class IssueData
            {
                [JsonProperty("issueDisplayNumber")]
                public virtual string IssueDisplayNumber { get; set; }

                [JsonProperty("issueOrderNumber")]
                public virtual int? IssueOrderNumber { get; set; }

            }
        }
    }
}
