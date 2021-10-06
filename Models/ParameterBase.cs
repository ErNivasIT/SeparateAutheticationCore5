using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSingleSignOn.Models
{
    public class ParameterBase
    {
        public IEnumerable<ParameterPerson> People { get; set; }
    }
}
