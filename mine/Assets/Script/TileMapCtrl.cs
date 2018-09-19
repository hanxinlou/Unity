using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMapCtrl : MonoBehaviour
{
    public GameObject success;
    public GameObject failed;
    public GameObject Key;
    public Tilemap tilemap;
    public TileBase mine0;
    public TileBase flag;
    public TileBase[] tiles;
    public TileBase hide;
    public TileBase key;
    public TileBase door;
    public TileBase live;
    public Text text;
    int height = 10;
    int width = 100;
    int heart = 3;
    int keynum = 0;
    // Use this for initialization
    void Start()
    {
        initmine();
    }

    // Update is called once per frame
    enum Status
    {
        hide, show, flag
    }
    List<List<int>> A;
    List<List<Status>> B;
    //int[,] A = new int(height, 20);
    //Status[,] B = new Status[10, 20];

    //递归显示空格子周围的信息同时修改格子状态（显示、隐藏等）----------------------------------
    private void Display(Vector3Int cellPosition)
    {
        if (B[cellPosition.y][cellPosition.x] != Status.hide) return;
        if (A[cellPosition.y][cellPosition.x] == 0)
        {
            B[cellPosition.y][cellPosition.x] = Status.show;
            tilemap.SetTile(cellPosition, tiles[A[cellPosition.y][cellPosition.x]]);
            if (cellPosition.x - 1 >= 0)
                Display(cellPosition + new Vector3Int(-1, 0, 0));
            if (cellPosition.y - 1 >= 0)
                Display(cellPosition + new Vector3Int(0, -1, 0));
            if (cellPosition.y + 1 < height)
                Display(cellPosition + new Vector3Int(0, +1, 0));
            if (cellPosition.x + 1 < width)
                Display(cellPosition + new Vector3Int(+1, 0, 0));
        }
        else if(A[cellPosition.y][cellPosition.x]>0)
        {
            tilemap.SetTile(cellPosition, tiles[A[cellPosition.y][cellPosition.x]]);
            B[cellPosition.y][cellPosition.x] = Status.show;
        }
        else if (A[cellPosition.y][cellPosition.x] ==-2)
        {
            tilemap.SetTile(cellPosition, door);
            B[cellPosition.y][cellPosition.x] = Status.show;
        }
        else if (A[cellPosition.y][cellPosition.x]==-3)
        {
            tilemap.SetTile(cellPosition, key);
            B[cellPosition.y][cellPosition.x] = Status.show;
        }
        else if (A[cellPosition.y][cellPosition.x] == -4)
        {
            tilemap.SetTile(cellPosition, live);
            B[cellPosition.y][cellPosition.x] = Status.show;
        }
    }

    void Update()
    {
        text.text = heart.ToString();//显示生命值
        //鼠标左键单击事件---------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            //转换坐标
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
            if (cellPosition.x < 0 || cellPosition.y < 0 || cellPosition.x >= width || cellPosition.y >= height)
                return;
            if (B[cellPosition.y][cellPosition.x] == Status.flag)
                return;
            if (A[cellPosition.y][cellPosition.x] == -1)
            {
                tilemap.SetTile(cellPosition, mine0);//显示地雷
                B[cellPosition.y][cellPosition.x] = Status.show;
                heart--;//生命值减少
                //生命值减少至0则游戏失败弹出失败界面---------------
                if (heart == 0)
                {
                    failed.SetActive(true);
                }
            }
            //点击拾取生命值---------------------
            else if (A[cellPosition.y][cellPosition.x] == -4 && B[cellPosition.y][cellPosition.x] == Status.show)
            {
                heart++;
                tilemap.SetTile(cellPosition, tiles[Minenum(cellPosition.y, cellPosition.x)]);
            }
            //点击拾取钥匙------------------------
            else if (A[cellPosition.y][cellPosition.x] == -3 && B[cellPosition.y][cellPosition.x] == Status.show)
            {
                tilemap.SetTile(cellPosition, tiles[Minenum(cellPosition.y, cellPosition.x)]);
                keynum++;
                Key.SetActive(true);
            }
            //有钥匙的情况下点击大门弹出游戏通关界面--------------
            else if (A[cellPosition.y][cellPosition.x] == -2 && B[cellPosition.y][cellPosition.x] == Status.show&&keynum>0)
            {
                keynum--;
                Key.SetActive(false);
                success.SetActive(true);
            }
            else 
            {
                //tilemap.SetTile(cellPosition, tiles[A[cellPosition.x, cellPosition.y]]);
                //B[cellPosition.x, cellPosition.y] = 1;
                Display(cellPosition);
            }

        }
        //鼠标右键单击事件-----------------------
        else if (Input.GetMouseButtonDown(1))
        {
            //转换坐标----------------------
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
            if (cellPosition.x < 0 || cellPosition.y < 0 || cellPosition.x >= width || cellPosition.y >= height)
                return;
            //标记地雷------------------------------
            if (B[cellPosition.y][cellPosition.x] == Status.hide)
            {
                B[cellPosition.y][cellPosition.x] = Status.flag;
                tilemap.SetTile(cellPosition, flag);
            }
            //再次点击标记取消地雷标记------------------------
            else if (B[cellPosition.y][cellPosition.x] == Status.flag)
            {
                B[cellPosition.y][cellPosition.x] = Status.hide;
                tilemap.SetTile(cellPosition, hide);
            }
        }
    }
    //计算当前坐标周围的地雷数--------------------------------------------
    public int Minenum(int i,int j)
    {
        A[i][j]=0;
        if (i - 1 >= 0 && j - 1 >= 0 && A[i - 1][j - 1] == -1)
            A[i][j]++;
        if (i - 1 >= 0 && A[i - 1][j] == -1)
            A[i][j]++;
        if (i - 1 >= 0 && j + 1 < width && A[i - 1][j + 1] == -1)
            A[i][j]++;
        if (j - 1 >= 0 && A[i][j - 1] == -1)
            A[i][j]++;
        if (j + 1 < width && A[i][j + 1] == -1)
            A[i][j]++;
        if (i + 1 < height && j - 1 >= 0 && A[i + 1][j - 1] == -1)
            A[i][j]++;
        if (i + 1 < height && A[i + 1][j] == -1)
            A[i][j]++;
        if (i + 1 < height && j + 1 < width && A[i + 1][j + 1] == -1)
            A[i][j]++;
        return A[i][j];
    }
    //初始化扫雷区域--------------------------------------------------
    private void initmine()
    {
        A = new List<List<int>>();
        B = new List<List<Status>>();
        int i, j;
        int temp1,temp2;
        for (i = 0; i < height; i++)
        {
            List<int> vs = new List<int>();
            List<Status> tb = new List<Status>();

            for (j = 0; j < width; j++)
            {
                tb.Add(Status.hide);
                temp1 = Random.Range(0, 100);
                temp2= Random.Range(0, 100);
                int difficulty;//难度系数
                if (PlayData.difficulty == 0)
                    difficulty = 10;
                else if (PlayData.difficulty == 1)
                    difficulty = 20;
                else
                    difficulty = 30;
                if (temp2 < 1)
                {
                    vs.Add(-4);
                }
                else
                { 
                    if (temp1 < difficulty)
                        vs.Add(-1);
                    else
                        vs.Add(0);
                }
                tilemap.SetTile(new Vector3Int(j, i, 0), hide);
            }
            A.Add(vs);
            B.Add(tb);
        }
        //计算每个格子周围的地雷数并记录----------------------------------
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                if (A[i][j] != -1 && A[i][j] != -4)
                {
                    if (i - 1 >= 0 && j - 1 >= 0 && A[i - 1][j - 1] == -1)
                        A[i][j]++;
                    if (i - 1 >= 0 && A[i - 1][j] == -1)
                        A[i][j]++;
                    if (i - 1 >= 0 && j + 1 < width && A[i - 1][j + 1] == -1)
                        A[i][j]++;
                    if (j - 1 >= 0 && A[i][j - 1] == -1)
                        A[i][j]++;
                    if (j + 1 < width && A[i][j + 1] == -1)
                        A[i][j]++;
                    if (i + 1 < height && j - 1 >= 0 && A[i + 1][j - 1] == -1)
                        A[i][j]++;
                    if (i + 1 < height && A[i + 1][j] == -1)
                        A[i][j]++;
                    if (i + 1 < height && j + 1 < width && A[i + 1][j + 1] == -1)
                        A[i][j]++;
                }
            }
        }
        //随机产生一个门的坐标并保证不和生命以及地雷用同一个坐标--------------
        int doorx, doory;
        do
        {
            doorx = Random.Range(0, 10);
            doory = Random.Range(25, 60);
        } while ( A[doorx][doory] == -1 || A[doorx][doory] == -4);
        A[doorx][doory] = -2;
        //随机产生一个钥匙的坐标并保证不和生命、门以及地雷用同一个坐标--------------
        int keyx, keyy;
        do
        {
            keyx = Random.Range(0, 10);
            keyy = Random.Range(20, 80);
        } while (doorx == keyx && doory == keyy||A[doorx][doory]==-1|| A[doorx][doory] == -4);
        A[keyx][keyy] = -3;
    }
    private void OnMouseDown()
    {

    }
}
