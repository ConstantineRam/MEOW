using System;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Utils.ExtensionMethods
{
  public static class DescriptionAttributeEx
  {
    public static string GetDesc<T>(this T id)
    {
      MemberInfo memberInfo = typeof(T).GetMember(id.ToString()).FirstOrDefault();

      if (memberInfo != null)
      {
        DescriptionAttribute attribute = (DescriptionAttribute) memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
        return attribute.Key;
      }

      return String.Empty;
    }

  }


}