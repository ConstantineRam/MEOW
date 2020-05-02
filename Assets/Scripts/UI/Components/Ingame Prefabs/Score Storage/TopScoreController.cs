using UnityEngine.UI;
using UnityEngine;

public class TopScoreController : MonoBehaviour {
  [Tooltip("What should be shown in Best Score field if it == 0.")]
  [SerializeField]
  private string DefaultZeroText = "0";
  private Text ScoreText;


  void Awake()
  {

    this.ScoreText = this.GetComponent<Text>();
    if (Game.Settings.BestScore == 0)
    {
      this.ScoreText.text = this.DefaultZeroText;
      return;
    }

    this.ScoreText.text = Game.Settings.BestScore.ToString();
  }


}
