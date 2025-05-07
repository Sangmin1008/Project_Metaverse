using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 1f;
    public float Delay
    {
        get => delay;
        set => delay = value;
    }

    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize
    {
        get => weaponSize;
        set => weaponSize = value;
    }

    [SerializeField] private int power = 1;
    public int Power
    {
        get => power;
        set => power = value;
    }

    [SerializeField] private float speed = 1f;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField] private float attackRange = 10f;
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockBack = false;
    public bool IsOnKnockBack
    {
        get => isOnKnockBack;
        set => isOnKnockBack = value;
    }

    [SerializeField] private float knockBackPower = 0.1f;
    public float KnockBackPower
    {
        get => knockBackPower;
        set => knockBackPower = value;
    }

    [SerializeField] private float knockBackTime = 0.5f;
    public float KnockBackTime
    {
        get => knockBackTime;
        set => knockBackTime = value;
    }

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    
    public BaseController Controller { get; private set; }

    private Animator _animator;
    public SpriteRenderer WeaponRenderer { get; private set; }
    public AudioClip attackSoundClip;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        _animator = GetComponentInChildren<Animator>();
        WeaponRenderer = GetComponentInChildren<SpriteRenderer>();

        _animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Attack()
    {
        AttackAnimation();

        if (attackSoundClip != null)
        {
            SoundManager.PlayClip(attackSoundClip);
        }
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        WeaponRenderer.flipY = isLeft;
    }
}
