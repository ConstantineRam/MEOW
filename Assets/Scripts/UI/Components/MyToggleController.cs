using DG.Tweening;
//using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyToggleController : ExtendedBehaviour, IPointerUpHandler
{

  [Serializable]
  public class ToggleClickEvent : UnityEvent<bool> { }


  [SerializeField]
  private ToggleClickEvent OnValueChanged;

  [SerializeField]
  private Image onBackImage;
  [SerializeField]
  private Transform toggleImage;
  [SerializeField]
  private Text text;

  [SerializeField]
  private TogglePositions togglePos;
  [SerializeField]
  private TogglePositions textPos;

  private bool isOn;

  private void Awake()
  {
    UpdateState(true);
    Game.Events.LanguageChanged.Listen(kind => UpdateState(true));
  }

  public void SetState(bool state, bool insta = true)
  {
    isOn = state;
    UpdateState(insta);
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    isOn = !isOn;
    OnValueChanged.Invoke(isOn);
    UpdateState();
  }

  private void UpdateState(bool insta = false)
  {
    onBackImage.DOKill();
    onBackImage.DOFade(isOn ? 1 : 0, insta ? 0 : 0.25f);

    toggleImage.DOKill();
    toggleImage.DOLocalMoveX(isOn ? togglePos.right : togglePos.left, insta ? 0 : 0.25f).SetEase(Ease.InOutExpo);
    if (!insta)
    {
      toggleImage.DOScale(new Vector3(1.1f, 0.9f), 0.07f).OnComplete(() =>
      {
        toggleImage.DOScale(1, 0.45f).SetEase(Ease.OutBack);
      });
    }


 //   string toggleText = isOn ? ScriptLocalization.MENU.On : ScriptLocalization.MENU.Off;
 //   text.DOKill();
 //   text.transform.DOLocalMoveX(isOn ? textPos.left : textPos.right, insta ? 0 : 0.2f).SetEase(Ease.InOutExpo).SetDelay(insta ? 0 : 0.15f).OnStart(() => { text.text = toggleText.ToUpper(); });
  }

  [Serializable]
  private struct TogglePositions
  {
    public float left;
    public float right;
  }
}
