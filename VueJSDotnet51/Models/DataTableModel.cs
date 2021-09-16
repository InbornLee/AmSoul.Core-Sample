using System.Collections;

namespace VueJSDotnet51.Models
{
    public class DataTableRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Order { get; set; }
        public string Sort { get; set; }

    }
    public class DataTableResponse
    {
        public class PagingInfo
        {
            public int Limit { get; set; }
            public int Offset { get; set; }
            public long Total { get; set; }
        }
        public PagingInfo Paging { get; set; }
        public ICollection Data { get; set; }

    }
}
