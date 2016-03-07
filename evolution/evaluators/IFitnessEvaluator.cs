using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    public interface IFitnessEvaluator
    {
        int Generation { get; set; }
        void Evaluate(Population pop);
    }
}
