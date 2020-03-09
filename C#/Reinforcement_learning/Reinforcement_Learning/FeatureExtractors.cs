using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Reinforcement_Learning
{
    public static class SimpleExtractor
    {
        public static Dictionary<string, float> GetFeatures(State state)
        {
            Dictionary<string, float> features = new Dictionary<string, float>();
            GFG gg = new GFG();
            state.ownPlayer.tiles_hand.Sort(gg);
            state.UpdatePossiblePool();

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
            features.Add("length of second longest dishonor suit", numbersOfDishonorSuits[1]);
            features.Add("length of third longest dishonor suit", numbersOfDishonorSuits[2]);
            features.Add("length of honor suit", numberOfHonor);
            if (numberOfHonor >= 11)
            {
                features.Add("is at least 11 honors", 1);
            }
            else
            {
                features.Add("is at least 11 honors", 0);
            }

            features.Add("number of triplets", GetNumberOfTriplets(state.ownPlayer));
            features.Add("number of duplets", GetNumberOfDuplets(state.ownPlayer));
            features.Add("number of sequences", GetNumberOfSequences(state.ownPlayer));
            features.Add("number of good half sequences", GetNumberOfGoodHalfSequences(state.ownPlayer));
            features.Add("number of melds", GetNumberOfMelds(state.ownPlayer));
            features.Add("number of melds with eyes", GetNumberOfMeldsWithEyes(state.ownPlayer));
            features.Add("number of orphans", GetNumberOfOrphans(state.ownPlayer));
            features.Add("is 13 orphans", Is_13_Orphans(state.ownPlayer) ? 1 : 0);
            features.Add("is win", IsWin(state.ownPlayer) ? 1 : 0);
            features.Add("is 10 score", Is10Score(state.ownPlayer));
            features.Add("is 8 score", Is8Score(state.ownPlayer));
            features.Add("is 7 score", Is7Score(state.ownPlayer));
            features.Add("is 6 score", Is6Score(state.ownPlayer));
            features.Add("is 3 score", Is3Score(state.ownPlayer));
            features.Add("number of tiles that improves 1 meld", GetNumberOfTilesThatImprovesOneMeld(state)); // 2+1 to 3+1, or 2+0 to 3+0
            features.Add("number of tiles that improves eyes", GetNumberOfTilesThatImprovesEyes(state)); // 3+0 to 3+1
            features.Add("remaining pool count", state.remaining_pool.Count);


            return features;
        }

        public static bool IsQuadruplet(Player player, Tile tile) //Only tiles_hand
        {
            int count = 0;
            foreach (Tile eachtile in player.tiles_hand)
            {
                if (eachtile.tile_integer == tile.tile_integer)
                {
                    count++;
                }
            }
            if (count == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsTriplet(Player player, Tile tile) //Only tiles_hand
        {
            int count = 0;
            foreach (Tile eachtile in player.tiles_hand)
            {
                if (eachtile.tile_integer == tile.tile_integer)
                {
                    count++;
                }
            }
            if (count == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDuplet(Player player, Tile tile) //Only tiles_hand
        {
            int count = 0;
            foreach (Tile eachtile in player.tiles_hand)
            {
                if (eachtile.tile_integer == tile.tile_integer)
                {
                    count++;
                }
            }
            if (count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsSingle(Player player, Tile tile) //Only tiles_hand
        {
            int count = 0;
            foreach (Tile eachtile in player.tiles_hand)
            {
                if (eachtile.tile_integer == tile.tile_integer)
                {
                    count++;
                }
            }
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsAllSingle(Player player)
        {
            bool allSingle = true;
            foreach (Tile tile in player.tiles_hand)
            {
                if (!IsSingle(player, tile))
                {
                    allSingle = false;
                    break;
                }
            }
            return allSingle;
        }
        public static int GetNumberOfTriplets(Player player)
        {
            int numberOfTriplets = 0;
            foreach (Tile tile in player.tiles_hand)
            {
                if (IsQuadruplet(player, tile))
                {
                    numberOfTriplets++;
                }
            }
            numberOfTriplets /= 4;
            int temp = numberOfTriplets;
            numberOfTriplets = 0;

            foreach (Tile tile in player.tiles_hand)
            {
                if (IsTriplet(player, tile))
                {
                    numberOfTriplets++;
                }
            }
            numberOfTriplets /= 3;
            numberOfTriplets += temp;

            foreach (List<Tile> tiles in player.tiles_displayed)
            {
                if (tiles[0].tile_integer == tiles[1].tile_integer)
                {
                    numberOfTriplets++;
                }
            }

            return numberOfTriplets;
        }
        public static int GetNumberOfDuplets(Player player)
        {
            int numberOfDuplets = 0;
            foreach (Tile tile in player.tiles_hand)
            {
                if (IsDuplet(player, tile))
                {
                    numberOfDuplets++;
                }
            }
            numberOfDuplets /= 2;

            return numberOfDuplets;
        }
        public static int GetNumberOfSequences(Player player)
        {
            int count = 0;
            List<Tile> tiles_hand_copy = new List<Tile>(player.tiles_hand);
            List<int> indexes = new List<int>();

            int RemoveSequence(int _count)
            {
                for (int j = 0; j < tiles_hand_copy.Count; j++)
                {
                    if (j == 0)
                    {
                        continue;
                    }
                    if (indexes.Count == 0)
                    {
                        if (tiles_hand_copy[j].tile_pattern <= 2 &&
                            tiles_hand_copy[j].tile_integer == tiles_hand_copy[j - 1].tile_integer + 1)
                        {
                            indexes.Add(j - 1);
                            indexes.Add(j);
                        }
                    }
                    else
                    {
                        if (tiles_hand_copy[j].tile_integer == tiles_hand_copy[j - 1].tile_integer)
                        {
                            continue;
                        }
                        else if (tiles_hand_copy[j].tile_pattern <= 2 &&
                            tiles_hand_copy[j].tile_integer == tiles_hand_copy[j - 1].tile_integer + 1)
                        {
                            indexes.Add(j);
                            _count++;
                            tiles_hand_copy.RemoveAt(indexes[2]);
                            tiles_hand_copy.RemoveAt(indexes[1]);
                            tiles_hand_copy.RemoveAt(indexes[0]);
                            indexes.Clear();
                            _count = RemoveSequence(_count);
                            return _count;
                        }
                        else
                        {
                            indexes.Clear();
                        }
                    }
                }
                return _count;
            }

            count = RemoveSequence(count);

            int i = 0;
            foreach (List<Tile> tiles in player.tiles_displayed)
            {
                if (tiles[0].tile_pattern <= 2 &&
                    tiles[1].tile_integer == tiles[0].tile_integer + 1)
                {
                    count++;
                }
                i++;
            }
            return count;
        }

        public static int GetNumberOfSequencesExceptMelds(Player player)
        {
            int count = 0;
            List<Tile> tiles_hand_copy = new List<Tile>(player.tiles_hand);
            List<int> indexes = new List<int>();

            int RemoveSequence(int _count)
            {
                for (int j = 0; j < tiles_hand_copy.Count; j++)
                {
                    if (j == 0)
                    {
                        continue;
                    }
                    if (indexes.Count == 0)
                    {
                        if (tiles_hand_copy[j].tile_pattern <= 2 &&
                            tiles_hand_copy[j].tile_integer == tiles_hand_copy[j - 1].tile_integer + 1)
                        {
                            indexes.Add(j - 1);
                            indexes.Add(j);
                        }
                    }
                    else
                    {
                        if (tiles_hand_copy[j].tile_integer == tiles_hand_copy[j - 1].tile_integer)
                        {
                            continue;
                        }
                        else if (tiles_hand_copy[j].tile_pattern <= 2 &&
                            tiles_hand_copy[j].tile_integer == tiles_hand_copy[j - 1].tile_integer + 1)
                        {
                            indexes.Add(j);
                            _count++;
                            tiles_hand_copy.RemoveAt(indexes[2]);
                            tiles_hand_copy.RemoveAt(indexes[1]);
                            tiles_hand_copy.RemoveAt(indexes[0]);
                            indexes.Clear();
                            _count = RemoveSequence(_count);
                            return _count;
                        }
                        else
                        {
                            indexes.Clear();
                        }
                    }
                }
                return _count;
            }

            count = RemoveSequence(count);

            return count;
        }

        public static int GetNumberOfGoodHalfSequences(Player player) //The two tiles should be isolated
        {
            List<Tile> tiles_hand = player.tiles_hand;
            int numberOfGoodHalfSequences = 0;

            for (int i = 0; i < tiles_hand.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else if (tiles_hand.Count == 2)
                {
                    if (tiles_hand[0].tile_pattern <= 2 &&
                        tiles_hand[1].tile_integer == tiles_hand[0].tile_integer + 1)
                    {
                        return 1;
                    }
                }
                else if (i == tiles_hand.Count - 1)
                {
                    if (tiles_hand[i - 1].tile_pattern <= 2 &&
                        tiles_hand[i].tile_integer == tiles_hand[i - 1].tile_integer + 1)
                    {
                        numberOfGoodHalfSequences++;
                    }
                }
                else
                {
                    bool hasAnotherHalf = false;
                    for (int j = 0; j < tiles_hand.Count - 1 - i; j++)
                    {
                        if (tiles_hand[i].tile_pattern <= 2 &&
                            tiles_hand[i + j + 1].tile_pattern <= 2 &&
                            tiles_hand[i + j + 1].tile_integer == tiles_hand[i].tile_integer + 1)
                        {
                            hasAnotherHalf = true;
                        }
                    }
                    if (i >= 2)
                    {
                        for (int j = 0; j < i - 2 + 1; j++)
                        {
                            if (tiles_hand[i].tile_pattern <= 2 &&
                                tiles_hand[i - j - 2].tile_pattern <= 2 &&
                                tiles_hand[i - j - 2].tile_integer == tiles_hand[i].tile_integer - 2)
                            {
                                hasAnotherHalf = true;
                            }
                        }
                    }
                    if (tiles_hand[i - 1].tile_pattern <= 2 &&
                        tiles_hand[i].tile_integer == tiles_hand[i - 1].tile_integer + 1 &&
                        !hasAnotherHalf)
                    {
                        numberOfGoodHalfSequences++;
                    }
                }
            }
            return numberOfGoodHalfSequences;
        }

        public static int GetNumberOfMelds(Player player)
        {
            List<Tile> tiles_hand = player.tiles_hand;

            List<int> indexes = new List<int>();
            int resume = 0;
            for (int i = 0; i < tiles_hand.Count; i++)
            {
                if (i < resume)
                {
                    continue;
                }
                if (IsQuadruplet(player, tiles_hand[i]))
                {
                    indexes.Add(i);
                    resume = i + 4;
                }
                else if (IsTriplet(player, tiles_hand[i]))
                {
                    indexes.Add(i);
                    resume = i + 3;
                }
            }
            int numberOfTriplets = indexes.Count;
            int maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player);
            if (numberOfTriplets == 4)
            {
                return 4;
            }
            for (int i = 0; i < numberOfTriplets; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < numberOfTriplets; j++)
                    {
                        Tile tile = tiles_hand[indexes[j]];
                        tiles_hand.RemoveAt(indexes[j]);
                        tiles_hand.RemoveAt(indexes[j]);
                        tiles_hand.RemoveAt(indexes[j]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 1)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 1;
                        }
                        tiles_hand.Insert(indexes[j], tile);
                        tiles_hand.Insert(indexes[j], tile);
                        tiles_hand.Insert(indexes[j], tile);
                    }
                }
                if (i == 1)
                {
                    if (numberOfTriplets >= 2)
                    {
                        Tile tile1 = tiles_hand[indexes[1]];
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        Tile tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[1], tile1);
                    }
                    if (numberOfTriplets >= 3)
                    {
                        Tile tile1 = tiles_hand[indexes[2]];
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        Tile tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        //////////
                        tile1 = tiles_hand[indexes[2]];
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                    }
                    if (numberOfTriplets == 4)
                    {
                        Tile tile1 = tiles_hand[indexes[3]];
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        Tile tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[3], tile1);
                        tiles_hand.Insert(indexes[3], tile1);
                        tiles_hand.Insert(indexes[3], tile1);
                        //////////
                        tile1 = tiles_hand[indexes[3]];
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tile0 = tiles_hand[indexes[1]];
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[3], tile1);
                        tiles_hand.Insert(indexes[3], tile1);
                        tiles_hand.Insert(indexes[3], tile1);
                        //////////
                        tile1 = tiles_hand[indexes[3]];
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tile0 = tiles_hand[indexes[2]];
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[2], tile0);
                        tiles_hand.Insert(indexes[2], tile0);
                        tiles_hand.Insert(indexes[2], tile0);
                        tiles_hand.Insert(indexes[3], tile1);
                        tiles_hand.Insert(indexes[3], tile1);
                        tiles_hand.Insert(indexes[3], tile1);
                    }
                }
                if (i == 2)
                {
                    if (numberOfTriplets >= 3)
                    {
                        Tile tile2 = tiles_hand[indexes[2]];
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        Tile tile1 = tiles_hand[indexes[1]];
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        Tile tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[2], tile2);
                        tiles_hand.Insert(indexes[2], tile2);
                        tiles_hand.Insert(indexes[2], tile2);
                    }
                    if (numberOfTriplets == 4)
                    {
                        Tile tile2 = tiles_hand[indexes[3]];
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        Tile tile1 = tiles_hand[indexes[1]];
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        Tile tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[1], tile1);
                        tiles_hand.Insert(indexes[3], tile2);
                        tiles_hand.Insert(indexes[3], tile2);
                        tiles_hand.Insert(indexes[3], tile2);
                        //////////
                        tile2 = tiles_hand[indexes[3]];
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tile1 = tiles_hand[indexes[2]];
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tile0 = tiles_hand[indexes[0]];
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        tiles_hand.RemoveAt(indexes[0]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[0], tile0);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[3], tile2);
                        tiles_hand.Insert(indexes[3], tile2);
                        tiles_hand.Insert(indexes[3], tile2);
                        //////////
                        tile2 = tiles_hand[indexes[3]];
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tiles_hand.RemoveAt(indexes[3]);
                        tile1 = tiles_hand[indexes[2]];
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tiles_hand.RemoveAt(indexes[2]);
                        tile0 = tiles_hand[indexes[1]];
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        tiles_hand.RemoveAt(indexes[1]);
                        if (maxNumberOfMelds < GetNumberOfSequencesExceptMelds(player) + 2)
                        {
                            maxNumberOfMelds = GetNumberOfSequencesExceptMelds(player) + 2;
                        }
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[1], tile0);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[2], tile1);
                        tiles_hand.Insert(indexes[3], tile2);
                        tiles_hand.Insert(indexes[3], tile2);
                        tiles_hand.Insert(indexes[3], tile2);
                    }
                }
            }
            maxNumberOfMelds += player.tiles_displayed.Count;
            return maxNumberOfMelds;
        }

        public static bool IsWin(Player player)
        {
            if (Is_13_Orphans(player))
            {
                return true;
            }
            List<Tile> tiles_hand = player.tiles_hand;
            if (tiles_hand.Count % 3 != 2)
            {
                return false;
            }
            for (int i = 0; i < tiles_hand.Count - 1; i++)
            {
                if (i < tiles_hand.Count - 2)
                {
                    if (tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer &&
                        tiles_hand[i].tile_integer != tiles_hand[i + 2].tile_integer)
                    {
                        Tile tile = tiles_hand[i];
                        tiles_hand.RemoveAt(i);
                        tiles_hand.RemoveAt(i);

                        int sum = 0;
                        foreach (Tile eachTile in tiles_hand)
                        {
                            sum += eachTile.tile_integer;
                        }
                        if (sum % 3 != 0)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            continue;
                        }

                        if (GetNumberOfMelds(player) == 4)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            return true;
                        }
                        else
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                        }
                    }
                }
                else
                {
                    if (tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer)
                    {
                        Tile tile = tiles_hand[i];
                        tiles_hand.RemoveAt(i);
                        tiles_hand.RemoveAt(i);

                        int sum = 0;
                        foreach (Tile eachTile in tiles_hand)
                        {
                            sum += eachTile.tile_integer;
                        }
                        if (sum % 3 != 0)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            continue;
                        }

                        if (GetNumberOfMelds(player) == 4)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            return true;
                        }
                        else
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                        }
                    }
                }
            }
            return false;
        }

        public static int GetNumberOfMeldsWithEyes(Player player)
        {
            List<Tile> tiles_hand = player.tiles_hand;

            if (IsAllSingle(player))
            {
                return 0;
            }

            int max = 0;
            for (int i = 0; i < tiles_hand.Count - 1; i++)
            {
                if (i < tiles_hand.Count - 2)
                {
                    if (tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer &&
                        tiles_hand[i].tile_integer != tiles_hand[i + 2].tile_integer)
                    {
                        Tile tile = tiles_hand[i];
                        tiles_hand.RemoveAt(i);
                        tiles_hand.RemoveAt(i);

                        int sum = 0;
                        foreach (Tile eachTile in tiles_hand)
                        {
                            sum += eachTile.tile_integer;
                        }

                        if (GetNumberOfMelds(player) == 4)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 4)
                            {
                                max = 4;
                            }
                        }
                        else if (GetNumberOfMelds(player) == 3)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 3)
                            {
                                max = 3;
                            }
                        }
                        else if (GetNumberOfMelds(player) == 2)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 2)
                            {
                                max = 2;
                            }
                        }
                        else if (GetNumberOfMelds(player) == 1)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 1)
                            {
                                max = 1;
                            }
                        }
                        else
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                        }
                    }
                }
                else
                {
                    if (tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer)
                    {
                        Tile tile = tiles_hand[i];
                        tiles_hand.RemoveAt(i);
                        tiles_hand.RemoveAt(i);

                        int sum = 0;
                        foreach (Tile eachTile in tiles_hand)
                        {
                            sum += eachTile.tile_integer;
                        }

                        if (GetNumberOfMelds(player) == 4)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 4)
                            {
                                max = 4;
                            }
                        }
                        else if (GetNumberOfMelds(player) == 3)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 3)
                            {
                                max = 3;
                            }
                        }
                        else if (GetNumberOfMelds(player) == 2)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 2)
                            {
                                max = 2;
                            }
                        }
                        else if (GetNumberOfMelds(player) == 1)
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                            if (max < 1)
                            {
                                max = 1;
                            }
                        }
                        else
                        {
                            tiles_hand.Insert(i, tile);
                            tiles_hand.Insert(i, tile);
                        }
                    }
                }
            }
            return max;
        }

        public static int GetWinScore(Player player)
        {
            List<List<Tile>> tiles_displayed = player.tiles_displayed;
            List<Tile> tiles_hand = player.tiles_hand;

            int score = 0;
            int maxScore = 20;

            if (IsWin(player))
            {
                List<Tile> tiles_13Or14 = player.GetTiles_13Or14();
                bool hasDot = false;
                bool hasBamboo = false;
                bool hasCharacter = false;
                bool hasHonor = false;
                foreach (Tile tile in tiles_13Or14)
                {
                    if (tile.tile_pattern == 0)
                    {
                        hasDot = true;
                    }
                    if (tile.tile_pattern == 1)
                    {
                        hasBamboo = true;
                    }
                    if (tile.tile_pattern == 2)
                    {
                        hasCharacter = true;
                    }
                    if (tile.tile_pattern == 3)
                    {
                        hasHonor = true;
                    }
                }

                if (hasDot && !hasBamboo && !hasCharacter && !hasHonor)
                {
                    score += 7;
                }
                if (!hasDot && hasBamboo && !hasCharacter && !hasHonor)
                {
                    score += 7;
                }
                if (!hasDot && !hasBamboo && hasCharacter && !hasHonor)
                {
                    score += 7;
                }
                if (!hasDot && !hasBamboo && !hasCharacter && hasHonor)
                {
                    score = maxScore;
                }
                if (hasDot && !hasBamboo && !hasCharacter && hasHonor)
                {
                    score += 3;
                }
                if (!hasDot && hasBamboo && !hasCharacter && hasHonor)
                {
                    score += 3;
                }
                if (!hasDot && !hasBamboo && hasCharacter && hasHonor)
                {
                    score += 3;
                }

                if (GetNumberOfTriplets(player) == 4)
                {
                    score += 3;
                }
            }
            return score;
        }

        public static int Is10Score(Player player)
        {
            if (GetWinScore(player) == 10)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int Is8Score(Player player)
        {
            if (GetWinScore(player) == 8)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int Is7Score(Player player)
        {
            if (GetWinScore(player) == 7)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int Is6Score(Player player)
        {
            if (GetWinScore(player) == 6)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int Is3Score(Player player)
        {
            if (GetWinScore(player) == 3)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int GetNumberOfOrphans(Player player) //Only tiles_hand
        {
            List<Tile> tiles_unique = new List<Tile>(new HashSet<Tile>(player.tiles_hand));
            int count = 0;

            new List<string> { "00", "08", "10", "18", "20", "28", "30", "31", "32", "33", "34", "35", "36" }.ForEach(orphan =>
            {
                if (tiles_unique.Any(tile => tile.tile_code == orphan))
                {
                    count++;
                }
            });

            return count;
        }

        public static bool Is_13_Orphans(Player player)
        {
            if (player.tiles_hand.Count != 14)
            {
                return false;
            }
            foreach (Tile tile in player.tiles_hand)
            {
                if (!new List<string> { "00", "08", "10", "18", "20", "28", "30", "31", "32", "33", "34", "35", "36" }.Contains(tile.tile_code))
                {
                    return false;
                }
            }
            if (GetNumberOfOrphans(player) == 13)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetNumberOfTilesThatImprovesOneMeld(State state)
        {
            int output = 0;
            int original_except_eyes_meld_count = GetNumberOfMelds(state.ownPlayer);
            int original_has_eyes_meld_count = GetNumberOfMeldsWithEyes(state.ownPlayer); // 0 if no duplets

            int SameFunctionWithMoreInput(State _state, int _original_except_eyes_meld_count, int _original_has_eyes_meld_count)
            {
                Player player = _state.ownPlayer;
                List<Tile> tiles_hand_copy = new List<Tile>(player.tiles_hand);

                int new_except_eyes_meld_count = 0;
                int new_has_eyes_meld_count = 0;
                int count = 0;

                if (tiles_hand_copy.Count % 3 == 1)
                {
                    foreach (Tile tile in _state.possible_pool)
                    {
                        if (tile.tile_pattern == 4)
                        {
                            continue;
                        }
                        tiles_hand_copy.Add(tile);
                        tiles_hand_copy = tiles_hand_copy.OrderBy(eachTile => eachTile.tile_integer).ToList();
                        Player new_player = new Player(0)
                        {
                            tiles_hand = new List<Tile>(tiles_hand_copy),
                            tiles_displayed = new List<List<Tile>>(player.tiles_displayed)
                        };
                        new_except_eyes_meld_count = GetNumberOfMelds(new_player);
                        new_has_eyes_meld_count = GetNumberOfMeldsWithEyes(new_player);
                        if (IsAllSingle(player))
                        {
                            if (new_except_eyes_meld_count > _original_except_eyes_meld_count)
                            {
                                count++;
                            }
                        }
                        else //(IsAllSingle(new_player))
                        {
                            if (new_except_eyes_meld_count > _original_except_eyes_meld_count &&
                                new_has_eyes_meld_count > _original_has_eyes_meld_count)
                            {
                                count++;
                            }
                        }
                        tiles_hand_copy.Remove(tile);
                    }
                }
                else
                {
                    Main.DebugLog("Unreasonable!");
                    return 0;
                }

                return count;
            }

            if (state.ownPlayer.tiles_hand.Count % 3 == 1)
            {
                output = SameFunctionWithMoreInput(state, original_except_eyes_meld_count, original_has_eyes_meld_count);
            }
            else if (state.ownPlayer.tiles_hand.Count % 3 == 2)
            {
                State state1;
                List<int> allScores = new List<int>();
                foreach (Tile tile in state.ownPlayer.tiles_hand)
                {
                    state1 = new State();
                    state1.DeepCopy(state);
                    state1.ownPlayer.tiles_hand.Remove(tile);
                    state1.discarded_pool.Add(tile);
                    state1.UpdatePossiblePool();
                    //Main.DebugLog($"Reinforcement_Learning: If I discard {tile.tile_code}:");
                    allScores.Add(SameFunctionWithMoreInput(state1, original_except_eyes_meld_count, original_has_eyes_meld_count));
                }
                output = allScores.Max();
            }
            return output;
        }
        public static int GetNumberOfTilesThatImprovesEyes(State state)
        {
            Player player = state.ownPlayer;
            return 0;
        }
    }
}
