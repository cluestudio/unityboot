using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BoxContentWidget : BoxContent {
#if BOOT_NGUI_SUPPORT    
    UIWidget widget = null;

    void Start() {
        Refresh();
    }

    void Refresh() {
        if (widget == null) {
            widget = GetComponent<UIWidget>();
        }

        width = widget.width;
        height = widget.height;
    }

#if UNITY_EDITOR
    void Update() {
        Refresh();
    }
#endif
    
#endif
}
