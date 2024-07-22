using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerScript : MonoBehaviour
{

    //追加
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject clearText;

    int[,] map;//変更。二次元配列で宣言
    GameObject[,] field;//ゲーム管理用の配列



    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1280,720,false);

        //mapの生成
        map = new int[,] {
                { 0,0,3,0,0,0,0,3,0 },
                { 0,0,2,0,0,0,0,2,0 },
                { 1,0,0,0,0,0,0,0,0 }
            };
        //フィールドサイズの変更
        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)
            ];
        /*確認したら削除
        Object instance = Instantiate(
            playerPrefab,
            new Vector3(0, 0, 0),
            Quaternion.identity
            );
        */

        //Debug.Log("Hello World");
        //移動可能かどうか
        string debugText = "";

        //変更。二重for文で二次元配列の情報を出力
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
                //if (map[y,x]==0){
                //    field[y, x] = Instantiate(
                //    playerPrefab,
                //    new Vector3(x, y, 0),
                //    Quaternion.identity
                //    );
                //}
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) + y, 0),
                        Quaternion.identity
                        );
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }

                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
                        new Vector3(x, map.GetLength(0) + y, 0),
                        Quaternion.identity
                        );
                }

            }
            debugText += "\n";//改行
        }
        Debug.Log(debugText);
    }





    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //nullだったらタグを調べず次の要素へ移る
                if (field[y, x] == null)
                {
                    continue;
                }
                //nullだったらcontinueしているので、
                //この行にたどり着く場合はnull出ないことが確定
                //タグの確認を行う
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }

            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {


        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            return false;
        }
        //配列外参照防止
        //Boxタグを持っていたら再起処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }




        Vector3 moveToPosition =new Vector3(
            moveTo.x,map.GetLength(0)-moveTo.y,0
            );

        field[moveFrom.y,moveFrom.x].GetComponent<Move>().MoveTo( moveToPosition );

        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(1, 0));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber(
                playerIndex,
                playerIndex + new Vector2Int(0, 1));
        }

        //もしクリアしていたら
        if (IsCleard())
        {
            Debug.Log("Clear");

            //　ゲームオブジェクトのSetActiveメゾットを使い有効化
            clearText.SetActive(true);

        }

    }

    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }

            }
        }
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }
        //条件未達成でなければ条件達成
        return true;
    }

}
