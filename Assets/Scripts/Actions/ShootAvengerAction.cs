using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAvengerAction : BaseAction
{

    public static event EventHandler<OnShootAvengerEventArgs> OnAnyShootAvenger;
    public event EventHandler<OnShootAvengerEventArgs> OnShootAvenger;
    public event EventHandler OnActionTaken;
    public event EventHandler OnShootAvengerActionCompleted;

    public class OnShootAvengerEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField]
    private LayerMask obstaclesLayerMask;

    private State state;
    private int maxShootDistance = 4;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f; // 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:;
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = .1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = .5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                OnShootAvengerActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShootAvenger?.Invoke(this, new OnShootAvengerEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnAnyShootAvenger?.Invoke(this, new OnShootAvengerEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(70);
    }


    public override string GetActionName()
    {
        return "Shoot Avenger";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return GetValidActionGridPositionList(unit.GetGridPosition());
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not valid
                    continue;
                }

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same team
                    continue;
                }

                // Check if unit sees target unit (if does not hit obstacle)
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized; 
                float unitShoulderHeight = 1.45f; //1.7f

                // Get gun offset - first create test GO, set it at 0 position/roatation, aim to target, and calculate offset
                float righthandGunOffset = 0.14f;
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                GameObject testObject = new GameObject();
                testObject.transform.position = Vector3.zero;
                testObject.transform.rotation = Quaternion.identity;
                testObject.transform.forward = aimDirection;
                Destroy(testObject);
                Vector3 rightHandGunWorldOffset = testObject.transform.TransformPoint(Vector3.right * righthandGunOffset);
                //Debug.Log(rightHandGunWorldOffset);
                // End Get gun offset

                if (Physics.Raycast(
                    unitWorldPosition + Vector3.up * unitShoulderHeight + rightHandGunWorldOffset, 
                    shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    // Blocked by an obstacle
                    continue;
                }
                /*Debug.DrawLine(
                    unitWorldPosition + Vector3.up * unitShoulderHeight + rightHandGunWorldOffset, 
                    targetUnit.GetWorldPosition() + Vector3.up * unitShoulderHeight, 
                    Color.green, 
                    10f
                ); FOR DEBUGGING SHOOT VISIBILITY */

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        OnActionTaken?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit() => targetUnit;

    public int GetMaxShootDistance() => maxShootDistance;

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        // Shoot weakest player unit
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
