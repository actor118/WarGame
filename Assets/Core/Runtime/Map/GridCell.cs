using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum CellFactor
{
    ///<summary>
    ///平地
    ///</summary>
    Ground = 1,
    ///<summary>
    ///沙漠
    ///</summary>
    Sand = 5,//沙漠
    ///<summary>
    ///水域
    ///</summary>
    Water = 9,//水域
    ///<summary>
    ///不可到达
    ///</summary>
    Unreachable = 999//不可到达
}

[SerializeField]
public enum CellStatus
{
    Nothing = 0,//无状态
    OnHover = 1,//光标悬浮
    Selected = 2,//光标选中

    //以下是编辑模式使用的状态
    IsPath = 4,//寻路路径
    IsStart = 5,//表示寻路起点
    IsEnd = 6,//表示寻路终点

    InOpenList = 11,//是否在开放列表中
    InCloseList = 12,//是否在关闭列表中

    IsParent = 100,
    Round = 200,//是否是周围可通过单元格
    Handle = 300,


}




[SerializeField]
public class GridCell : MonoBehaviour
{
    //xz面的二维坐标，游戏内坐标
    public int X;
    public int Z;

    //地块单元格的高度单位
    public int Y;
    public int F, G, H; //F = G + H   A*寻路的代价值

    //父节点对象
    [SerializeField]
    public GridCell parent;

    //地图对象
    public GridMap Map;

    //各自上面的单位
    public GameObject Unit;

    //单元格的地形
    public CellFactor CellFactor;
    //单元格状态
    public CellStatus CellStatus;

    public Vector3Int Pos
    {
        get
        {
            return new Vector3Int(X, Y, Z);
        }
    }
    //构造函数
    public GridCell(int x,int z,CellFactor factor,GridMap map,int y = 0)
    {
        X = x;
        Z = z;
        Y = y;
        F = 0;
        G = 0;
        H = 0;

        CellFactor = factor;
        CellStatus = CellStatus.Nothing;

        this.Map = map;
    }

    //构造函数 复制的时候使用
    public GridCell(GridCell cell)
    {
        X = cell.X;
        Z = cell.Z;
        Y = cell.Y;
        F = 0;
        G = 0;
        H = 0;

        CellFactor = cell.CellFactor;
        CellStatus = CellStatus.Nothing;

        this.Map = cell.Map;
    }

    //增加地块高度
    public void AddY(bool cancel = false)
    {
        if(cancel)
        {
            Y--;
        }
        else
        {
            Y++;
        }
    }

    public override string ToString()
    {
        return $"{X}-{Z}/{Y}";
    }

    //计算单元格的距离
    public float GetDistance(GridCell cell)
    {
        float d = Vector2Int.Distance(new Vector2Int(cell.X, cell.Z), new Vector2Int(X, Z));
        return d;
    }

    //获得单元格的世界坐标
    public Vector3 GetWorldPosition()
    {
        GridCellBolck Obj = Map.GetCellBlockObject(X, Z);
        if(Obj != null)
        {
            return Obj.transform.position;
        }
        return Vector3.zero;


    }

    public static bool operator ==(GridCell src,GridCell dest)
    {
        if(System.Object.Equals(dest,null) && System.Object.Equals(src,null))
        {
            return true;
        }
        else if(System.Object.Equals(dest,null) && System.Object.Equals(src, null) == false)
        {
            return false;
        }
        else if(System.Object.Equals(dest, null) == false && System.Object.Equals(src, null))
        {
            return false;
        }
        else
        {
            return src.X == dest.X && src.Z == dest.Z;
        }
    }
    public static bool operator !=(GridCell src, GridCell dest)
    {
        if (System.Object.Equals(dest, null) && System.Object.Equals(src, null))
        {
            return false;
        }
        else if (System.Object.Equals(dest, null) && System.Object.Equals(src, null) == false)
        {
            return true;
        }
        else if (System.Object.Equals(dest, null) == false && System.Object.Equals(src, null))
        {
            return true;
        }
        else
        {
            return src.X != dest.X && src.Z != dest.Z;
        }
    }
}
