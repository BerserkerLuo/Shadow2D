using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StoryMgr : SingletonBase<StoryMgr>
{
    Dictionary<string, StoryInfo> storyMap = new Dictionary<string, StoryInfo>();

    public StoryInfo GetStoryInfo(string storyName) {
        try{
            StoryInfo storyInfo = storyMap.GetValueOrDefault(storyName, default);
            if (storyInfo != null)
                return storyInfo;

            string json = File.ReadAllText($"{Application.streamingAssetsPath}/{"Story"}/{storyName}.json");
            storyInfo = JsonUtility.FromJson<StoryInfo>(json);
            storyMap.Add(storyName, storyInfo);
            return storyInfo;
        }
        catch(Exception) {
            return null;
        }
    }

    public StoryInfo GetDefaultStory() {
        return GetStoryInfo("Story1");
    }
}