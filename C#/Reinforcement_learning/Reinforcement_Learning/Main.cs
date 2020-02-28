using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Reinforcement_Learning
{
    public class Main
    {
        public static void Start()
        {
            Reinforcement_Learning.Tile tile0 = new Reinforcement_Learning.Tile("04");
            Console.WriteLine(tile0);
            Console.WriteLine(tile0.tile_pattern);
        }
    }
}

