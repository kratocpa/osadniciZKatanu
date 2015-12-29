using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evolution
{
    public class EvolutionAlgorithm
    {

        public int popSize, generationNo;
        public List<IOperator> operators;
        public ISelector matingSelectors;
        public IFitnessEvaluator eval;


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
