using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneControllerBase : MonoBehaviour, SceneController {
    public static SceneType returnScene = SceneType.Lobby;
    Stack<SceneType> sceneHistory = new Stack<SceneType>();

    protected bool InitController(SceneType returnScene) {
        Service.scene = this;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex != 0 && Service.ready == false) {
            SceneControllerBase.returnScene = returnScene;
            SceneManager.LoadScene(0);
            return false;
        }
        return true;
    }

    public void CloseController() {
        Service.scene = null;
    }

    public void SwitchScene(SceneType scene) {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene.ToString());
        sceneHistory.Push(scene);
    }

    public void SwitchPrevScene() {
        if (sceneHistory.Count > 0) {
            var scene = sceneHistory.Pop();
            SceneManager.LoadScene(scene.ToString());
        }
    }

    public void SwitchSceneWithHistoryClear(SceneType scene) {
        sceneHistory.Clear();
        SwitchScene(scene);
    }

    public Coroutine Run(IEnumerator iterationResult) {
        return StartCoroutine(iterationResult);
    }

    public void Stop(Coroutine coroutine) {
        this.StopCoroutineSafe(coroutine);
    }
}