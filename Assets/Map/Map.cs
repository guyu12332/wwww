using UnityEngine;

public class Map : MonoBehaviour
{

    // Use this for initialization
   // [SerializeField]
    private Sprite mapTileSp;
    int row;
    int col;
    GameObject mapTile;
    GameObject map;
    //public List<MapTile> mapTileList;

    private readonly float TileWidth = 2.0427f; //1.909090909090909
    private readonly float TileHeight = 1.07f;

    private MapTile[,] mapTileList;
   public  void Init(int row, int col)
    {
        //row = 5;
        //col = 5;
        map = GameObject.Find("GameObject");
        mapTile = Resources.Load<GameObject>("mapTile");
        this.row = row;
        this.col = col;
        mapTileList = new MapTile[row, col];
        initMap();
    }

    private void initMap()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                float x = (j - i) * TileWidth / 2;
                float y = (i + j) * (TileHeight / 2) - row * 0.3f;
                GameObject tmp = GameObject.Instantiate(mapTile);
                tmp.transform.SetParent(map.transform, false);
                tmp.transform.localPosition = new Vector3(x, y, 0);

                MapTile tile = tmp.GetComponent<MapTile>();
                tile.setXY(i, j);
                // Debug.DrawLine(new Vector3(x- TileWidth / 2, y + TileHeight / 2), new Vector3(x + TileWidth / 2, y + TileHeight / 2), Color.red,100000);
                // Debug.DrawLine(new Vector3(x - TileWidth / 2, y - TileHeight / 2), new Vector3(x + TileWidth / 2, y - TileHeight / 2), Color.red,10000);
                // Debug.DrawLine(new Vector3(x - TileWidth / 2, y + TileHeight / 2), new Vector3(x - TileWidth / 2, y - TileHeight / 2), Color.red,10000);
                // Debug.DrawLine(new Vector3(x + TileWidth / 2, y - TileHeight / 2), new Vector3(x + TileWidth / 2, y + TileHeight / 2), Color.red,10000);
                mapTileList[i, j] = tile;
            }



        }
    }

    void Update()
    {
        //点击地图上的图快，使其变色。
        if (Input.GetMouseButtonDown(0))
        {
            MapTile tile = getGameXY(Input.mousePosition);

            tile.GetComponent<Renderer>().material.color = Color.red;
        }
        //  if (mapTileList.Length > 0)
        //   {
        //int x = mapTileList[0,0].X;
        //int y = mapTileList[0,0].Y;
        // Debug.DrawLine(new Vector2(x - TileWidth / 2, y + TileHeight / 2), new Vector2(x + TileWidth / 2, y + TileHeight / 2), Color.red, 100);
        // Debug.DrawLine(new Vector2(x - TileWidth / 2, y - TileHeight / 2), new Vector2(x + TileWidth / 2, y - TileHeight / 2), Color.red, 100);
        //  Debug.DrawLine(new Vector2(x - TileWidth / 2, y + TileHeight / 2), new Vector2(x - TileWidth / 2, y - TileHeight / 2), Color.red, 1000);
        //  Debug.DrawLine(new Vector2(x + TileWidth / 2, y - TileHeight / 2), new Vector2(x + TileWidth / 2, y + TileHeight / 2), Color.red, 1000);
        //      }

    }

    /// <summary>
    /// 获取游戏坐标(正方形)有误差
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public MapTile getGameXY(Vector2 pos)
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 3));
        // Debug.Log(p);
        //  p = map.transform.InverseTransformPoint(p);
        //  Debug.Log(p);
        //p.x = p.x - Screen.width / 2;
        p.y = p.y + row * 0.3f;
        int x = (int)Mathf.Round(0.5f * (p.y / (TileHeight / 2) - p.x / (TileWidth / 2)));
        int y = (int)Mathf.Round(0.5f * (p.y / (TileHeight / 2) + p.x / (TileWidth / 2)));

        Debug.Log(x + "  " + y);

        //根据矩形算出来的图快
        MapTile tile = mapTileList[x, y];
        Vector3 tilePos = tile.transform.localPosition;
        ///八个点
        Vector3 leftTop = tilePos + new Vector3(-TileWidth, -TileHeight);
        Vector3 topMiddle = tilePos + new Vector3(0, -TileHeight / 2);
        Vector3 rightTop = tilePos + new Vector3(TileWidth, -TileHeight);
        Vector3 rightMiddle = tilePos + new Vector3(TileWidth, 0);
        Vector3 rightBottom = tilePos + new Vector3(TileWidth, TileHeight);
        Vector3 bottomMiddle = tilePos + new Vector3(0, TileHeight / 2);
        Vector3 leftBottom = tilePos + new Vector3(-TileWidth, TileHeight);
        Vector3 leftMiddle = tilePos + new Vector3(-TileWidth, 0);
        //判断是否在左上三角形
        bool isLeftTop = PointinTriangle(leftTop, topMiddle, leftMiddle, p);
        if (isLeftTop)
        {
            Debug.Log("isLeftTop********************** " + x + "  " + (y - 1));
            return mapTileList[x, y - 1];
        }
        bool isRightTop = PointinTriangle(topMiddle, rightTop, rightMiddle, p);
        if (isRightTop)
        {
            Debug.Log("isRightTop******************************** " + (x - 1) + "  " + y);
            return mapTileList[x - 1, y];
        }
        bool isRightBottom = PointinTriangle(rightMiddle, rightBottom, bottomMiddle, p);
        if (isRightBottom)
        {
            Debug.Log("isRightBottom******************************" + x + "  " + y + 1);
            return mapTileList[x, y + 1];
        }
        bool isLeftBottom = PointinTriangle(leftMiddle, bottomMiddle, leftBottom, p);
        if (isLeftBottom)
        {
            Debug.Log("isLeftBottom******************************" + x + 1 + "  " + y);
            return mapTileList[x + 1, y];
        }
        return tile;

    }



    //public  bool isInsideTriangle(float  cx, float cy, int[] x, int[] y)
    //{
    //    float vx2 = cx - x[0];
    //    float vy2 = cy - y[0];
    //    float vx1 = x[1] - x[0];
    //    float vy1 = y[1] - y[0];
    //    float vx0 = x[2] - x[0];
    //    float vy0 = y[2] - y[0];
    //    float dot00 = vx0 * vx0 + vy0 * vy0;
    //    float dot01 = vx0 * vx1 + vy0 * vy1;
    //    float dot02 = vx0 * vx2 + vy0 * vy2;
    //    float dot11 = vx1 * vx1 + vy1 * vy1;
    //    float dot12 = vx1 * vx2 + vy1 * vy2;
    //    float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
    //    float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
    //    float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
    //    return ((u > 0) && (v > 0) && (u + v < 1));
    //}
    // Determine whether point P in triangle ABC
    public bool PointinTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
    {
        Vector3 v0 = C - A;
        Vector3 v1 = B - A;
        Vector3 v2 = P - A;

        //float dot00 = v0.Dot(v0);
        float dot00 = Vector3.Dot(v0, v0);
        //float dot01 = v0.Dot(v1);
        float dot01 = Vector3.Dot(v0, v1);
        //float dot02 = v0.Dot(v2);
        float dot02 = Vector3.Dot(v0, v2);
        //float dot11 = v1.Dot(v1);
        float dot11 = Vector3.Dot(v1, v1);
        //float dot12 = v1.Dot(v2);
        float dot12 = Vector3.Dot(v1, v2);

        float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

        float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
        if (u < 0 || u > 1) // if u out of range, return directly
        {
            return false;
        }

        float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
        if (v < 0 || v > 1) // if v out of range, return directly
        {
            return false;
        }

        return u + v <= 1;
    }
}
