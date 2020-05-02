using System;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
public class RootTypeAttribute : Attribute
{
  public RootBase root { get; private set; }

  public RootTypeAttribute(RootBase root)
  {
    this.root = root;
  }
}
