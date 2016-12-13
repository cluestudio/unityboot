using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HBox : MonoBehaviour {
    public int padding = 10;
    List<BoxContent> contents = new List<BoxContent>();

    void Start() {
        RefreshContents();
        Arrange();
    }

    void RefreshContents() {
        contents.Clear();
        foreach (Transform child in this.transform) {
            var content = child.GetComponent<BoxContent>();
            if (content != null && content.gameObject.activeSelf) {
                contents.Add(content);
            }
        }
    }


#if UNITY_EDITOR        
    void Update() {
        Arrange();
    }
#endif

    public void Arrange() {
#if UNITY_EDITOR        
        RefreshContents();
#endif

        int contentsWidth = 0;
        foreach (var content in contents) {
            contentsWidth += content.width;
        }


        int width = contentsWidth + (contents.Count-1) * padding;
        Vector3 offset = Vector3.zero;
        offset.x -= width * 0.5f;
        foreach (var content in contents) {
            Vector3 position = offset;
            position.x += content.width * 0.5f;
            content.transform.localPosition = position;
            offset.x += content.width + padding;
        }
    }
}
