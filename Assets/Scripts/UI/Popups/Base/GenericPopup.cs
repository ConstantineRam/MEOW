using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class GenericPopup : MonoBehaviour
{
  public bool CanBeClosedByClickOnFade = true;
  public bool HasCustomDecor = false;
  public bool DisableBlur = false;

  [HideInInspector]
  public PopupId CallBack = PopupId.NoPopUp;
  public bool HasCallback { get { if (this.CallBack == PopupId.NoPopUp ) { return false; }  return true; } }

  [HideInInspector]
  public object CallBackData = null;

  //---------------------------------------------------------------------------------------------------------------

  public virtual void Activate(object data) { }
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// The difference beteen OnCLose and CloseSelf is that OnClose has all things you want to perform when this PopUp closes, no matter did it close itself via btn or by manager due to opening other popUp.
  /// CloseSelf is to be called when PopUp is intentionally closed by user or game.
  /// </summary>
  public virtual void OnClose()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void CloseSelf()
  {

    this.OnClose();

    if (this.CallBack == PopupId.NoPopUp)
    {
      Game.UiManager.Hide();
    }
    else
    {
      this.ExecuteCallBack();
    }
    
  }

  //---------------------------------------------------------------------------------------------------------------
  protected void ExecuteCallBack()
  {
    if (this.CallBack == PopupId.NoPopUp)
    {
      return;
    }

    Game.UiManager.Open(this.CallBack, this.CallBackData);
  }










#if UNITY_EDITOR
  public void OpenForEdit()
  {
    FakePopupsCanvas fakeCanvas;
    fakeCanvas = FindObjectOfType<FakePopupsCanvas>();
    if (fakeCanvas == null)
    {
      //var pew = Resources.FindObjectsOfTypeAll(typeof(FakePopupsCanvas)).First() as FakePopupsCanvas;
      var pew = Resources.LoadAll<FakePopupsCanvas>("").First();
      fakeCanvas = Instantiate(pew);
    }
    PrefabUtility.DisconnectPrefabInstance(fakeCanvas);
    fakeCanvas.transform.SetAsLastSibling();

    var oldObject = fakeCanvas.Container.GetComponentInChildren(GetType());
    if (oldObject != null)
    {
      EditorGUIUtility.PingObject(oldObject.gameObject);
      Selection.activeGameObject = oldObject.gameObject;
      return;
    }

    var popup = PrefabUtility.InstantiatePrefab(gameObject) as GameObject;
    popup.transform.parent = fakeCanvas.Container;
    popup.transform.localScale = Vector3.one;

    EditorGUIUtility.PingObject(popup.gameObject);
    Selection.activeGameObject = popup.gameObject;
  }
#endif

}
