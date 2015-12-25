using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    class Population : ICloneable
    {
        public List<Individual> population;
        public int sizeOfPopulation;
        public int lengthOfEachIndividual;
        public int upperEachIndividual, lowerEachIndividual;
        Random rnd;

        public Population(int lengthOfEachIndividual_, int upperEachIndividual_, int lowerEachIndividual_)
        {
            population = new List<Individual>();
            sizeOfPopulation = 0;
            lengthOfEachIndividual = lengthOfEachIndividual_;
            upperEachIndividual = upperEachIndividual_;
            lowerEachIndividual = lowerEachIndividual_;
            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public void AddIndividual(Individual newId)
        {
            population.Add(newId);
            sizeOfPopulation++;
        }

        public void GenerateRandomPopulation(int size)
        {
            sizeOfPopulation = size;
            population.Clear();
            for (int i = 0; i < sizeOfPopulation; i++)
            {
                Individual newId = new Individual(lengthOfEachIndividual, upperEachIndividual, lowerEachIndividual, i);
                for (int j = 0; j < lengthOfEachIndividual; j++)
                {
                    newId.individualArray[j] = rnd.Next(upperEachIndividual, lowerEachIndividual + 1);
                }
                population.Add(newId);
            }
        }

        public object Clone()
        {
            Population pop = new Population(lengthOfEachIndividual, upperEachIndividual, lowerEachIndividual);

            foreach (var curId in population)
            {
                Individual newId = (Individual)curId.Clone();
                pop.population.Add(newId);
            }

            return pop;
        }
    }
}
