using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Reinforcement_Learning
{
    public class FeatureExtractor
    {
        public virtual Dictionary<string, float> GetFeatures(State state)
        {
            return new Dictionary<string, float>();
        }
    }

    public class SimpleExtractor : FeatureExtractor
    {
        public override Dictionary<string, float> GetFeatures(State state)
        {
            Dictionary<string, float> features = new Dictionary<string, float>();
            GFG gg = new GFG();
            state.ownPlayer.tiles_hand.Sort(gg);

            List<Tile> tiles_13Or14 = state.ownPlayer.GetTiles_13Or14();

            /*
             * Count the length of different suits
             - Exluding quadruplets
            */

            int numberOfDot = 0;
            foreach (Tile tile in tiles_13Or14)
            {
                if (tile.tile_pattern == 0)
                {
                    numberOfDot++;
                }
            }
            int numberOfBamboo = 0;
            foreach (Tile tile in tiles_13Or14)
            {
                if (tile.tile_pattern == 1)
                {
                    numberOfBamboo++;
                }
            }
            int numberOfCharacter = 0;
            foreach (Tile tile in tiles_13Or14)
            {
                if (tile.tile_pattern == 2)
                {
                    numberOfCharacter++;
                }
            }
            int numberOfHonor = 0;
            foreach (Tile tile in tiles_13Or14)
            {
                if (tile.tile_pattern == 3)
                {
                    numberOfHonor++;
                }
            }

            List<int> numbersOfDishonorSuits = new List<int>();
            numbersOfDishonorSuits.Add(numberOfDot);
            numbersOfDishonorSuits.Add(numberOfBamboo);
            numbersOfDishonorSuits.Add(numberOfCharacter);
            numbersOfDishonorSuits.Sort((a, b) => b.CompareTo(a));

            features.Add("length of longest dishonor suit", numbersOfDishonorSuits[0]);
            features.Add("length of second and third longest dishonor suit", numbersOfDishonorSuits[1] + numbersOfDishonorSuits[2]);
            features.Add("length of honor suit", numberOfHonor);
            if (numberOfHonor >= 11)
            {
                features.Add("is at least 11 honors", 1);
            }
            else
            {
                features.Add("is at least 11 honors", 0);
            }



            return features;
        }
    }
}
 