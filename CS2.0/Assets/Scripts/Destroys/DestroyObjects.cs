using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{

    [Header("Destroy time")]
    public float destroyObgect = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyObgect);
    }
}
