using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class ButtonEntryChecker {
    const float defaultThreshold = 1f;
    Dictionary<string, float> map = new Dictionary<string, float>();

    public bool denyAll { get; set; }

    public bool CanEnter(string name) {
        return CanEnter(name, defaultThreshold);
    }

    public bool CanEnter(string name, float threshold) {
        if (denyAll) {
            return false;
        }

        float now = Time.realtimeSinceStartup;
        if (map.ContainsKey(name) == true) {
            float diff = now - map[name];
            if (diff < threshold) {
                return false;
            }
        }

        map[name] = now;
        return true;
    }

    public void Clear() {
        map.Clear();
        denyAll = false;
    }

    public void Clear(string name) {
        map.Remove(name);
    }
}