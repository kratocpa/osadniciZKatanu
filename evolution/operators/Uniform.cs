﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    class Uniform : IOperator
    {
        double xOverProb; // pravděpodobnost křížení
        Random rnd;

        public Uniform(double xOverProb)
        {
            this.xOverProb = xOverProb;
            rnd = new Random();
        }

        public Population Operate(Population parents)
        {
            Population offspring = new Population(parents.lengthOfEachIndividual, parents.upperEachIndividual, parents.lowerEachIndividual);

            for (int i = 0; i < parents.sizeOfPopulation / 2; i++)
            {
                Individual p1 = parents.population[i];
                Individual p2 = parents.population[2 * i + 1];

                Individual o1 = (Individual)p1.Clone();
                Individual o2 = (Individual)p2.Clone();
                if (rnd.NextDouble() < xOverProb)
                {
                    for (int j = 0; j < parents.lengthOfEachIndividual; j++)
                    {
                        if (rnd.Next(0, 2) == 1)
                        {
                            o1.individualArray[j] = p2.individualArray[j];
                            o2.individualArray[j] = p1.individualArray[j];
                        }
                    }
                }
                offspring.AddIndividual(o1);
                offspring.AddIndividual(o2);
            }
            return offspring;
        }
    }
}
