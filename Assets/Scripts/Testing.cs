using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField]
    private Unit unit;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridSystemVisual.instance.HideAllGridPositions();
            List<GridPosition> list = unit.GetComponent<MoveAction>().GetValidActionGridPositionList();
            GridSystemVisual.instance.ShowGridPositionList(list);
        }
    }

}
