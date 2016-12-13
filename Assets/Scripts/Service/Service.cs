using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Service : SingletonGameObject<Service> {
    // client version set
    static Version _version = new Version();

    // properties
    //-------------------------------------------------------------------------
    public static bool ready { get; set; }
    public static Version version { get { return _version; } }

    // Services
    //-------------------------------------------------------------------------
    public static SceneController scene { get; set; }
    public static GoPooler goPooler { get { return GoPoolerImpl.instance; } }
    public static StringBundleService sb { get { return StringBundleServiceImpl.instance; } }
    public static EncryptionService encryption { get { return EncryptionServiceImpl.instance; } }
    public static SoundService sound { get { return SoundServiceImpl.instance; } }
    public static TimeService time { get { return TimeServiceImpl.instance; } }

    // Callbacks
    //-------------------------------------------------------------------------
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    // APIs
    //-------------------------------------------------------------------------
    public static Coroutine Run(IEnumerator iterationResult) {
        return Service.instance.StartCoroutine(iterationResult);
    }

    public static void Stop(Coroutine coroutine) {
        Service.instance.StopCoroutineSafe(coroutine);
    }
}