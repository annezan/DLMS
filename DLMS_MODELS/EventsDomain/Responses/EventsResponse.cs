using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.EventsDomain.Responses
{
    public class EventsResponse
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Code1 { get; set; } = null!;

        public int Value { get; set; }
        public bool? BitMask { get; set; }

        public string? Code2 { get; set; }
    }
}
