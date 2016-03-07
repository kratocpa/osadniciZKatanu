using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    public class EvolutionAlgorithm
    {

        public int popSize; // velikost populace
        public int generationNo; // číslo kolikátá generace se právě počítá
        public List<IOperator> operators; // seznam operátorů které se mají použít (křížení, mutace)
        public ISelector matingSelectors; // selector na výběr partnerů pro křížení
        public IFitnessEvaluator eval; // funkce na ohodnocení jedince

        public enum mating { OnePtXOver, TwoPtXOver, UniformMating, none }
        public enum fitnessEvaluator { Basic, EbdWithEbd, ChangeRivals, none }

        public EvolutionAlgorithm(int popSize)
        {
            this.popSize = popSize;
            generationNo = 0;
            operators = new List<IOperator>();
            matingSelectors = new RouletteWheelSelector();
        }

        public Population Evolve(Population pop)
        {
            generationNo++;
            eval.Evaluate(pop);
            Population matingPool = matingSelectors.Select(pop, popSize);

            foreach (IOperator curOp in operators)
            {
                Population offspring = curOp.Operate(matingPool);
                matingPool = offspring;
            }

            return matingPool;
        }

    }
}
