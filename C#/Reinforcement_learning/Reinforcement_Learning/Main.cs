using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Reinforcement_Learning
{
    public class Main
    {
        public static void Start(string[] args)
        {
            GFG gg;

            /*Reinforcement_Learning.Tile tile0 = new Reinforcement_Learning.Tile("04");
            Main.DebugLog(tile0);
            Main.DebugLog(tile0.tile_pattern);
            Player player = new Player(0);
            player.tiles_hand.Add(tile0);
            DebugLog(player.tiles_hand);
            OneGame oneGame = new OneGame(2, 3);
            DebugLog(oneGame.remaining_pool[1].tile_code);
            DebugLog(oneGame.state.remaining_pool[1].tile_code);
            oneGame.state.Print();
            gg = new GFG();
            oneGame.state.ownPlayer.tiles_hand.Sort(gg);
            oneGame.state.Print();*/

            State state = new State
            {
                ownPlayer = new Player(0)
            };
            state.ownPlayer.tiles_displayed.Add(new List<Tile>());
            state.ownPlayer.tiles_displayed[0].Add(new Tile("10"));
            state.ownPlayer.tiles_displayed[0].Add(new Tile("11"));
            state.ownPlayer.tiles_displayed[0].Add(new Tile("12"));

            state.ownPlayer.tiles_hand.Add(new Tile("22"));
            state.ownPlayer.tiles_hand.Add(new Tile("22"));
            state.ownPlayer.tiles_hand.Add(new Tile("22"));
            state.ownPlayer.tiles_hand.Add(new Tile("12"));
            state.ownPlayer.tiles_hand.Add(new Tile("13"));
            state.ownPlayer.tiles_hand.Add(new Tile("14"));
            state.ownPlayer.tiles_hand.Add(new Tile("14"));
            state.ownPlayer.tiles_hand.Add(new Tile("15"));
            state.ownPlayer.tiles_hand.Add(new Tile("17"));
            state.ownPlayer.tiles_hand.Add(new Tile("18"));
            state.ownPlayer.tiles_hand.Add(new Tile("33"));

            // Create random discarded pool
            state.discarded_pool.Add(new Tile("00"));
            state.discarded_pool.Add(new Tile("03"));
            state.discarded_pool.Add(new Tile("06"));
            state.discarded_pool.Add(new Tile("06"));
            state.discarded_pool.Add(new Tile("10"));
            state.discarded_pool.Add(new Tile("12"));
            state.discarded_pool.Add(new Tile("13"));
            state.discarded_pool.Add(new Tile("13"));
            state.discarded_pool.Add(new Tile("16"));
            state.discarded_pool.Add(new Tile("20"));
            state.discarded_pool.Add(new Tile("21"));
            //state.discarded_pool.Add(new Tile("13"));

            state.UpdatePossiblePool();

            gg = new GFG();
            state.ownPlayer.tiles_hand.Sort(gg);
            Main.DebugLog("state: ");
            state.Print();

            Dictionary<string, float> features = SimpleExtractor.GetFeatures(state);
            Main.DebugLog("features: ");
            foreach (KeyValuePair<string, float> kvp in features)
            {
                Main.DebugLog("\"" + kvp.Key + "\": " + kvp.Value);
            }
            Dictionary<string, float> weights = ApproximateQAgent.GetWeights();
            Main.DebugLog("weights: ");
            foreach (KeyValuePair<string, float> kvp in weights)
            {
                Main.DebugLog("\"" + kvp.Key + "\": " + kvp.Value);
            }

            Main.DebugLog("value for the state: ");
            Main.DebugLog(ApproximateQAgent.GetQValue(state));

            Main.DebugLog(ApproximateQAgent.GetDiscardDecision(state));

            //GenerateTestCases(2, 201);
            //Excel.GenerateNeighbouringTestCases(2, 200);
            //Excel.RunTestCases(2, 3200);
        }

        public static void DebugLog(object message)
        {
            Console.WriteLine(message);
        }
    }
}

