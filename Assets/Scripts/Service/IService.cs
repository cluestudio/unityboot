using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ServiceType {
    StringBundle,
}

public interface IService {
    ServiceType type { get; }
    IEnumerator Initialize(BoolResult result);
}