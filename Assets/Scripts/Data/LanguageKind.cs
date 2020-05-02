using System;
using System.Linq;
using System.Reflection;

public enum LanguageKind
{
  [ShortLanguage("ENG")]
  English,
  [ShortLanguage("中文")]
  Chinese,
  [ShortLanguage("日本語")]
  Japanese,
  [ShortLanguage("РУC")]
  Russian,
  [ShortLanguage("ESP")]
  Spanish,
  [ShortLanguage("POR")]
  Portuguese,
  [ShortLanguage("العربية")]
  Arabic



  //Ukrainian,
  //Byelorussian,
  //French,
  //German,

  //Vietnamese,
  //Italian,
  //Korean,
  //Turkish,
  //Swedish,
  //Norwegian,
  //Portuguese,
  //Filipino,
  //Persian,
  //Hindi,
  //Hebrew
}

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
public class ShortLanguageAttribute : Attribute
{
  public string Key { get; private set; }

  public ShortLanguageAttribute(string key)
  {
    Key = key;
  }
}

public static class ShortLanguageAttributeEx
{
  public static string ToShort<T>(this T id)
  {
    MemberInfo memberInfo = typeof(T).GetMember(id.ToString()).FirstOrDefault();

    if (memberInfo != null)
    {
      ShortLanguageAttribute attribute = (ShortLanguageAttribute) memberInfo.GetCustomAttributes(typeof(ShortLanguageAttribute), false).FirstOrDefault();
      return attribute.Key;
    }

    return String.Empty;
  }
}
