using UnityEngine;
using System.Collections;

public class BoxContent : MonoBehaviour {
    public int width = 100;
    public int height = 100;

    void OnDrawGizmos() {
        var oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 0));
        Gizmos.matrix = oldMatrix;
    }
}
