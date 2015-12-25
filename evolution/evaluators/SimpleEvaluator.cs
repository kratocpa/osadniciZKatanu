using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{

    class SimpleEvaluator : IFitnessEvaluator
    {

        public SimpleEvaluator()
        {
        }

        public void Evaluate(Population pop)
        {
            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                pop.population[i].fitness = FitnessFunction(pop.population[i]);
            }
        }

        double FitnessFunction(Individual Id)
        {
            double fitness = 0;
            for (int i = 0; i < Id.lengthOfArray; i++)
            {
                if (Id.individualArray[i] == 1)
                {
                    fitness += 1;
                }
            }
            return fitness;
        }
    }
}
