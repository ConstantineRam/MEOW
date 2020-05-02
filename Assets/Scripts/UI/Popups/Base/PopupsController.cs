using Assets.Scripts.Utils.ExtensionMethods;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupsController : MonoBehaviour
{

  [SerializeField]
  private Transform container;
  [SerializeField]
  private Animator animator;
 // [SerializeField]
  //private Image fadeGlass;
  [SerializeField]
  private Image defaultDecor;

  [SerializeField]
  private Sprite FadeRemoval;

  private bool isAnimEnded;

  private GenericPopup currentPopup;

  public Transform Container { get { return container; } }
  private float fadeAlpha;
  private Material FadeMaterial;

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
   // FadeMaterial = new Material(fadeGlass.material);
    
   // Color fadeColor = fadeGlass.color;
  //  fadeAlpha = fadeColor.a;
   // fadeGlass.gameObject.SetActive(false);


  }

  //---------------------------------------------------------------------------------------------------------------
  private void RemoveFade()
  {
  //  fadeGlass.material = null;
  //  fadeGlass.sprite = this.FadeRemoval;
  //  fadeGlass.gameObject.SetActive(false);
  }

    //---------------------------------------------------------------------------------------------------------------
    private void ActivateFade()
  {
 //   fadeGlass.gameObject.SetActive(true);
//    fadeGlass.material = FadeMaterial;
//    fadeGlass.sprite = null;
  }

  //---------------------------------------------------------------------------------------------------------------
  public IEnumerator Show(string popupName, object data)
  {
    LoadAndActivate(popupName, data);
    yield return AnimShow();
  }

  //---------------------------------------------------------------------------------------------------------------
  public IEnumerator ShowPanel(string popupName, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    LoadAndActivate(popupName, data, CallBack);

    yield return AnimShowPanel();
  }

  //---------------------------------------------------------------------------------------------------------------
  public IEnumerator Show(PopupId popup, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    LoadAndActivate(popup, data, CallBack);
    yield return AnimShow();
  }

  //---------------------------------------------------------------------------------------------------------------
  public IEnumerator ShowPanel(PopupId popup, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    LoadAndActivate(popup, data, CallBack);

    yield return AnimShowPanel();
  }

  //---------------------------------------------------------------------------------------------------------------
  public IEnumerator Hide()
  {
    if (container.childCount <= 0) yield break;
    yield return AnimHide();

    //GenericPopup RemovedPopUp = container.GetComponentInChildren<GenericPopup>();
    //if (RemovedPopUp == null)
    //{
    //  Debug.LogError("Attempt to Hide Popup panel that doesn't already exist.");
    //}
    //else
    //{
    //  RemovedPopUp.OnClose();
    //}

    container.ClearChilds();
    yield return null;
  }



  //---------------------------------------------------------------------------------------------------------------
  public IEnumerator HidePanel()
  {
    if (container.childCount <= 0) yield break;

    GenericPopup RemovedPopUp = container.GetComponentInChildren<GenericPopup>();
    if (RemovedPopUp == null)
    {
      Debug.LogError("Attempt to Hide Popup panel that doesn't already exist.");
    }
    else
    {
      RemovedPopUp.OnClose();
    }

    yield return AnimHidePanel();



    container.ClearChilds();


    yield return null;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void CloseAll()
  {
    container.ClearChilds();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnFadeClick()
  {
    if (currentPopup.CanBeClosedByClickOnFade)
    {
      currentPopup.CloseSelf();
    }
  }


  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator AnimShow()
  {
    yield return SetAnimState("Show");
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator AnimHide()
  {
    yield return SetAnimState("Hide");
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator AnimShowPanel()
  {
    yield return SetAnimState("ShowPanel");
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator AnimHidePanel()
  {
    yield return SetAnimState("HidePanel");
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator SetAnimState(string state)
  {
    animator.SetTrigger("Idle");
    yield return null;
    animator.SetTrigger(state);
    isAnimEnded = false;

    yield return new WaitUntil(() => isAnimEnded);
  }


  //---------------------------------------------------------------------------------------------------------------
  // Called from animator.
  public void OnAnimationComplete()
  {
    isAnimEnded = true;
  }
  //---------------------------------------------------------------------------------------------------------------
  private bool LoadAndActivateRoutines(String path, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    if (path == String.Empty)
    {
      Debug.LogError("path == String.Empty");
    }

    GameObject prefab = Resources.Load(path) as GameObject;
    if (prefab == null)
    {
      Debug.Log("Attempt to Instantiate popup with path " + path + ", but Resources.Load returned null.");
      return false;
    }
    currentPopup = Instantiate(prefab, container).GetComponent<GenericPopup>();
    currentPopup.CallBack = CallBack;
    currentPopup.Activate(data);

    defaultDecor.gameObject.SetActive(!currentPopup.HasCustomDecor);

    if (currentPopup.DisableBlur)
    {
      return false;
    }
    else
    {
      return true;
    }

  }

  //---------------------------------------------------------------------------------------------------------------
  private bool LoadAndActivate(String PopUpName, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    string path = "Prefabs/UI/Popups/" + PopUpName;
    return this.LoadAndActivateRoutines(path, data, CallBack);
  }

  //---------------------------------------------------------------------------------------------------------------
  private bool LoadAndActivate(PopupId id, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    string path = id.GetDesc();
    return this.LoadAndActivateRoutines(path, data, CallBack);

  }
}
