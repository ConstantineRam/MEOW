using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

public class EditorPrefsUtils
{
  public static void Fill(object target)
  {
    var name = target.GetType().FullName;
    foreach (var propertyInfo in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
      var attribute = (EditorPrefsValueAttribute) propertyInfo.GetCustomAttributes(typeof(EditorPrefsValueAttribute), false).FirstOrDefault();
      if (attribute == null)
        continue;

      object value = null;
      if (propertyInfo.PropertyType == typeof(float)) value = EditorPrefs.GetFloat(name + ":" + attribute.Key, (Single) attribute.DefaultValue);
      else if (propertyInfo.PropertyType == typeof(string)) value = EditorPrefs.GetString(name + ":" + attribute.Key, (String) attribute.DefaultValue);
      else if (propertyInfo.PropertyType == typeof(int)) value = EditorPrefs.GetInt(name + ":" + attribute.Key, (Int32) attribute.DefaultValue);
      else if (propertyInfo.PropertyType == typeof(bool)) value = PlayerPrefsGetBool(name + ":" + attribute.Key, (Boolean) attribute.DefaultValue);

      propertyInfo.SetValue(target, value, null);
    }
  }

  public static void Save(object target)
  {
    var name = target.GetType().FullName;
    foreach (var propertyInfo in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
      var attribute = (EditorPrefsValueAttribute) propertyInfo.GetCustomAttributes(typeof(EditorPrefsValueAttribute), false).FirstOrDefault();

      if (attribute == null)
        continue;

      if (propertyInfo.PropertyType == typeof(float)) EditorPrefs.SetFloat(name + ":" + attribute.Key, (Single) propertyInfo.GetValue(target, null));
      else if (propertyInfo.PropertyType == typeof(string)) EditorPrefs.SetString(name + ":" + attribute.Key, (String) propertyInfo.GetValue(target, null));
      else if (propertyInfo.PropertyType == typeof(int)) EditorPrefs.SetInt(name + ":" + attribute.Key, (Int32) propertyInfo.GetValue(target, null));
      else if (propertyInfo.PropertyType == typeof(bool)) EditorPrefs.SetInt(name + ":" + attribute.Key, (Boolean) propertyInfo.GetValue(target, null) ? 1 : 0);
    }
  }

  private static bool PlayerPrefsGetBool(string key, Boolean defaultValue)
  {
    if (!EditorPrefs.HasKey(key))
      return defaultValue;

    return EditorPrefs.GetInt(key) != 0;
  }
}
