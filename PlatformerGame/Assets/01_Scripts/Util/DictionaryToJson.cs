using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class DictionaryToJson
{
    static StringBuilder sb = new StringBuilder();
    
    public static string Convert(Dictionary<string, object> dictionary)
    {
        sb.Clear();

        sb.Append("{");
        foreach (var element in dictionary)
        {
            sb.AppendFormat("\"{0}\":\"{1}\",", element.Key, element.Value);
        }

        sb.Remove(sb.Length - 1, 1);
        sb.Append("}");

        return sb.ToString();
    }
}
