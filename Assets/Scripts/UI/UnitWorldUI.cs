using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI actionPointsText;

    [SerializeField]
    private Unit unit;

    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private HealthSystem healthSystem;



    private void Start()
    {
        // Since this is a static event, it will trigger for ALL units when ANY unit triggers it. Optimization opportunity.
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;


        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

}
