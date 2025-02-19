using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptData/GridMapCellMaterials")]
public class GridMapCellMaterials : ScriptableObject
{
    public Material Ground;
    public Material Sand;
    public Material Water;
    public Material Unreachable;

    public Material Default;

    public Material OnHover;
    public Material Selected;

    public Material Path;

    public Material StartPoint;
    public Material EndPoint;

    public Material InClose;
    public Material InOpen;

    public Material RoundCell;

    public Material Parent;
}
