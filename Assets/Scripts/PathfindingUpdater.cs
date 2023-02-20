using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{

    private void Start()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;

        InteractWeapon.OnWeaponPickedUp += InteractWeapon_OnWeaponPickedUp;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;

        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }

    private void InteractWeapon_OnWeaponPickedUp(object sender, EventArgs e)
    {
        InteractWeapon interactWeapon = sender as InteractWeapon;

        Pathfinding.Instance.SetIsWalkableGridPosition(interactWeapon.GetGridPosition(), true);
    }
}
