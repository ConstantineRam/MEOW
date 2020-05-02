using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundCollectionBlock {

  private List<AudioClip> clips;


  public SoundCollectionBlock()
  {
    clips = new List<AudioClip>();
  }

  public void PushClip(AudioClip newClip)
  {
 
    clips.Add(newClip);
  }

  public bool HasClips()
  {
    if (clips.Count == 0)
    {
      return false;
    }
    return true;
  }

  public bool HasClip(AudioClip clipToFind)
  {
    if (clips.Contains(clipToFind))
     {
      return true;
     }

    return false;
  }

  public AudioClip GetClip(AudioClip forbiddenClip = null)
  {
    if (!HasClips())
    {
      Debug.Log(message: "<color=red>ERROR:</color> GetClip for clip collection was called, but collection has no sounds. Return NULL.");
      return null;
    }

    if (clips.Count == 1)
    {
      return clips.ElementAt(0);
    }
    AudioClip ResultClip = null;
    while (true)
    {
      var rnd = new System.Random(DateTime.Now.Millisecond);
      int randomSoundNum = rnd.Next(0, clips.Count);
      ResultClip = clips.ElementAt(randomSoundNum);
      if (forbiddenClip == null)
      {
        return ResultClip;
      }

      if (forbiddenClip != ResultClip)
      {
        return ResultClip;
      }

    }
  }

}
