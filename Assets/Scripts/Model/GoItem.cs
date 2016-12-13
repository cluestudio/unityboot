using UnityEngine;

public class GoItem : MonoBehaviour {
    public int useCount { get; set; }
    public string resourcePath { get; set; }

    public bool IsActive() { return gameObject.activeSelf; }
    public virtual void Prepare() {}
    public virtual void OnGettingOutPool() { }
    public virtual void OnGoingIntoPool() { }
}
