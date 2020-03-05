using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using IronXL;

namespace Reinforcement_Learning
{
    public class Main
    {
        public static void Start(string[] args)
        {
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
            GFG gg = new GFG();
            oneGame.state.ownPlayer.tiles_hand.Sort(gg);
            oneGame.state.Print();*/

            /*State state = new State
            {
                ownPlayer = new Player(0)
            };
            state.ownPlayer.tiles_hand.Add(new Tile("10"));
            state.ownPlayer.tiles_hand.Add(new Tile("10"));
            state.ownPlayer.tiles_hand.Add(new Tile("11"));
            state.ownPlayer.tiles_hand.Add(new Tile("11"));
            state.ownPlayer.tiles_hand.Add(new Tile("12"));
            state.ownPlayer.tiles_hand.Add(new Tile("12"));
            state.ownPlayer.tiles_hand.Add(new Tile("12"));
            state.ownPlayer.tiles_hand.Add(new Tile("13"));
            state.ownPlayer.tiles_hand.Add(new Tile("13"));
            state.ownPlayer.tiles_hand.Add(new Tile("14"));
            state.ownPlayer.tiles_hand.Add(new Tile("16"));
            state.ownPlayer.tiles_hand.Add(new Tile("17"));
            state.ownPlayer.tiles_hand.Add(new Tile("18"));
            state.ownPlayer.tiles_hand.Add(new Tile("13"));
            GFG gg = new GFG();
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
            Main.DebugLog(ApproximateQAgent.GetQValue(state));*/


            //GenerateTestCases(2, 201);
            GenerateNeighbouringTestCases(2, 200);
            RunTestCases(2, 3200);
        }

        public static void DebugLog(object message)
        {
            Console.WriteLine(message);
        }

        public static void GenerateTestCases(int first_index, int last_index)
        {
            WorkBook workbook = WorkBook.Load("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score.xlsx");
            WorkSheet sheet = workbook.DefaultWorkSheet;

            for (int i = 0; i < last_index - first_index + 1; i++)
            {
                // Create random initial pool of tiles
                List<Tile> remaining_pool = new List<Tile>();
                for (int j = 0; j < 4; j++)
                {
                    remaining_pool.Add(new Tile("00"));
                    remaining_pool.Add(new Tile("01"));
                    remaining_pool.Add(new Tile("02"));
                    remaining_pool.Add(new Tile("03"));
                    remaining_pool.Add(new Tile("04"));
                    remaining_pool.Add(new Tile("05"));
                    remaining_pool.Add(new Tile("06"));
                    remaining_pool.Add(new Tile("07"));
                    remaining_pool.Add(new Tile("08"));
                    remaining_pool.Add(new Tile("10"));
                    remaining_pool.Add(new Tile("11"));
                    remaining_pool.Add(new Tile("12"));
                    remaining_pool.Add(new Tile("13"));
                    remaining_pool.Add(new Tile("14"));
                    remaining_pool.Add(new Tile("15"));
                    remaining_pool.Add(new Tile("16"));
                    remaining_pool.Add(new Tile("17"));
                    remaining_pool.Add(new Tile("18"));
                    remaining_pool.Add(new Tile("20"));
                    remaining_pool.Add(new Tile("21"));
                    remaining_pool.Add(new Tile("22"));
                    remaining_pool.Add(new Tile("23"));
                    remaining_pool.Add(new Tile("24"));
                    remaining_pool.Add(new Tile("25"));
                    remaining_pool.Add(new Tile("26"));
                    remaining_pool.Add(new Tile("27"));
                    remaining_pool.Add(new Tile("28"));
                    remaining_pool.Add(new Tile("30"));
                    remaining_pool.Add(new Tile("31"));
                    remaining_pool.Add(new Tile("32"));
                    remaining_pool.Add(new Tile("33"));
                    remaining_pool.Add(new Tile("34"));
                    remaining_pool.Add(new Tile("35"));
                    remaining_pool.Add(new Tile("36"));
                }
                remaining_pool.Add(new Tile("40"));
                remaining_pool.Add(new Tile("41"));
                remaining_pool.Add(new Tile("42"));
                remaining_pool.Add(new Tile("43"));
                remaining_pool.Add(new Tile("44"));
                remaining_pool.Add(new Tile("45"));
                remaining_pool.Add(new Tile("46"));
                remaining_pool.Add(new Tile("47"));
                Util.Shuffle<Tile>(remaining_pool);
                List<Tile> tiles_hand = new List<Tile>();
                List<Tile> tiles_flower = new List<Tile>();
                for (int j = 0; j < 14; j++)
                {
                    while (remaining_pool[0].tile_pattern == 4)
                    {
                        tiles_flower.Add(remaining_pool[0]);
                        remaining_pool.RemoveAt(0);
                    }
                    tiles_hand.Add(remaining_pool[0]);
                    remaining_pool.RemoveAt(0);
                }
                GFG gg = new GFG();
                tiles_hand.Sort(gg);
                new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" }.ForEach((character) =>
                {
                    sheet[$"{character}{first_index + i}"].Value = tiles_hand[0].tile_code;
                    tiles_hand.RemoveAt(0);
                });
            }
            workbook.SaveAs("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score_output.xlsx");

            // Copy back
            workbook = WorkBook.Load("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score_output.xlsx");
            workbook.SaveAs("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score.xlsx");
        }

