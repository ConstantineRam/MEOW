using System;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
public class DescriptionAttribute : Attribute
{
  public string Key { get; private set; }

  public DescriptionAttribute(string key)
  {
    Key = key;
  }
}
