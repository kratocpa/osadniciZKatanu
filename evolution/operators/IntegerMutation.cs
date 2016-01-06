using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    public class IntegerMutation : IOperator
    {
        double mutationProb; // pravděpodobnost mutace
        double genChangeProb; // v případě že se jedinec mutuje, tak pravděpodobnost změnění  konkrétní hodnoty (pro každou hodnotu jedince)

        Random rnd;

        public IntegerMutation(double mutationProb, double genChangeProb)
        {
            this.mutationProb = mutationProb;
            this.genChangeProb = genChangeProb;
            rnd = new Random();
        }

        public Population Operate(Population parents)
        {
            Population offspring = new Population(parents.lengthOfEachIndividual, parents.upperEachIndividual, parents.lowerEachIndividual);
            for (int i = 0; i < parents.sizeOfPopulation; i++)
            {
                Individual o1 = (Individual)parents.population[i].Clone();

                if (rnd.NextDouble() < mutationProb)
                {
                    for (int j = 0; j < parents.lengthOfEachIndividual; j++)
                    {
                        if (rnd.NextDouble() < genChangeProb)
                        {
                            o1.individualArray[j] = rnd.Next(o1.upper, o1.lower);
                        }
                    }
                }

                offspring.AddIndividual(o1);
            }

            return offspring;
        }
    }
}
