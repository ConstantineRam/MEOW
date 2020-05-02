namespace Assets.Scripts.Utils.ExtensionMethods
{
  public static class AudioIdEx
  {
    public static string GetPath(this AudioId id)
    {
      return id.GetDesc();
    }

    public static void PlaySound(this AudioId id)
    {
      Game.AudioManager.PlaySound(id);
    }

    public static void PlayMusic(this AudioId id)
    {
      Game.AudioManager.PlayMusic(id);
    }
  }
}
