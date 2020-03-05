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

        public static int GetDiscardDecision(State state)
        {
            List<float> allScores = new List<float>();
            List<Tile> tiles_hand = state.ownPlayer.tiles_hand;
            State state1;
            state.Print();

            Main.DebugLog("Reinforcement_Learning: original score: " + GetQValue(state).ToString());
            foreach (Tile tile in tiles_hand)
            {
                state1 = new State();
                state1.ownPlayer.tiles_hand = new List<Tile>(tiles_hand);
                state1.ownPlayer.tiles_hand.Remove(tile);
                allScores.Add(GetQValue(state1));
            }

            float maxScore = -1000000;
            int maxIndex = 0;
            for (int i = 0; i < allScores.Count; i++)
            {
                Main.DebugLog("Reinforcement_Learning: score after discarding " + tiles_hand[i].tile_code + " : " + allScores[i].ToString());
                if (i == 0)
                {
                    maxScore = allScores[0];
                    maxIndex = 0;
                    continue;
                }
                if (allScores[i] > maxScore)
                {
                    maxScore = allScores[i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
    }
}
 