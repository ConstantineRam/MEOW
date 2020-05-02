using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils.ExtensionMethods
{
  public static class ArrayEx
  {
    public static bool HasElement<T>(this T[] array, int i)
    {
      return i >= 0 && i < array.Length;
    }

    public static bool HasElement<T>(this List<T> list, int i)
    {
      return i >= 0 && i < list.Count;
    }
  }
}
