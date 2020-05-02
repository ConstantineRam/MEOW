using Assets.Scripts.Utils.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public Signal<PopupId> OnPopupOpen = new Signal<PopupId>();
  
  [SerializeField]
  private PopupsController popupsController;
  private Queue<IEnumerator> queue;
  private PopupId? currentPopup;

  private Coroutine QueueProcessor;

  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    queue = new Queue<IEnumerator>();
    this.StartQueueProcessor();  
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StartQueueProcessor()
  {
    StopAllCoroutines();
   

    QueueProcessor = StartCoroutine(ProceedQueue());
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator ProceedQueue()
  {
    while (true)
    {
      if (!queue.IsEmpty())
      {
        yield return queue.Dequeue();
      }
      yield return null;
    }
  }




  #region public Methods
  //---------------------------------------------------------------------------------------------------------------
  public void Open(PopupId popup, object data = null, PopupId CallBack = PopupId.NoPopUp)
  {
    if (popup == PopupId.NoPopUp)
    {
      Debug.LogError("NoPopUp was called.");
      return;
    }

    Debug.Log("Open " + popup + " popup was called.");
    queue.Enqueue(ShowPopupCoroutine(popup, data, CallBack));

    OnPopupOpen.Invoke(popup);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Hide()
  {
    queue.Enqueue(HidePopupCoroutine());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void CloseAll()
  {
    if (popupsController != null)
    {
      popupsController.CloseAll();
      popupsController.gameObject.SetActive(false);
    }
    currentPopup = null;
  }

  #endregion



  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator ShowPopupCoroutine(PopupId popup, object data, PopupId CallBack = PopupId.NoPopUp)
  {
    Debug.Log("ShowPopupCoroutine started.");
    popupsController.gameObject.SetActive(true);

    if (currentPopup != null)
    {
      Debug.Log("Closing old popup.");
      yield return popupsController.HidePanel();
      yield return popupsController.ShowPanel(popup, data, CallBack);
    }
    else
    {
      Debug.Log("No existing popup found. Opening.");
      yield return popupsController.Show(popup, data, CallBack);
    }

    Debug.Log("New popUp registered.");
    currentPopup = popup;
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator HidePopupCoroutine()
  {
    yield return popupsController.Hide();

    popupsController.gameObject.SetActive(false);
    currentPopup = null;
  }

}
