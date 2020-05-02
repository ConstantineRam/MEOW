using UnityEngine.UI;
using UnityEngine;

public class WinScreen_BestScore : MonoBehaviour {


  private Text ScoreText;


  void Awake()
  {
    this.ScoreText = this.GetComponent<Text>();
    this.ScoreText.text = Game.Settings.BestScore.ToString();
  }


}
