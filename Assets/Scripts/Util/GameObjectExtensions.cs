using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectExtensions {
    public static GameObject FirstDescendant(this MonoBehaviour v, string name) {
        return GetGameObject(v.gameObject, name);
    }

    public static GameObject FirstDescendant(this GameObject v, string name) {
        return GetGameObject(v, name);
    }

    private static GameObject GetGameObject(this GameObject v, string name) {
        Queue<Transform> list = new Queue<Transform>();

        for (int i=0; i<v.transform.childCount; i++) {
            list.Enqueue(v.transform.GetChild(i));
        }

        while (list.Count > 0) {
            Transform trans = list.Dequeue();

            if (trans.gameObject.name == name){
                return trans.gameObject;
            }
            else {
                for (int j=0;j<trans.childCount;j++) {
                    list.Enqueue(trans.GetChild(j));
                }
            }
        }

        return null;
    }

    public static T FirstDescendantComponent<T>(this MonoBehaviour v, string name) where T : Component {
        GameObject obj = v.FirstDescendant(name);
        if (obj == null) {
            return null;
        }
     
        return obj.GetComponent<T>();
    }

    public static T FirstDescendantComponent<T>(this GameObject v, string name) where T : Component {
        GameObject obj = v.FirstDescendant(name);
        if (obj == null) {
            return null;
        }

        return obj.GetComponent<T>();
    }

    public static T GetGameObjectComponent<T>(this MonoBehaviour v, string name) where T : Component {
        GameObject obj = GameObject.Find(name);
        if (obj == null) {
            return null;
        }
     
        return obj.GetComponent<T>();
    }

    public static T InstantiateUI<T>(this MonoBehaviour v, string path, GameObject parent) where T : Component {
        path = "Prefabs/UI/" + path;
        GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
        if (parent != null) {
            obj.transform.parent = parent.transform;
        }
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<T>();
    }

    public static T Instantiate<T>(this MonoBehaviour v, string path, GameObject parent) where T : Component {
        path = "Prefabs/" + path;
        GameObject obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
        if (parent != null) {
            obj.transform.parent = parent.transform;
        }
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<T>();
    }

    public static void ClearChildren(this MonoBehaviour v) {
        for (int i=0; i<v.transform.childCount; i++) {
            UnityEngine.Object.Destroy(v.transform.GetChild(i).gameObject);
        }
    }    

    public static void ClearChildren(this GameObject v) {
        for (int i=0; i<v.transform.childCount; i++) {
            UnityEngine.Object.Destroy(v.transform.GetChild(i).gameObject);
        }
    }    

    public static Color HexToColor(this Color v, string hex) {
        byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r,g,b, 255);
    }

    public static bool HasTouch(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount >= 1;
        }

        return Input.GetMouseButton(0);
    }

    public static bool HasTouch1(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount == 1;
        }

        return Input.GetMouseButton(0);
    }

    public static bool HasTouch2(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount == 2;
        }

        return false;
    }

    public static bool HasTouch1Began(this MonoBehaviour v) {
#if !UNITY_EDITOR
        if ( Application.platform == RuntimePlatform.IPhonePlayer || 
             Application.platform == RuntimePlatform.Android) {
            return Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
#endif
        return Input.GetMouseButtonDown(0);
    }

    public static bool HasTouch1Doing(this MonoBehaviour v) {
#if !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.touchCount == 1 && 
                (Input.GetTouch(0).phase == TouchPhase.Moved || 
                Input.GetTouch(0).phase == TouchPhase.Stationary);
        }
#endif

        return Input.GetMouseButton(0);
    }

    public static Vector3 GetTouchPosition1(this MonoBehaviour v) {
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.Android) {
            return Input.GetTouch(0).position;
        }

        return Input.mousePosition;
    }

#if BOOT_NGUI_SUPPORT
    public static Vector3 GetUITouchPosition(this UIRoot root, Vector3 pt) {
        float scale = root.GetScreenUIScale();
        float width = Screen.width * scale;
        float height = Screen.height * scale;

        Vector3 res = pt * scale;
        res.x -= width * 0.5f;
        res.y -= height * 0.5f;
        return res;
    }    

    public static Vector3 GetUITouchPosition1(this UIRoot root) {
        float scale = root.GetScreenUIScale();
        float width = Screen.width * scale;
        float height = Screen.height * scale;

        Vector3 pt = root.GetTouchPosition1() * scale;
        pt.x -= width * 0.5f;
        pt.y -= height * 0.5f;
        return pt;
    }    

    public static float GetUIWidth(this UIRoot root) {
        float scale = root.GetScreenUIScale();
        return Screen.width * scale;
    }

    public static float GetUIHeight(this UIRoot root) {
        float scale = root.GetScreenUIScale();
        return Screen.height * scale;
    }

    public static float GetScreenUIScale(this UIRoot root) {
        if (root.fitWidth) {
            return root.manualWidth / (float)Screen.width;
        }
        else {
            return root.manualHeight / (float)Screen.height;
        }
    }

    public static Vector3 ScreenToUIPosition(this UIRoot v, Vector3 pt) {
        float screenScale = 1f;
        if (v.fitWidth) {
            screenScale = v.manualWidth / (float)Screen.width;
        }
        else {
            screenScale = v.manualHeight / (float)Screen.height;
        }

        Vector3 position = pt;
        position.x -= Screen.width / 2f;
        position.y -= Screen.height / 2f;
        position.x *= screenScale;
        position.y *= screenScale;
        position.z = 0;
        return position;
    }

    public static void SetDepthRecursive(this UIPanel v, int depth) {
        int offset = depth - v.depth;

        UIPanel []panels = v.GetComponentsInChildren<UIPanel>();
        foreach (UIPanel panel in panels) {
            panel.depth += offset;
        }
    }    
#endif

    public static void StopCoroutineSafe(this MonoBehaviour v, Coroutine coroutine) {
        if (coroutine != null) {
            v.StopCoroutine(coroutine);
        }
    }

    public static string Indent(this string v, int indent) {
        if (indent > 0) {
            string space = "  ";
            for (int i=0; i<indent; i++) {
                space += "  ";
            }

            v = space + v.Replace("\n", "\n" + space);
        }
        return v;
    }

    public static void ChangeLayers(this GameObject go, string name) {
        ChangeLayers(go, LayerMask.NameToLayer(name));
    }

    public static void ChangeLayers(this GameObject go, int layer) {
        go.layer = layer;
        foreach (Transform child in go.transform) {
            ChangeLayers(child.gameObject, layer);
        }
    }
}