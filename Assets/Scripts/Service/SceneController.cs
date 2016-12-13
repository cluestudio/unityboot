using UnityEngine;
using System.Collections;

public enum SceneType {
    Intro,
    Lobby,
}

public interface SceneController {
    void SwitchScene(SceneType scene);
    void SwitchPrevScene();
    void SwitchSceneWithHistoryClear(SceneType scene);

    Coroutine Run(IEnumerator iterationResult);
    void Stop(Coroutine coroutine);
}
