using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum CellFactor
{
    ///<summary>
    ///ƽ��
    ///</summary>
    Ground = 1,
    ///<summary>
    ///ɳĮ
    ///</summary>
    Sand = 5,//ɳĮ
    ///<summary>
    ///ˮ��
    ///</summary>
    Water = 9,//ˮ��
    ///<summary>
    ///���ɵ���
    ///</summary>
    Unreachable = 999//���ɵ���
}

[SerializeField]
public enum CellStatus
{
    Nothing = 0,//��״̬
    OnHover = 1,//�������
    Selected = 2,//���ѡ��

    //�����Ǳ༭ģʽʹ�õ�״̬
    IsPath = 4,//Ѱ··��
    IsStart = 5,//��ʾѰ·���
    IsEnd = 6,//��ʾѰ·�յ�

    InOpenList = 11,//�Ƿ��ڿ����б���
    InCloseList = 12,//�Ƿ��ڹر��б���

    IsParent = 100,
    Round = 200,//�Ƿ�����Χ��ͨ����Ԫ��
    Handle = 300,


}




[SerializeField]
public class GridCell : MonoBehaviour
{
    //xz��Ķ�ά���꣬��Ϸ������
    public int X;
    public int Z;

    //�ؿ鵥Ԫ��ĸ߶ȵ�λ
    public int Y;
    public int F, G, H; //F = G + H   A*Ѱ·�Ĵ���ֵ

    //���ڵ����
    [SerializeField]
    public GridCell parent;

    //��ͼ����
    public GridMap Map;

    //��������ĵ�λ
    public GameObject Unit;

    //��Ԫ��ĵ���
    public CellFactor CellFactor;
    //��Ԫ��״̬
    public CellStatus CellStatus;

    public Vector3Int Pos
    {
        get
        {
            return new Vector3Int(X, Y, Z);
        }
    }
    //���캯��
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

    //���캯�� ���Ƶ�ʱ��ʹ��
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

    //���ӵؿ�߶�
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

    //���㵥Ԫ��ľ���
    public float GetDistance(GridCell cell)
    {
        float d = Vector2Int.Distance(new Vector2Int(cell.X, cell.Z), new Vector2Int(X, Z));
        return d;
    }

    //��õ�Ԫ�����������
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
