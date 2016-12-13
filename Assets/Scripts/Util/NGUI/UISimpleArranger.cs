using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if BOOT_NGUI_SUPPORT

[ExecuteInEditMode]
public class UISimpleArranger : MonoBehaviour {
    public int width = 100;
    public int height = 100;
    public int bodyWidth = 0;
    public int bodyHeight = 0;
    public Vector2 offset = Vector2.zero;

    public enum AnchroType {
        Center,
        Left,
        Right,
        Top,
        Bottom,
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
    }

    public AnchroType anchor;

    void Start() {
        Arrange();
    }

#if UNITY_EDITOR
    void Update() {
        if (Application.isPlaying == false) {
            Arrange();
        }
    }
#endif

    public void Arrange() {
        Vector3 anchorPt = Vector3.zero;
        float z = transform.position.z;

        float xMin = -width * 0.5f;
        float xMax = width * 0.5f;
        float yMin = -height * 0.5f;
        float yMax = height * 0.5f;

        float centerX = 0;
        float centerY = 0;

        switch(anchor) {
            case AnchroType.Center:
                anchorPt = new Vector3(centerX, centerY, z);
                break;
            case AnchroType.Left:
                anchorPt = new Vector3(xMin, centerY, z);
                break;
            case AnchroType.Right:
                anchorPt = new Vector3(xMax, centerY, z);
                break;
            case AnchroType.Top:
                anchorPt = new Vector3(centerX, yMax, z);
                break;
            case AnchroType.Bottom:
                anchorPt = new Vector3(centerX, yMin, z);
                break;
            case AnchroType.LeftTop:
                anchorPt = new Vector3(xMin, yMax, z);
                break;
            case AnchroType.LeftBottom:
                anchorPt = new Vector3(xMin, yMin, z);
                break;
            case AnchroType.RightTop:
                anchorPt = new Vector3(xMax, yMax, z);
                break;
            case AnchroType.RightBottom:
                anchorPt = new Vector3(xMax, yMin, z);
                break;
        }

        UIPanel panel = GetComponentInChildren<UIPanel>();
        if (panel == null) {
            return;
        }

        UIRoot root = panel.root;
        if (root == null) {
            return;
        }

        float halfWidth = (Screen.width * ((float)root.activeHeight/Screen.height)) / 2;
        float halfHeight = root.activeHeight / 2;

        if (bodyWidth > 0) {
            halfWidth = bodyWidth * 0.5f;
        }
        if (bodyHeight > 0) {
            halfHeight = bodyHeight * 0.5f;
        }

        Vector3 position = Vector3.zero;
        switch(anchor) {
            case AnchroType.Left:
                position.x = -halfWidth;
                break;
            case AnchroType.Right:
                position.x = halfWidth;
                break;
            case AnchroType.Top:
                position.y = halfHeight;
                break;
            case AnchroType.Bottom:
                position.y = -halfHeight;
                break;
            case AnchroType.LeftTop:
                position.x = -halfWidth;
                position.y = halfHeight;
                break;
            case AnchroType.LeftBottom:
                position.x = -halfWidth;
                position.y = -halfHeight;
                break;
            case AnchroType.RightTop:
                position.x = halfWidth;
                position.y = halfHeight;
                break;
            case AnchroType.RightBottom:
                position.x = halfWidth;
                position.y = -halfHeight;
                break;
        }

        transform.localPosition = position + (Vector3)offset  - anchorPt;
    }

    void OnDrawGizmos() {
        var oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 0));
        Gizmos.matrix = oldMatrix;
    }
}


#endif