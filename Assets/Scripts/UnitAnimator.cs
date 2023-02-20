using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform bulletProjectilePrefab;

    [SerializeField]
    private Transform shootPointTransform;

    [SerializeField]
    private Transform rifleTransform;

    [SerializeField]
    private Transform shootPointAvengerTransform;

    [SerializeField]
    private Transform avengerTransform;

    [SerializeField]
    private Transform swordTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) 
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }

        if (TryGetComponent<ShootAvengerAction>(out ShootAvengerAction shootAvengerAction))
        {
            shootAvengerAction.OnActionTaken += ShootAvengerAction_OnActionTaken;
            shootAvengerAction.OnShootAvenger += ShootAvengerAction_OnShootAvenger;
            shootAvengerAction.OnShootAvengerActionCompleted += ShootAvengerAction_OnShootAvengetrActionCompleted;
        }
    }

    private void Start()
    {
        InteractWeapon.OnWeaponPickedUp += InteractWeapon_OnWeaponPickedUp;

        EquipRifle();
    }

    private void InteractWeapon_OnWeaponPickedUp(object sender, EventArgs e)
    {
        if (TryGetComponent<ShootAvengerAction>(out ShootAvengerAction shootAvengerAction))
        {
            shootAvengerAction.OnActionTaken += ShootAvengerAction_OnActionTaken;
            shootAvengerAction.OnShootAvenger += ShootAvengerAction_OnShootAvenger;
            shootAvengerAction.OnShootAvengerActionCompleted += ShootAvengerAction_OnShootAvengetrActionCompleted;
        }
    }
    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void ShootAvengerAction_OnShootAvenger(object sender, ShootAvengerAction.OnShootAvengerEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void ShootAvengerAction_OnActionTaken(object sender, EventArgs e)
    {
        EquipAvenger();
    }

    private void ShootAvengerAction_OnShootAvengetrActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
        avengerTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
        avengerTransform.gameObject.SetActive(false);
    }

    private void EquipAvenger()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(false);
        avengerTransform.gameObject.SetActive(true);
    }
}
