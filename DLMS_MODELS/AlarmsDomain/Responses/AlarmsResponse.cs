using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.AlarmsDomain.Responses
{
    public class AlarmsResponse
    {
        public int Id { get; set; }
        public string Code1 { get; set; }
        public int Value { get; set; }

        public string Code2 { get; set; }
    }
}
