using Assets.Scripts.Utils.ExtensionMethods;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
  private SwitchSceneScreen switchSceneScreen;
  private AsyncOperation asyncOperation;


  public GameState CurrentState { get; private set; }
  public RootBase ActiveRoot { get { return CurrentState.ToRootBase(); } }
  public Signal<GameState> OnStateChanged = new Signal<GameState>();

  public void SwitchToFirstState()
  {
    GameState state = Game.PreferredFirstState == GameState.None ? GameStateEx.FindFirstState() : Game.PreferredFirstState;
    SetState(state, true, true);
  }

  public void SetState(GameState targetState, bool withAnim = true, bool restartState = false)
  {

    if (CurrentState == targetState && !restartState)
    {
      return;
    }

    Game.TimerManager.HaltAll();
    Game.UiManager.CloseAll();
    StartCoroutine(EnableSceneAsync(targetState, withAnim));
  }

  private IEnumerator EnableSceneAsync(GameState targetState, bool withAnim = true)
  {
    // спавним префаб с анимацией перехода между сценами
    if (switchSceneScreen == null && withAnim)
      SpawnSwitchScreen();

    // стартуем загрузку сцены
    asyncOperation = SceneManager.LoadSceneAsync(targetState.ToSceneName(), LoadSceneMode.Single);
    // и выключаем её автовключение
    asyncOperation.allowSceneActivation = false;

    // ждем пока операция закончиться
    while (asyncOperation.progress < 0.9f)
    {
      yield return null;
    }

    // если надо играть анимацию
    if (withAnim)
    {
      var wait = true;
      // запускаем анимацию FadeIn
      switchSceneScreen.FadeIn(() => wait = false);
      // ждем колбека
      yield return new WaitWhile(() => wait);
    }

    // выгружаем активный рут (он выключает всех своих чилдов)
    if (ActiveRoot != null)
      ActiveRoot.Unload();

    // чистим все, что можно, закрываем попапы
    GC.Collect();
    Resources.UnloadUnusedAssets();
    Game.UiManager.CloseAll();

    // разрешаем активацию новой сцены
    asyncOperation.allowSceneActivation = true;
    // обновляем CurrentState и ActiveRoot вместе с ним
    CurrentState = targetState;

    // загружаем новый рут (включает все свои объекты)
    if (ActiveRoot != null)
      ActiveRoot.Load();

    // FadeOut анимации
    if (withAnim)
    {
      switchSceneScreen.FadeOut();
    }

    Game.UiManager.StartQueueProcessor();
    OnStateChanged.Invoke(CurrentState);
  }

  private void SpawnSwitchScreen()
  {
    SwitchSceneScreen switchSceneRef = Resources.LoadAll<SwitchSceneScreen>("").First();
    if (switchSceneRef == null)
    {
      Debug.LogError("<color=red>Cant find [SwitchSceneScreen]\n</color>");
      return;
    }

    switchSceneScreen = Instantiate(switchSceneRef, transform).GetComponent<SwitchSceneScreen>();
  }

}
