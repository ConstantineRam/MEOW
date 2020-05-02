using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class EditorPrefsValueAttribute : Attribute
{
  public object DefaultValue { get; private set; }
  public string Key { get; private set; }

  public EditorPrefsValueAttribute(string key, object defaultValue = null)
  {
    Key = key;
    DefaultValue = defaultValue;
  }
}
