using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangeWeaponHandler _rangeWeaponHandler;
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private ProjectileManager _projectileManager;

    public bool fxOnDestroy = true;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady) return;

        currentDuration += Time.deltaTime;
        if (currentDuration > _rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = direction * _rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << other.gameObject.layer)))
        {
            DestroyProjectile(other.ClosestPoint(transform.position) - direction * 0.2f, fxOnDestroy);
        }
        else if (_rangeWeaponHandler.target.value == (_rangeWeaponHandler.target.value | (1 << other.gameObject.layer)))
        {
            ResourceController resourceController = other.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-_rangeWeaponHandler.Power);
                if (_rangeWeaponHandler.IsOnKnockBack)
                {
                    BaseController controller = other.GetComponent<BaseController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockBack(transform, _rangeWeaponHandler.KnockBackPower, _rangeWeaponHandler.KnockBackTime);
                    }
                }
            }
            
            DestroyProjectile(other.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
    {
        _projectileManager = projectileManager;
        _rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        _spriteRenderer.color = weaponHandler.ProjectileColor;

        transform.right = this.direction;

        if (this.direction.x < 0)
        {
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(0, 0, 0);
        }

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            _projectileManager.CreateImpactParticlesAtPosition(position, _rangeWeaponHandler);
        }
        Destroy(this.gameObject);
    }
}
