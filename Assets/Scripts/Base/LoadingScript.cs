//using I2.Loc;
using System.Collections;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
  [SerializeField] private Game gameRef;

  [Space]
  [SerializeField] private float gameLogoDelay;

  private void Awake()
  {
    StartCoroutine(WaitAndInitGame());
  }

  private IEnumerator WaitAndInitGame()
  {
    yield return new WaitForSeconds(gameLogoDelay);

    Instantiate(gameRef);
  }
}
