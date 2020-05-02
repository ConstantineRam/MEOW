using System;
using Assets.Scripts.Utils.ExtensionMethods;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{

  private enum Mode
  {
    OnClick,
    OnDown,
    OnUp,
  }


  const AudioId defaultAudioClip = AudioId.ClickSound;
  [SerializeField]
  private Mode mode = Mode.OnUp;

  //[SerializeField]
  //private AudioId clip = AudioId.ClickSound;
  [Header("Clips (Plays random if > 1 or Default sound if == 0)")]
  [SerializeField] private AudioId[] clip = { defaultAudioClip };


  public void OnPointerClick(PointerEventData eventData)
  {
    if (mode != Mode.OnClick) return;
    PlayRandomClip();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (mode != Mode.OnDown) return;
    PlayRandomClip();
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (mode != Mode.OnUp) return;
    PlayRandomClip();
  }

  private void PlayRandomClip()
  {
    if (clip.Length == 0)
      {
       defaultAudioClip.PlaySound();
      return;
      }

    var rnd = new System.Random(DateTime.Now.Millisecond);
    int randomSoundNum = rnd.Next(0, clip.Length);
    clip[randomSoundNum].PlaySound();
   }
}

