using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.CodeObisDomain.Responses
{
    public class CodeObisResponse
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Code1 { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string? Code2 { get; set; }
        public string? Unit { get; set; }
    }
}
