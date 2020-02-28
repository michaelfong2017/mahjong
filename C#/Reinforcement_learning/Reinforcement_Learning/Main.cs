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
            Main.DebugLog(tile0);
            Main.DebugLog(tile0.tile_pattern);
        }

        public static void DebugLog(object message)
        {
            Console.WriteLine(message);
        }
    }
}

