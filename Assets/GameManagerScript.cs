using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerScript : MonoBehaviour
{

    //�ǉ�
    public GameObject playerPrefab;
    //public GameObject GrayPrefab;

    int[,] map;//�ύX�B�񎟌��z��Ő錾
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��
    GameObject obj;


    // Start is called before the first frame update
    void Start()
    {
        //map�̐���
        map =new int[,] {
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 1,0,0,0,0 }
            };
        //�t�B�[���h�T�C�Y�̕ύX
        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)
            ];
        /*�m�F������폜
        Object instance = Instantiate(
            playerPrefab,
            new Vector3(0, 0, 0),
            Quaternion.identity
            );
        */

        //Debug.Log("Hello World");
        //�ړ��\���ǂ���
        //PrintArray();
        string debugText = "";

        //�ύX�B��dfor���œ񎟌��z��̏����o��
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
                if (map[y, x] == 1){
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0)+y-1, 0),
                        Quaternion.identity
                        );
                }
            }
            debugText += "\n";//���s
        }
        Debug.Log(debugText);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        int playerIndex = GetPlayerIndex();
    //        MoveNumber(1, playerIndex, playerIndex + 1);
    //        //PrintArray();
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        int playerIndex = GetPlayerIndex();
    //        MoveNumber(1, playerIndex, playerIndex - 1);
    //        //PrintArray();
    //    }
    //}



    Vector2Int GetPlayerIndex(){
        for (int y = 0; y < map.Length; y++){
            for (int x = 0; x < map.GetLength(0); x++){
                if (obj.tag==null){
                    continue;
                }
                if(obj.tag=="Player"){
                    if (field[y, x] == playerPrefab){
                        return new Vector2Int(x, y);
                    }
                }
            }
        }
        return new Vector2Int(-1,-1);
    }

    bool MoveNumber(Vector2Int number, Vector2Int moveFrom, Vector2Int moveTo){

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) + moveTo.y - 1, 0);

        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)){
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)){
            return false;
        }
        //if (map[moveTo] == 2){
        //    int velocity = moveTo - moveFrom;
        //    bool success = MoveNumber(2, moveTo, moveTo + velocity);
        //    if (!success){
        //        return false;
        //    }
        //}

        field[moveTo.y,moveTo.x] = field[moveFrom.y,moveFrom.x];
        field[moveFrom.y,moveFrom.x] = null;
        return true;
    }


}
