using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedGarbageController : UnitySingleton<SpawnedGarbageController>
{
    public void AddAsChild(GameObject child)
    {
        child.transform.parent = transform;
    }
}
