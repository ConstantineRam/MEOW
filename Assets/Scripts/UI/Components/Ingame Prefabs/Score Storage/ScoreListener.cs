using UnityEngine.UI;
using UnityEngine;

public class ScoreListener : MonoBehaviour
{

  private Text ScoreText;

	
	void Awake ()
  {

    this.ScoreText = this.GetComponent<Text>();
    this.ScoreText.text = "0";
    Game.Events.ReportScore.Listen(UpdateScore);
  }

  public void UpdateScore(int NewScore)
  {
    Game.ActionRoot.SetCurrenScore(Game.ActionRoot.GetCurrenScore() + NewScore);
    this.ScoreText.text = Game.ActionRoot.GetCurrenScore().ToString();
  }

}
