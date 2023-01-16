using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBuffs : MonoBehaviour
{
    public GameObject buffs;
    public static List<Modifiers> playerBuffs = new();
    private void Awake()
    {
        DontDestroyOnLoad(buffs);
    }

    private void Start()
    {
    }
}
