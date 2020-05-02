namespace Assets.Scripts.Utils.ExtensionMethods
{
  public static class BoolEx
  {
    public static int To01(this bool value)
    {
      return value ? 1 : 0;
    }
  }
}