        public static void GenerateNeighbouringTestCases(int first_index, int number_of_group)
        {
            WorkBook workbook = WorkBook.Load("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score.xlsx");
            WorkSheet sheet = workbook.DefaultWorkSheet;

            for (int i = 0; i < number_of_group; i++)
            {
                // Create random initial pool of tiles
                List<Tile> remaining_pool = new List<Tile>();
                for (int j = 0; j < 4; j++)
                {
                    remaining_pool.Add(new Tile("00"));
                    remaining_pool.Add(new Tile("01"));
                    remaining_pool.Add(new Tile("02"));
                    remaining_pool.Add(new Tile("03"));
                    remaining_pool.Add(new Tile("04"));
                    remaining_pool.Add(new Tile("05"));
                    remaining_pool.Add(new Tile("06"));
                    remaining_pool.Add(new Tile("07"));
                    remaining_pool.Add(new Tile("08"));
                    remaining_pool.Add(new Tile("10"));
                    remaining_pool.Add(new Tile("11"));
                    remaining_pool.Add(new Tile("12"));
                    remaining_pool.Add(new Tile("13"));
                    remaining_pool.Add(new Tile("14"));
                    remaining_pool.Add(new Tile("15"));
                    remaining_pool.Add(new Tile("16"));
                    remaining_pool.Add(new Tile("17"));
                    remaining_pool.Add(new Tile("18"));
                    remaining_pool.Add(new Tile("20"));
                    remaining_pool.Add(new Tile("21"));
                    remaining_pool.Add(new Tile("22"));
                    remaining_pool.Add(new Tile("23"));
                    remaining_pool.Add(new Tile("24"));
                    remaining_pool.Add(new Tile("25"));
                    remaining_pool.Add(new Tile("26"));
                    remaining_pool.Add(new Tile("27"));
                    remaining_pool.Add(new Tile("28"));
                    remaining_pool.Add(new Tile("30"));
                    remaining_pool.Add(new Tile("31"));
                    remaining_pool.Add(new Tile("32"));
                    remaining_pool.Add(new Tile("33"));
                    remaining_pool.Add(new Tile("34"));
                    remaining_pool.Add(new Tile("35"));
                    remaining_pool.Add(new Tile("36"));
                }
                remaining_pool.Add(new Tile("40"));
                remaining_pool.Add(new Tile("41"));
                remaining_pool.Add(new Tile("42"));
                remaining_pool.Add(new Tile("43"));
                remaining_pool.Add(new Tile("44"));
                remaining_pool.Add(new Tile("45"));
                remaining_pool.Add(new Tile("46"));
                remaining_pool.Add(new Tile("47"));
                Util.Shuffle<Tile>(remaining_pool);
                List<Tile> tiles_hand = new List<Tile>();
                List<Tile> tiles_flower = new List<Tile>();
                for (int j = 0; j < 14; j++)
                {
                    while (remaining_pool[0].tile_pattern == 4)
                    {
                        tiles_flower.Add(remaining_pool[0]);
                        remaining_pool.RemoveAt(0);
                    }
                    tiles_hand.Add(remaining_pool[0]);
                    remaining_pool.RemoveAt(0);
                }
                GFG gg = new GFG();
                tiles_hand.Sort(gg);
                List<Tile> tiles_hand_copy = new List<Tile>(tiles_hand);
                new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N" }.ForEach((character) =>
                {
                    sheet[$"{character}{first_index + 16 * i}"].Value = tiles_hand[0].tile_code;
                    tiles_hand.RemoveAt(0);
                });
                for (int j = 0; j < 14; j++)
                {
                    tiles_hand = new List<Tile>(tiles_hand_copy);
                    tiles_hand.RemoveAt(j);
                    new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" }.ForEach((character) =>
                    {
                        sheet[$"{character}{first_index + 16 * i + 1 + j}"].Value = tiles_hand[0].tile_code;
                        tiles_hand.RemoveAt(0);
                    });
                }
            }
            workbook.SaveAs("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score_output.xlsx");

            // Copy back
            workbook = WorkBook.Load("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score_output.xlsx");
            workbook.SaveAs("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score.xlsx");
        }

        public static void RunTestCases(int first_index, int last_index)
        {
            WorkBook workbook = WorkBook.Load("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score.xlsx");
            WorkSheet sheet = workbook.DefaultWorkSheet;
            for (int i = 0; i < last_index - first_index + 1; i++)
            {
                Main.DebugLog(System.IO.Directory.GetCurrentDirectory());
                State state = new State
                {
                    ownPlayer = new Player(0)
                };

                Range range = sheet[$"A{first_index + i}:N{first_index + i}"];
                foreach (Cell cell in range)
                {
                    if (!cell.IsEmpty)
                    {
                        state.ownPlayer.tiles_hand.Add(new Tile(cell.Value.ToString()));
                    }
                }
                GFG gg = new GFG();
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

                sheet[$"O{first_index + i}"].Value = ApproximateQAgent.GetQValue(state);
            }
            workbook.SaveAs("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score_output.xlsx");

            // Copy back
            workbook = WorkBook.Load("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score_output.xlsx");
            workbook.SaveAs("/Users/michael/mahjong/C#/Reinforcement_learning/Reinforcement_learning/hands_and_score.xlsx");
        }
    }
}

