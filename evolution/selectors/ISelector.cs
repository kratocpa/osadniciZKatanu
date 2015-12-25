using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    interface ISelector
    {
        Population Select(Population from, int howMany);
    }
}
