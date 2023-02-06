using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{

    [SerializeField]
    private Material greenMaterial;

    [SerializeField]
    private Material redMaterial;

    [SerializeField]
    private MeshRenderer meshRenderer;


    private GridPosition gridPosition;
    private bool isGreen;
    private Action onInteractionComplete;
    //private float timer;
    //private const float TIMER_DURATION = 1f;
    //private bool isActive;
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        //isActive = true;
        //timer = .5f; 

        if (isGreen)
        {
            SetColorRed();
        } 
        else
        {
            SetColorRed();
        }
    }
}
