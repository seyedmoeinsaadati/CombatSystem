using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller<T> : MonoBehaviour
{
    protected T target;
    public bool IsActive { get { return enabled; } set { enabled = value; } }


    protected void Awake()
    {
        target = GetComponent<T>();
    }

    protected void Update() => Control();

    public abstract void Control();

    public void ChangeTarget(T target)
    {
        this.target = target;
    }
}