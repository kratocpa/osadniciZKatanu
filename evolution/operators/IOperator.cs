using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    interface IOperator
    {
        Population Operate(Population parrents);
    }
}
