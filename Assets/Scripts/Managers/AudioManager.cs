using Assets.Scripts.Utils.ExtensionMethods;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
  private TimerManager.Timer timer;

  private Dictionary<AudioId, SoundCollectionBlock> clipsCollection;

  private List<AudioSource> activeMusicSources;
  private List<AudioSource> activeSoundSources;

  public const AudioId DEFAULT_AUDIO_CLIP_ID = AudioId.ClickSound;
  private AudioClip LastMusicClip = null;

  private void Awake()
  {
    activeMusicSources = new List<AudioSource>();
    activeSoundSources = new List<AudioSource>();
    CacheClipsCollection();

    Game.Events.MusicEnabled.Listen(OnMusicEnabledChanged);
    Game.Events.SoundEnabled.Listen(OnSoundEnabledChanged);
  }

  private void Update()
  {
    RemoveExtraMusic();
  }

  private void LateUpdate()
  {
    CheckAndDestroyFinishedSources();

    if (this.HasActiveMusic())
    {
      return;
    }

  }

  private void RemoveExtraMusic()
  {
    int counterSound = 0;
    foreach (AudioSource source in activeMusicSources)
    {
      if (source.isPlaying)
      {
        counterSound++;
        if (counterSound > 1)
        {
          source.Stop();
        }
      }

    }
  }
  private bool HasActiveMusic()
  {

    foreach (AudioSource source in activeMusicSources)
    {
      if (source.isPlaying)
      {
        return true;
      }
            
    }
    return false;
  }

  #region Play, Stop
  public AudioSource PlayMusic(AudioId audioId, bool loop = true, float volume = 1, bool overrideOthers = true, float overrideTime = 0)
  {
    if (IsPlaying(audioId))
    {
      return GetActiveSourcesById(audioId).FirstOrDefault();
    }
      

    SoundCollectionBlock soundCollection;
    clipsCollection.TryGetValue(audioId, out soundCollection);
    if (soundCollection == null)
    {
      Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color> PlayMusic was called for ID:" + audioId.ToString() + ", but no collection with this ID exists. Ignored. Check the enum.");
      return null;
    }

    AudioClip clip;

    clip = soundCollection.GetClip(LastMusicClip);
    LastMusicClip = clip;
    if (clip == null)
    {
      Debug.Log(message: "<color=red>ERROR:</color> PlayMusic was called for ID:" + audioId.ToString() + ", but collection has no sounds and returns null. Ignored.");
      return null;
    }

    AudioSource source = gameObject.AddComponent<AudioSource>();
    source.volume = volume;
    source.loop = loop;
    source.clip = clip;
    source.mute = !Game.Settings.MusicEnabled;


    source.Play();

    if (overrideOthers)
    {
      foreach (AudioSource s in activeMusicSources)
      {
        StopAudio(s, overrideTime);
   
      }
      activeMusicSources.Clear();
    } 

    //Game.TimerManager.Start(overrideTime+0.1f, () =>
    //{
      activeMusicSources.Add(source);
 //   });
    
    
    return source;
  }

  public AudioSource PlaySound(AudioId audioId, float volume = 1)
  {
    SoundCollectionBlock soundCollection;
    clipsCollection.TryGetValue(audioId, out soundCollection);
    if (soundCollection == null)
    {
      Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color> PlaySound was called for ID:" + audioId.ToString() + ", but no collection with this ID exists. Ignored. Check the enum.");
      return null;
    }

    AudioClip clip;
    clip = soundCollection.GetClip();
    if (clip == null)
    {
      Debug.Log(message: "<color=red>ERROR:</color> PlaySound was called for ID:" + audioId.ToString() + ", but collection has no sounds and returns null. Ignored.");
      return null;
    }

    AudioSource source = gameObject.AddComponent<AudioSource>();
    source.mute = !Game.Settings.SoundEnabled;
    source.clip = clip;

    source.Play();

    activeSoundSources.Add(source);

    return source;
  }

  public void StopAudio(AudioSource src, float duration = 0)
  {
    StopAudio(ClipToId(src.clip), duration);
  }
  public void StopAudio(AudioId id, float duration = 0f)
  {
    GetActiveSourcesById(id).ForEach(s => s.DOFade(0, duration).OnComplete(s.Stop));
  }

  public void StopAllSounds(float duration = 0f)
  {
    foreach (AudioSource source in activeSoundSources)
    {
      source.DOFade(0, duration).OnComplete(source.Stop);
    }   
  }

  public void StopAllMusic(float duration = 0f)
  {
    foreach (AudioSource source in activeMusicSources)
    {
      source.DOFade(0, duration).OnComplete(source.Stop);
    }
  }

  public void StopEverything(float duration = 0f)
  {
    StopAllMusic(duration);
    StopAllSounds(duration);
  }

  #endregion

  #region Subscription events
  private void OnMusicEnabledChanged(bool isEnabled)
  {
    foreach (AudioSource source in activeMusicSources)
    {
      source.DOKill();
      if (isEnabled) source.mute = !isEnabled;
      source.DOFade(isEnabled.To01(), 0.5f).OnComplete(() =>
      {
        source.mute = !isEnabled;
      });
    }
  }
  private void OnSoundEnabledChanged(bool isEnabled)
  {
    foreach (AudioSource source in activeSoundSources)
    {
      source.DOKill();
      if (isEnabled) source.mute = !isEnabled;
      source.DOFade(isEnabled.To01(), 0.5f).OnComplete(() =>
      {
        source.mute = !isEnabled;
      });
    }
  }
  #endregion

  #region Utils
  private void CacheClipsCollection()
  {
 
    clipsCollection = new Dictionary<AudioId, SoundCollectionBlock>();

    foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
    {
      if (clipsCollection.ContainsKey(id))
      {
        Debug.Log(message: "<color=red>ERROR:</color> attempt to add AudioId with the same ID:" +id.ToString()+". Ignored. Check the enum.");
        continue;
      }

      SoundCollectionBlock soundCollectionBlock = new SoundCollectionBlock();
  
      string directoryPath = id.GetPath();
      
      var loadedClips = Resources.LoadAll(directoryPath, typeof(AudioClip)).Cast<AudioClip>().ToArray();
      Debug.Log( id.ToString() + " "+ loadedClips.Length);
      foreach (var singleClip in loadedClips)
       { 
        soundCollectionBlock.PushClip(singleClip);
       }

      clipsCollection.Add(id, soundCollectionBlock);

    } //foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
  }

  private AudioId ClipToId(AudioClip clip)
  {
    if (clip == null)
    {
      Debug.Log(message: "<color=red>ERROR:</color>, <color=white>ClipToId</color> got null in <color=white>clip</color>. Return default value: " + DEFAULT_AUDIO_CLIP_ID + ".");
      return DEFAULT_AUDIO_CLIP_ID;
    }
    SoundCollectionBlock soundCollectionBlock;
    foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
    {
      clipsCollection.TryGetValue(id, out soundCollectionBlock);
      if (soundCollectionBlock == null)
       {
        Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color>, <color=white>ClipToId</color> can't retrive a value from dictionary <color=white>clipsCollection</color> for ID:" + id.ToString()+". Ignored.");
        continue;
       }
      if (soundCollectionBlock.HasClip(clip))
       {
        return id;
       }
    } // foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
    Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color>, <color=white>ClipToId</color> can't clip " + clip.name + " in game collection. Return default value: "+ DEFAULT_AUDIO_CLIP_ID+".");
    return DEFAULT_AUDIO_CLIP_ID;
  }

  private bool IsPlaying(AudioId id)
  {
    return !GetActiveSourcesById(id).IsEmpty();
  }

  private List<AudioSource> GetActiveSourcesById(AudioId id)
  {
    List<AudioSource> result = new List<AudioSource>();
    // Music
    result.AddRange(activeMusicSources.Where(a => ClipToId(a.clip) == id));
    // Sound
    result.AddRange(activeSoundSources.Where(a => ClipToId(a.clip) == id));
    return result;
  }

  private void CheckAndDestroyFinishedSources()
  {
    // Music
    List<AudioSource> toRemove = new List<AudioSource>();
    foreach (AudioSource source in activeMusicSources)
    {
      if (source.isPlaying) continue;
      toRemove.Add(source);
    }
    toRemove.ForEach(s => { activeMusicSources.Remove(s); Destroy(s); });

    // Sound
    toRemove.Clear();
    foreach (AudioSource source in activeSoundSources)
    {
      if (source.isPlaying) continue;
      toRemove.Add(source);
    }
    toRemove.ForEach(s => { activeSoundSources.Remove(s); Destroy(s); });
  }
  #endregion

}
