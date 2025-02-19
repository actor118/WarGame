using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
/// <summary>
/// 单元地块的游戏对象脚本
/// 主要用来控制地块的状态显示
/// </summary>
public class GridCellBolck : MonoBehaviour
{
    public GridMap Map;
    [Tooltip("地块材质数据文件")]
    public GridMapCellMaterials GridMapCellMaterials;
    [Tooltip("地块Mesh 数据文件")]
    public GridMapCellBlocks GridMapCellBlocks;
    [Tooltip("地块Mesh连接位置")]
    public Transform Block;
    [Tooltip("辅助文字")]
    public GridCellBolck_Text Text;

    [HideInInspector]
    GridCell CellData = null;

    [HideInInspector]
    GridCell CellData_EditMode = null;

    MeshCollider _meshCollider;

    public MeshCollider meshCollider
    {
        get
        {
            if(_meshCollider == null)
            {
                _meshCollider = GetComponent<MeshCollider>();
            }
            return _meshCollider;
        }
    }

    MeshRenderer _meshRenderer = null;
    public MeshRenderer MeshRenderer
    {
        get
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            return _meshRenderer;
        }
    }
    public void Refresh()
    {
        if(CellData != null)
        {
            transform.position = new Vector3(transform.position.x, CellData.Y * 0.5f, transform.position.z);
        }
        if(GridMapCellBlocks == null ||MeshRenderer == null)
        {
            return;
        }

        List<Material> m = new List<Material>();

        if(CellData.CellFactor == CellFactor.Ground)
        {
            m.Add(GridMapCellMaterials.Ground);
        }
        if (CellData.CellFactor == CellFactor.Water)
        {
            m.Add(GridMapCellMaterials.Water);
        }
        if (CellData.CellFactor == CellFactor.Sand)
        {
            m.Add(GridMapCellMaterials.Sand);
        }
        if (CellData.CellFactor == CellFactor.Unreachable)
        {
            m.Add(GridMapCellMaterials.Unreachable);
        }

        if((CellData.CellStatus & CellStatus.IsStart) == CellStatus.IsStart)
        {
            m.Add(GridMapCellMaterials.StartPoint);
        }
        if ((CellData.CellStatus & CellStatus.IsEnd) == CellStatus.IsEnd)
        {
            m.Add(GridMapCellMaterials.EndPoint);
        }
        if ((CellData.CellStatus & CellStatus.Selected) == CellStatus.Selected)
        {
            m.Add(GridMapCellMaterials.Selected);
        }
        if ((CellData.CellStatus & CellStatus.OnHover) == CellStatus.OnHover)
        {
            m.Add(GridMapCellMaterials.OnHover);
        }

        if(CellData_EditMode != null)
        {
            if ((CellData_EditMode.CellStatus & CellStatus.IsPath) == CellStatus.IsPath)
            {
                m.Add(GridMapCellMaterials.Path);
            }
            if ((CellData_EditMode.CellStatus & CellStatus.Round) == CellStatus.Round)
            {
                m.Add(GridMapCellMaterials.RoundCell);
            }
            if ((CellData_EditMode.CellStatus & CellStatus.OnHover) == CellStatus.OnHover)
            {
                m.Add(GridMapCellMaterials.OnHover);
            }
            if ((CellData_EditMode.CellStatus & CellStatus.InCloseList) == CellStatus.InCloseList)
            {
                m.Add(GridMapCellMaterials.InClose);
            }
            if ((CellData_EditMode.CellStatus & CellStatus.InOpenList) == CellStatus.InOpenList)
            {
                m.Add(GridMapCellMaterials.InOpen);
            }

            MeshRenderer.materials = m.ToArray();

            if(Text != null)
            {
                Text.UpdateText();
            }

        }
    }
    public void SetCellData(GridCell cell)
    {
        CellData = cell;
    }
    //计算网格边距大小
    public Bounds GetBounds()
    {
        return MeshRenderer.bounds;
    }

    public GridCell GetCellData_EditMode()
    {
        return CellData_EditMode;
    }

    public void SetCellData_EditMode(GridCell cell,CellStatus? add = null,CellStatus? remove = null)
    {
        if(CellData_EditMode != null)
        {
            CellData_EditMode.CellStatus |= cell.CellStatus;
            CellData_EditMode.F = cell.F;
            CellData_EditMode.H = cell.H;
            CellData_EditMode.G = cell.G;
        }
        else
        {
            CellData_EditMode = cell;
        }

        if(CellData_EditMode != null)
        {
            if(add.HasValue)
            {
                CellData_EditMode.CellStatus |= add.Value;
                
            }
            //移除状态叠加
            if (remove.HasValue)
            {
                CellData_EditMode.CellStatus &= ~remove.Value;
            }
        }
    }
    public void ClearCellData_EditMode()
    {
        CellData_EditMode = null;
    }
    private void OnDestroy()
    {
        CellData = null;
        CellData_EditMode = null;
        if (Text != null)
        {
            Destroy(Text.gameObject);
        }
    }
}
