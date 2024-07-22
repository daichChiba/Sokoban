using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerScript : MonoBehaviour
{

    //�ǉ�
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject clearText;

    int[,] map;//�ύX�B�񎟌��z��Ő錾
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��



    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1280,720,false);

        //map�̐���
        map = new int[,] {
                { 0,0,3,0,0,0,0,3,0 },
                { 0,0,2,0,0,0,0,2,0 },
                { 1,0,0,0,0,0,0,0,0 }
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
            debugText += "\n";//���s
        }
        Debug.Log(debugText);
    }





    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //null��������^�O�𒲂ׂ����̗v�f�ֈڂ�
                if (field[y, x] == null)
                {
                    continue;
                }
                //null��������continue���Ă���̂ŁA
                //���̍s�ɂ��ǂ蒅���ꍇ��null�o�Ȃ����Ƃ��m��
                //�^�O�̊m�F���s��
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
        //�z��O�Q�Ɩh�~
        //Box�^�O�������Ă�����ċN����
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

        //�����N���A���Ă�����
        if (IsCleard())
        {
            Debug.Log("Clear");

            //�@�Q�[���I�u�W�F�N�g��SetActive���]�b�g���g���L����
            clearText.SetActive(true);

        }

    }

    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ��𔻒f
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
                //��ł������Ȃ�������������B��
                return false;
            }
        }
        //�������B���łȂ���Ώ����B��
        return true;
    }

}
