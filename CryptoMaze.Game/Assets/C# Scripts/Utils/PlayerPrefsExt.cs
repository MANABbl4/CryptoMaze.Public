using System;
using UnityEngine;

public static class PlayerPrefsExt
{
    public static DateTime? GetDateTime(string key)
    {
        var stringValue = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(stringValue))
        {
            return null;
        }

        if (!long.TryParse(stringValue, out var value))
        {
            return null;
        }

        return DateTime.FromBinary(value);
    }

    public static void SetDateTime(string key, DateTime value)
    {
        PlayerPrefs.SetString(key, value.ToBinary().ToString());
    }
}
