using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.ErrorDomain.Responses
{
    public class ErrorResponse
    {
        public int Id { get; set; }
        public string Code1 { get; set; } = null!;

        public int Value { get; set; }

        public string? Code2 { get; set; }
    }
}
