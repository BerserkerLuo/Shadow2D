using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryEditor
{
    public class Global
    {
        private static bool _dirty = false; 

        public static bool Dirty { 
            get { return _dirty; }
            set {
                _dirty = value; 
            }
        }

        public static string FilePath = "";

        public static List<EGotoType> GotoOptions = new List<EGotoType> { EGotoType.Random, EGotoType.Options, EGotoType.RandomRole };
        public static List<ECompType> ComponentType = new List<ECompType> { ECompType.Battle, ECompType.Reward, ECompType.EventUnlock,ECompType.EndResult,ECompType.EventEnd };
        public static List<int> EventList = new List<int>(); 
    }
}