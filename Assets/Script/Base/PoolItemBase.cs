using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPollItem
{
    void Spawn();

    void ReCycle();
}

public class PoolItemBase : MonoBehaviour, IPollItem
{

    private void OnEnable()
    {
        Spawn();
    }

    private void OnDisable()
    {
        ReCycle();
    }
    public virtual void Spawn()
    {

    }

    public virtual void ReCycle()
    {
        
    }

    
}
