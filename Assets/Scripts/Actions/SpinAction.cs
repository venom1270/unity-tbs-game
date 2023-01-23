using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        totalSpinAmount += spinAddAmount;
        // Slightly changed logic from tutorial - this guarantees return to original rotation value (bar floating point inaccuracies)
        if (totalSpinAmount >= 360f)
        {
            spinAddAmount = 360f - (totalSpinAmount - spinAddAmount);
            isActive = false;
            onActionComplete();
        }
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
    }
    public void Spin(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        totalSpinAmount = 0f;
        isActive = true;
    }
}