using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour   // ΩÃ±€≈Ê
{
    public static T Inst;

    public virtual void Awake()
    {
        if (Inst != null) { Destroy(gameObject); return; }

        Inst = GetComponent<T>();
        if (Inst == null)
            Inst = gameObject.AddComponent<T>();

        DontDestroyOnLoad(gameObject);
    }
}