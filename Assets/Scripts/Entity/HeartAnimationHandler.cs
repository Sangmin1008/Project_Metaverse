using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimationHandler : MonoBehaviour
{
    private static readonly int IsHealing = Animator.StringToHash("IsHealing");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    
    protected Animator animator;
    
    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    public void StartDamage()
    {
        animator.SetBool(IsDamage, true);
    }
    
    public void EndDamage()
    {
        animator.SetBool(IsDamage, false);
    }

    public void StartHealing()
    {
        animator.SetBool(IsHealing, true);
    }
    
    public void EndHealing()
    {
        animator.SetBool(IsHealing, false);
    }
}
