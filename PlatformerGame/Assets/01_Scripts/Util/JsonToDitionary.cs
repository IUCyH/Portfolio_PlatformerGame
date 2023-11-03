using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonToDitionary
{
    static Dictionary<string, string> dictionary = new Dictionary<string, string>();
    static char[] charactersToIgnore = new[] { '"', '{', '}' };
    
    public static Dictionary<string, string> Convert(string json)
    {
        var split = json.Split(",");
        dictionary.Clear();
        
        foreach (var data in split)
        {
            var temp = data.Split(":");
            var key = temp[0].Trim(charactersToIgnore);
            var value = temp[1].Trim(charactersToIgnore);
            
            dictionary.Add(key, value);
        }

        return dictionary;
    }
}
