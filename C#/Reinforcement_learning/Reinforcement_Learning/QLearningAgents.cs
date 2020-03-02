using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Reinforcement_Learning
{
    public class ApproximateQAgent
    {
        Dictionary<string, float> features;
        Dictionary<string, float> weights;

        public ApproximateQAgent()
        {
            features = new Dictionary<string, float>();
            weights = new Dictionary<string, float>();
            weights.Add("length of longest dishonor suit", 20);
            weights.Add("length of second and third longest dishonor suit", -35);
            weights.Add("length of honor suit", -18);
            weights.Add("is at least 11 honors", 500);

            

        }

        public Dictionary<string, float> GetWeights()
        {
            return weights;
        }

        public float GetQValue(State state)
        {
            SimpleExtractor simpleExtractor = new SimpleExtractor();
            features = simpleExtractor.GetFeatures(state);
            float sum = 0;
            foreach (KeyValuePair<string, float> kvp in features)
            {
                sum += weights[kvp.Key] * kvp.Value;
            }
            return sum;
        }
    }
}
 