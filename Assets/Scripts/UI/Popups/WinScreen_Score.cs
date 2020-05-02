using UnityEngine.UI;
using UnityEngine;

public class WinScreen_Score : MonoBehaviour
{

  private Text ScoreText;


  void Awake()
  {
    this.ScoreText = this.GetComponent<Text>();
    this.ScoreText.text = Game.ActionRoot.GetCurrenScore().ToString();
  }


}
