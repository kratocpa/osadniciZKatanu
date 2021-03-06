﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace evolution
{
    public class Individual : ICloneable
    {
        public int upper; // každá hodnota jedince je menší než tato hodnota
        public int lower; // každý hodnota jedince je větší než tato hodnota
        public int seed; // hodnota pro generátor náhodných čísel
        public int[] individualArray; // pole hodnot jedince
        public int lengthOfArray;
        public double fitness; // fitness jedince

        public Individual(int lengthOfArray_, int upper_, int lower_, int seed_)
        {
            lengthOfArray = lengthOfArray_;
            upper = upper_;
            lower = lower_;
            seed = seed_;
            individualArray = new int[lengthOfArray];

            fitness = 0;
        }

        public Individual(GenerateMovesProperties mvProp, int fitness_, int upper_, int lower_)
        {
            lengthOfArray = mvProp.Parameters.Count;
            upper = upper_;
            lower = lower_;
            fitness = fitness_;
            individualArray = new int[lengthOfArray];
            for (int i = 0; i < lengthOfArray; i++)
            {
                individualArray[i] = (int)mvProp.Parameters[i].Scale;
            }
        }

        public object Clone()
        {
            Individual newId = new Individual(lengthOfArray, upper, lower, seed);

            for (int i = 0; i < lengthOfArray; i++)
            {
                newId.individualArray[i] = individualArray[i];
            }
            newId.fitness = fitness;

            return newId;
        }

    }
}
