using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Reinforcement_Learning
{
    public enum DiscardBy
    {
        NOBODY,
        PLAYER_0,
        PLAYER_1,
        PLAYER_2,
        PLAYER_3
    }

    public class Tile
    {
        public string tile_code;
        public int tile_integer;
        public int tile_pattern;
        public int tile_number;
        public DiscardBy discardedBy;

        public Tile(string _tile_code)
        {
            tile_code = _tile_code;
            tile_integer = int.Parse(tile_code);
            tile_pattern = int.Parse(tile_code) / 10;
            tile_number = int.Parse(tile_code) % 10;
            discardedBy = DiscardBy.NOBODY;
        }

        public int CompareTo(Tile y)
        {
            if (tile_integer < y.tile_integer)
            {
                return -1;
            }
            else if (tile_integer == y.tile_integer)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    class GFG : IComparer<Tile>
    {
        public int Compare(Tile x, Tile y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            return x.CompareTo(y);
        }
    }

    public class Player
    {
        public int player_number;
        public List<Tile> tiles_hand;
        public List<List<Tile>> tiles_displayed;
        public List<Tile> tiles_flower;

        public Player(int _player_number)
        {
            player_number = _player_number;
            tiles_hand = new List<Tile>();
            tiles_displayed = new List<List<Tile>>();
            tiles_flower = new List<Tile>();
        }

        public List<Tile> GetTiles_13Or14()
        {
            List<Tile> tiles_13Or14 = new List<Tile>();
            foreach (List<Tile> tiles in tiles_displayed)
            {
                int i = 0;
                foreach (Tile tile in tiles)
                {
                    if (i != 3)
                    {
                        tiles_13Or14.Add(tile);
                    }
                    i++;
                }
            }
            foreach (Tile tile in tiles_hand)
            {
                tiles_13Or14.Add(tile);
            }
            return tiles_13Or14;
        }
    }

    public class State
    {
        public Player ownPlayer;
        public Player nextPlayer;
        public Player oppositePlayer;
        public Player lastPlayer;

        public List<Tile> remaining_pool;
        public List<Tile> discarded_pool;
        public List<Tile> possible_pool;

        public State()
        {
            ownPlayer = new Player(0);
            nextPlayer = new Player(1);
            oppositePlayer = new Player(2);
            lastPlayer = new Player(3);

            remaining_pool = new List<Tile>();
            discarded_pool = new List<Tile>();
            possible_pool = new List<Tile>();
        }

        public void Print()
        {
            string message;
            message = "Reinforcement_Learning: ";
            message += "{\"tiles_displayed\": ";
            if (ownPlayer.tiles_displayed.Count != 0)
            {
                message += "[";
            }
            int i = 0;
            foreach (List<Tile> tiles in ownPlayer.tiles_displayed)
            {
                int j = 0;
                if (j == 0)
                {
                    message += "[";
                }
                foreach (Tile tile in tiles)
                {
                    message += tile.tile_code;
                    if (j != tiles.Count - 1)
                    {
                        message += ", ";
                    }
                    else
                    {
                        message += "]";
                    }
                    j++;
                }
                if (i != ownPlayer.tiles_displayed.Count - 1)
                {
                    message += ", ";
                }
                else
                {
                    message += "]";
                }
                i++;
            }

            message += ", \"tiles_hand\": [";
            i = 0;
            foreach (Tile tile in ownPlayer.tiles_hand)
            {
                message += tile.tile_code;
                if (i != ownPlayer.tiles_hand.Count - 1)
                {
                    message += ", ";
                }
                else
                {
                    message += "]";
                }
                i++;
            }
            message += "}";

            Main.DebugLog(message);
        }

        public void DeepCopy(State state)
        {
            ownPlayer.tiles_hand = new List<Tile>(state.ownPlayer.tiles_hand);
            ownPlayer.tiles_displayed = new List<List<Tile>>(state.ownPlayer.tiles_displayed);
            ownPlayer.tiles_flower = new List<Tile>(state.ownPlayer.tiles_flower);
            remaining_pool = new List<Tile>(state.remaining_pool);
            discarded_pool = new List<Tile>(state.discarded_pool);
            possible_pool = new List<Tile>(state.possible_pool);
        }

        public void UpdatePossiblePool()
        {
            possible_pool.Clear();
            for (int i = 0; i < 4; i++)
            {
                possible_pool.Add(new Tile("00"));
                possible_pool.Add(new Tile("01"));
                possible_pool.Add(new Tile("02"));
                possible_pool.Add(new Tile("03"));
                possible_pool.Add(new Tile("04"));
                possible_pool.Add(new Tile("05"));
                possible_pool.Add(new Tile("06"));
                possible_pool.Add(new Tile("07"));
                possible_pool.Add(new Tile("08"));
                possible_pool.Add(new Tile("10"));
                possible_pool.Add(new Tile("11"));
                possible_pool.Add(new Tile("12"));
                possible_pool.Add(new Tile("13"));
                possible_pool.Add(new Tile("14"));
                possible_pool.Add(new Tile("15"));
                possible_pool.Add(new Tile("16"));
                possible_pool.Add(new Tile("17"));
                possible_pool.Add(new Tile("18"));
                possible_pool.Add(new Tile("20"));
                possible_pool.Add(new Tile("21"));
                possible_pool.Add(new Tile("22"));
                possible_pool.Add(new Tile("23"));
                possible_pool.Add(new Tile("24"));
                possible_pool.Add(new Tile("25"));
                possible_pool.Add(new Tile("26"));
                possible_pool.Add(new Tile("27"));
                possible_pool.Add(new Tile("28"));
                possible_pool.Add(new Tile("30"));
                possible_pool.Add(new Tile("31"));
                possible_pool.Add(new Tile("32"));
                possible_pool.Add(new Tile("33"));
                possible_pool.Add(new Tile("34"));
                possible_pool.Add(new Tile("35"));
                possible_pool.Add(new Tile("36"));
            }
            possible_pool.Add(new Tile("40"));
            possible_pool.Add(new Tile("41"));
            possible_pool.Add(new Tile("42"));
            possible_pool.Add(new Tile("43"));
            possible_pool.Add(new Tile("44"));
            possible_pool.Add(new Tile("45"));
            possible_pool.Add(new Tile("46"));
            possible_pool.Add(new Tile("47"));

            foreach (Tile tile in discarded_pool)
            {
                possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
            }
            foreach (Tile tile in ownPlayer.tiles_hand)
            {
                possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
            }
            foreach (Tile tile in ownPlayer.tiles_flower)
            {
                possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
            }
            foreach (List<Tile> tiles in ownPlayer.tiles_displayed)
            {
                foreach (Tile tile in tiles)
                {
                    possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
                }
            }
            foreach (Tile tile in nextPlayer.tiles_flower)
            {
                possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
            }
            foreach (List<Tile> tiles in nextPlayer.tiles_displayed)
            {
                foreach (Tile tile in tiles)
                {
                    possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
                }
            }
            foreach (Tile tile in oppositePlayer.tiles_flower)
            {
                possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
            }
            foreach (List<Tile> tiles in oppositePlayer.tiles_displayed)
            {
                foreach (Tile tile in tiles)
                {
                    possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
                }
            }
            foreach (Tile tile in lastPlayer.tiles_flower)
            {
                possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
            }
            foreach (List<Tile> tiles in lastPlayer.tiles_displayed)
            {
                foreach (Tile tile in tiles)
                {
                    possible_pool.Remove(possible_pool.Find(eachTile => eachTile.tile_integer == tile.tile_integer));
                }
            }
        }
    }

    public class OneGame
    {
        public int wind;
        public int dealer;
        public List<Tile> remaining_pool;
        public List<Tile> discarded_pool;
        public List<Player> players;
        public State state;

        public OneGame(int _wind, int _dealer)
        {
            wind = _wind;
            dealer = _dealer;
            remaining_pool = new List<Tile>();
            discarded_pool = new List<Tile>();
            players = new List<Player>
            {
                new Player(0),
                new Player(1),
                new Player(2),
                new Player(3)
            };
            state = new State
            {
                ownPlayer = players[0],
                remaining_pool = remaining_pool
            };

            // Create random initial pool of tiles
            for (int i = 0; i < 4; i++)
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

            // Assign tiles to players
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    while (remaining_pool[0].tile_pattern == 4)
                    {
                        players[i].tiles_flower.Add(remaining_pool[0]);
                        remaining_pool.RemoveAt(0);
                    }
                    players[i].tiles_hand.Add(remaining_pool[0]);
                    remaining_pool.RemoveAt(0);
                }
            }
            while (remaining_pool[0].tile_pattern == 4)
            {
                players[dealer].tiles_flower.Add(remaining_pool[0]);
                remaining_pool.RemoveAt(0);
            }
            players[dealer].tiles_hand.Add(remaining_pool[0]);
            remaining_pool.RemoveAt(0);
        }

        public int GetWind()
        {
            return wind;
        }
    }

    public static class Util
    {
        private static readonly System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
            }
        }
    }
}

