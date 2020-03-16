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
                { "length of longest dishonor suit", 40 },
                { "length of second longest dishonor suit", -30 },
                { "length of third longest dishonor suit", -40 },
                { "length of honor suit", -5 },
                { "is at least 11 honors", 500 },
                { "is 12 longest dishonor suit", 200 },
                { "is 13 longest dishonor suit", 400 },
                { "is 14 longest dishonor suit", 600 },
                { "is 2 second longest dishonor suit", 100 },
                { "is 1 second longest dishonor suit", 300 },
                { "is 0 second longest dishonor suit", 500 },
                { "number of prevailing wind honor", 12},
                { "number of dealer honor", 14},
                { "number of both prevailing wind and dealer honor", 30},
                { "number of dragon honor", 13 },
                { "is all single", -600 },
                { "number of triplets", 40 },
                { "number of duplets", 20 },
                { "number of remaining duplets", 30 },
                { "number of sequences", 40 },
                { "number of good half sequences", 5 },
                { "number of first neighbours within 2", 20 },
                { "number of displayed melds", 400 },
                { "number of hand melds", 400 },
                { "number of hand melds with eyes", 200 },
                { "number of orphans", -2 },
                { "number of 2 or 8", -1 },
                { "is 13 orphans", 2500 },
                { "is win", 1000 },
                { "is 10 score", 6400 },
                { "is 8 score", 3200 },
                { "is 7 score", 2400 },
                { "is 6 score", 1600 },
                { "is 3 score", 400 },
                { "number of tiles that improves 1 meld", 15 },
                { "number of tiles that improves eyes", 2 }, //from 0 to 1
                { "number of tiles that improves triplets",  15},
                { "number of tiles that wins", 25 },
                { "number of tiles that improves remaining duplets", 1 },
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
                state1.DeepCopy(state);
                state1.ownPlayer.tiles_hand.Remove(tile);
                state1.discarded_pool.Add(tile);
                state1.UpdatePossiblePool();
                allScores.Add(GetQValue(state1));
            }

            float maxScore = -1000000;
            int maxIndex = 0;
            List<int> maxIndexes = new List<int>();
            for (int i = 0; i < allScores.Count; i++)
            {
                Main.DebugLog("Reinforcement_Learning: score after discarding " + tiles_hand[i].tile_code + " : " + allScores[i].ToString());
                if (i == 0)
                {
                    maxScore = allScores[0];
                    maxIndex = 0;
                    maxIndexes.Add(0);
                    continue;
                }
                if (allScores[i] > maxScore)
                {
                    maxScore = allScores[i];
                    maxIndex = i;
                    maxIndexes.Clear();
                    maxIndexes.Add(i);
                }
                if (allScores[i] == maxScore)
                {
                    maxIndexes.Add(i);
                }
            }
            System.Random rnd = new System.Random();
            maxIndex = maxIndexes[rnd.Next(0, maxIndexes.Count)];
            return maxIndex;
        }
    }
}
