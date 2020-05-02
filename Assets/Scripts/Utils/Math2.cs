using UnityEngine;

public class Math2 : MonoBehaviour
{

  public static float Random01()
  {
    return Random.Range(0f, 1f);
  }

  public static bool RandomBool()
  {
    return Random.Range(0f, 1f) > 0.5f;
  }

  public static float Remap01(float value, float min, float max)
  {
    return (value - min) / (max - min);
  }

  public static float Remap(float value, float from1, float from2, float to1, float to2)
  {
    return (value - from1) / (from2 - from1) * (to2 - to1) + to1;
  }
}
