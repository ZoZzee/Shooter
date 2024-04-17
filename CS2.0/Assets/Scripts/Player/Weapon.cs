using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Name Weapon")]
    public string nameWeapon;

    [Header("Bullets")]
    public int bullets;
    public int bulletMax;
    public int bulletAll;

    [Header("Damage")]
    public float damage;

    [Header ("Distance")]
    public float distance;

}
