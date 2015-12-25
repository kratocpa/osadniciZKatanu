using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    class RouletteWheelSelector : ISelector
    {
        Random rnd;

        public RouletteWheelSelector()
        {
            rnd = new Random();
        }

        public Population Select(Population from, int howMany)
        {
            double sum = 0;

            for (int i = 0; i < from.sizeOfPopulation; i++)
            {
                sum += from.population[i].fitness;
            }

            Population pop = new Population(from.lengthOfEachIndividual, from.upperEachIndividual, from.lowerEachIndividual);

            for (int i = 0; i < howMany; i++)
            {
                double ball = rnd.Next(0, (int)sum);
                double curSum = 0;
                int j = 0;
                while (ball > curSum + from.population[j].fitness)
                {
                    curSum += from.population[j].fitness;
                    j++;
                }

                Individual Ic = (Individual)from.population[j].Clone();
                pop.AddIndividual(Ic);
            }

            return pop;
        }
    }
}
