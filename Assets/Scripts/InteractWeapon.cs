using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class InteractWeapon : MonoBehaviour, IInteractable
{

    public static event EventHandler OnWeaponPickedUp;

    private GridPosition gridPosition;

    private Action onInteractionComplete;
    private float timer;
    private const float TIMER_DURATION = 0.2f;
    private bool isActive;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);
            onInteractionComplete();

            Destroy(gameObject);

            OnWeaponPickedUp?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = TIMER_DURATION;

        Unit currentlySelectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        currentlySelectedUnit.AddAction<ShootAvengerAction>();
    }

    public GridPosition GetGridPosition() => gridPosition;
}
