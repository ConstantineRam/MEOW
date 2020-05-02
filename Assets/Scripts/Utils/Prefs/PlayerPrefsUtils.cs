using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PlayerPrefsUtils
{
  public static string GetKey(object target, string attributeKey)
  {
    var name = target.GetType().FullName;
    return name + ":" + attributeKey;
  }

  public static void Fill(object target)
  {
    foreach (var fieldInfo in target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
    {
      var attribute = (PlayerPrefsValueAttribute) fieldInfo.GetCustomAttributes(typeof(PlayerPrefsValueAttribute), false).FirstOrDefault();
      if (attribute == null)
        continue;

      object value = null;
      var key = GetKey(target, attribute.Key);
      if (fieldInfo.FieldType == typeof(float)) value = PlayerPrefs.GetFloat(key, (float) attribute.DefaultValue);
      else if (fieldInfo.FieldType == typeof(string)) value = PlayerPrefs.GetString(key, (String) attribute.DefaultValue);
      else if (fieldInfo.FieldType == typeof(int)) value = PlayerPrefs.GetInt(key, (Int32) attribute.DefaultValue);
      else if (fieldInfo.FieldType == typeof(bool)) value = PlayerPrefsGetBool(key, (Boolean) attribute.DefaultValue);

      fieldInfo.SetValue(target, value);
    }
  }

  public static void Save(object target)
  {
    foreach (var propertyInfo in target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
    {
      var attribute = (PlayerPrefsValueAttribute) propertyInfo.GetCustomAttributes(typeof(PlayerPrefsValueAttribute), false).FirstOrDefault();

      if (attribute == null)
        continue;
      var key = GetKey(target, attribute.Key);
      if (propertyInfo.FieldType == typeof(float)) PlayerPrefs.SetFloat(key, (Single) propertyInfo.GetValue(target));
      else if (propertyInfo.FieldType == typeof(string)) PlayerPrefs.SetString(key, (String) propertyInfo.GetValue(target));
      else if (propertyInfo.FieldType == typeof(int)) PlayerPrefs.SetInt(key, (Int32) propertyInfo.GetValue(target));
      else if (propertyInfo.FieldType == typeof(bool)) PlayerPrefs.SetInt(key, (Boolean) propertyInfo.GetValue(target) ? 1 : 0);
    }
    PlayerPrefs.Save();
  }

  public static void Reset(object target)
  {
    PlayerPrefs.DeleteAll();
  }

  private static bool PlayerPrefsGetBool(string key, Boolean defaultValue)
  {
    if (!PlayerPrefs.HasKey(key))
      return defaultValue;

    return PlayerPrefs.GetInt(key) != 0;
  }
}
