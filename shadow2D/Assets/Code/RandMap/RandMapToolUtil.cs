
using UnityEngine;
using System.Collections.Generic;

namespace RandMap
{
    public class RandMapToolUtil 
    {
        public static Dictionary<string, double> TimeLogMap = new Dictionary<string, double>();
        public static void TimeStartLog(string str) {
            TimeLogMap.Remove(str);
            TimeLogMap.Add(str,Time.time);
        }
        public static void TimeEndLog(string str)
        {
            if (TimeLogMap.ContainsKey(str) == false)
                return;

            double startTime = TimeLogMap.GetValueOrDefault(str,0);
            double now = Time.time;

            Debug.Log(str + " Time:" + (now - startTime));
        }


    }
}