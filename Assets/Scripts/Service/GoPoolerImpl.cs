using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GoPoolerImpl : SingletonGameObject<GoPoolerImpl>, GoPooler {
    Dictionary<string, Stack> storedObjects = new Dictionary<string, Stack>();
    Dictionary<string, GameObject> storedPrefabs = new Dictionary<string, GameObject>();

    public T Get<T>(string path, GameObject parent) where T : GoItem {
        if (storedObjects.ContainsKey(path)) {
            Stack stack = storedObjects[path];
            GameObject stored = PopGameObjectFromStack(stack);
            if (stored != null) {
                SetReady(stored, parent);
                return stored.GetComponent<T>();
            }
        }

        GameObject newly = MakeLocal(path);
        if (newly == null) {
            return null;
        }

        SetReady(newly, parent);
        return newly.GetComponent<T>();
    }

    public T Get<T>(GameObject prefab, GameObject parent) where T : GoItem {
        string path = prefab.GetHashCode().ToString();
        storedPrefabs[path] = prefab;
        return Get<T>(path, parent);
    }

    public void Return(GoItem item) {
        if (item == null) {
            return;
        }

        string path = item.resourcePath;
        if (path == null || path.Length == 0) {
            Destroy(item.gameObject);
            return;
        }

        if (item.isInPool == true) {
            return;
        }

        Stack stack = GetStack(path);
        stack.Push(new WeakReference(item.gameObject));
        item.isInPool = true;
        item.OnGoingIntoPool();
        MatchParent(item);
        item.transform.localPosition = Vector3.up * 9999;
        item.gameObject.SetActive(false);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Return(GoItem item, float delay) {
        StartCoroutine(ReturnWithDelay(item, delay));
    }

    IEnumerator ReturnWithDelay(GoItem item, float delay) {
        int useCount = item.useCount;
        yield return new WaitForSeconds(delay);
        if (useCount != item.useCount) {
            yield break;
        }

        Return(item);
    }

    public void Prepare(string path, int count) { 
        PrepareLocal(path, count);
    }
   
    GameObject MakeLocal(string path) {
        GameObject itemObj = null;
        
        GameObject prefab;
        if (!storedPrefabs.TryGetValue(path, out prefab) || prefab == null) {
            prefab = Resources.Load<GameObject>(path);
            storedPrefabs[path] = prefab;
        }

        itemObj = Instantiate(prefab) as GameObject;
        if (itemObj == null) {
            Logger.LogError("Can not load GameObject:" + path);
            return null;
        }

        GoItem item = itemObj.GetComponent<GoItem>();
        if (item == null) {
            Logger.LogError("Doesn't have GoItem:" + path);
        }

        item.resourcePath = path;
        MatchParent(item);
        item.transform.localScale = Vector3.one;
        return itemObj;
    }

    Stack GetStack(string path) {
        Stack stack = null; 
        if (storedObjects.ContainsKey(path)) {
            stack = storedObjects[path];
        }
        else {
            stack = new Stack();
            storedObjects[path] = stack;
        }

        return stack;
    }

    void SetReady(GameObject itemObj, GameObject parent) {
        if (parent != null) {
            itemObj.transform.parent = parent.transform;
        }

        itemObj.transform.localPosition = Vector3.up * 9999;
        itemObj.SetActive(true);

        GoItem item = itemObj.GetComponent<GoItem>();
        if (item != null) {
            item.useCount++;
            item.isInPool = false;
            item.OnGettingOutPool();
        }
    }

    void PrepareLocal(string path, int count) { 
        for (int i=0; i<count; i++) {
            GameObject itemObj = MakeLocal(path);
            if (itemObj == null) {
                continue;
            }

            GoItem item = itemObj.GetComponent<GoItem>();
            if (item == null) {
                Logger.LogError("Doesn't have GoItem:" + path);
            }

            MatchParent(item);
            item.transform.localPosition = Vector3.zero;
            itemObj.SetActive(true);
            item.resourcePath = path;
            item.Prepare();
            Return(item);
        }
    }

#if BOOT_NGUI_SUPPORT
    bool IsNGUIWidget(GoItem item) {
        return item.GetComponentInChildren<UIWidget>() != null;
    }

    void MatchParent(GoItem item) {
        if (IsNGUIWidget(item)) {
            if (UIRoot.list.Count > 0) {
                foreach (UIRoot root in UIRoot.list) {
                    if (root.gameObject.layer == item.gameObject.layer) {
                        item.transform.parent = root.transform;
                        break;
                    }
                }
            }
        }
        else {
            item.transform.parent = transform;
        }
    }
#else
    void MatchParent(GoItem item) {
        item.transform.parent = transform;
    }
#endif

    GameObject PopGameObjectFromStack(Stack stack) {
        while (stack.Count > 0) {
            WeakReference item = (WeakReference)stack.Pop();

            if (item.Target != null) {
                return item.Target as GameObject;
            }
        }

        return null;
    }
}
