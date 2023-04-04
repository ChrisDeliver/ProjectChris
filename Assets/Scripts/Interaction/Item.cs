using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="2D/Item")]

public class Item : ScriptableObject
{
    [SerializeField] public Sprite icon = null;
}