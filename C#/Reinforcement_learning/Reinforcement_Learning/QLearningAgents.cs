using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Reinforcement_Learning
{
    public static class ApproximateQAgent
    {
        static Dictionary<string, float> features = new Dictionary<string, float>();
        static Dictionary<string, float> weights = new Dictionary<string, float>
            {
                { "length of longest dishonor suit", 20 },
                { "length of second and third longest dishonor suit", -35 },
                { "length of honor suit", -18 },
                { "is at least 11 honors", 500 },
                { "number of triplets", 25 },
                { "number of duplets", 10 },
                { "number of sequences", 25 },
                { "number of good half sequences", 5 },
                { "number of melds", 400 },
                { "number of melds with eyes", 200 },
                { "number of orphans", -1 },
                { "is 13 orphans", 2500 },
                { "is win", 1000 },
                { "is 10 score", 6400 },
                { "is 8 score", 3200 },
                { "is 7 score", 2400 },
                { "is 6 score", 1600 },
                { "is 3 score", 400 },
                { "remaining pool count", 1 }
            };

    public static Dictionary<string, float> GetWeights()
        {
            return weights;
        }

        public static float GetQValue(State state)
        {
            features = SimpleExtractor.GetFeatures(state);
            float sum = 0;
            foreach (KeyValuePair<string, float> kvp in features)
            {
                sum += weights[kvp.Key] * kvp.Value;
            }
            return sum;
        }
    }
}
 