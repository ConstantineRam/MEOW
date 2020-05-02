using UnityEngine;

public class ExtendedBehaviour : MonoBehaviour
{
  public void print(object a, object b)
  {
    print(a + " : " + b);
  }
  public void print(object a, object b, object c)
  {
    print(a + " : " + b + " : " + c);
  }
}
