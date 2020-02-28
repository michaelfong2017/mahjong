using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}

