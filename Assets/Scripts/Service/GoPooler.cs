using UnityEngine;

public interface GoPooler {
    void Prepare(string path, int count);
    void Return(GoItem item);
    void Return(GoItem item, float delay);
    T Get<T>(string path, GameObject parent) where T : GoItem;
    T Get<T>(GameObject prefab, GameObject parent) where T : GoItem;
}