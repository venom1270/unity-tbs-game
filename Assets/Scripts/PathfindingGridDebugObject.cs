using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{

    [SerializeField]
    private TextMeshPro gCostext;

    [SerializeField]
    private TextMeshPro hCostext;

    [SerializeField]
    private TextMeshPro fCostext;

    [SerializeField]
    private SpriteRenderer isWalkableSpriteRenderer;

    private PathNode pathNode;
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode) gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostext.text = pathNode.GetGCost().ToString();
        hCostext.text = pathNode.GetHCost().ToString();
        fCostext.text = pathNode.GetFCost().ToString();
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}
