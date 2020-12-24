using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InitializePosition : MonoBehaviour
{
    [SerializeField] bool isSure;
    [Header("Move Level Settings")]
    [SerializeField] int WhichLevel;
    public bool Move;
    [Header("Generate Level Settings")]
    public bool isGenerate;
    [SerializeField] int levelGenerate, levelGenerateMax;
    [Header("Single Objects Settings")]
    public GameObject campfire;
    [Header("Array Objects Settings")]
    #region Parent Objects
    public GameObject enemyObjs;
    public GameObject RockObjs;
    public GameObject PathObjs;
    public GameObject barrelObjs;
    public GameObject Stone;
    #endregion

    #region List  Array
    public List<GameObject> enemyList;
    public List<GameObject> rockList;
    public List<GameObject> barrelList;
    public List<GameObject> pathList;
    public List<GameObject> stoneList;
    #endregion

    #region Implement Unity /* Edit in void Update() in Implement Unity if have new Objects */

    void Update()
    {
        /* Adding List to Array */
        AddingListArray(enemyObjs,enemyList);
        AddingListArray(RockObjs, rockList);
        AddingListArray(PathObjs, pathList);
        AddingListArray(barrelObjs, barrelList);
        AddingListArray(Stone, stoneList);
        /* ------------------ Generate Level ------------------ */
            GenerateLevel();
        /* ------------------------------------------------------ */




        /* MoveLevel Scene */
            MoveLevel();
    }


    #endregion
    
    
    
    /* There are two things in This script : 1. Generate new Level , 2. Move to level */
    #region Generate Level Script
    void GenerateLevel()
    {
        if(isGenerate)
        {
            Debug.Log("Are you sure ?");
            Debug.Log("Tick on isSure boolean to Generate");
            /* Condition for List and SingleObjects ?? Maybe adding more level ? */
            _ConditionForSingle(levelGenerate);
            _ConditionForArray(levelGenerate);
            if (isSure && levelGenerate <= levelGenerateMax)
            {
                Debug.Log("Generate Level !!!");
                _SavePosition(levelGenerate - 1);
            }
            if (!isSure && levelGenerate > levelGenerateMax)
            {
                Debug.Log("Cannot generate Level : Max Level is  :" + levelGenerateMax);
            }
        }
    }
    

    /* Edit here if you have new objects */
    void _SavePosition(int levelGen)
    {
        // Single Object 
        GameObject obj = campfire;
        if(obj!=null)
        {
            if (obj.active) obj.GetComponent<Campfire>().scenePos[levelGen] = obj.transform.position;
        }

        // Array Object
        foreach(var value in enemyList)
        { if (value.active) value.GetComponent<Enemy>().scenePos[levelGen] = value.transform.localPosition; }
        foreach(var value in rockList)
        { if (value.active) value.GetComponent<Rock>().scenePos[levelGen] = value.transform.localPosition; }
        foreach(var value in pathList)
        { if (value.active) value.GetComponent<PathPosition>().scenePos[levelGen] = value.transform.localPosition; }
        foreach(var value in barrelList)
        { if (value.active) value.GetComponent<Box>().scenePos[levelGen] = value.transform.localPosition; }
        foreach(var value in stoneList)
        { if (value.active) value.GetComponent<Stone>().scenePos[levelGen] = value.transform.localPosition; }
    }
    void _ConditionForSingle(int levelGen)
    {
        /* Campfire Size List ScenePos */
            List<Vector3> list = campfire.GetComponent<Campfire>().scenePos;
            AddList(list, list.Count,levelGen);


    }
    void _ConditionForArray(int levelGen)
    {
                                /* Added List here to increase Size List  */
        foreach (var value in enemyList) 
        {
            List<Vector3> list = value.GetComponent<Enemy>().scenePos;
            AddList(list, list.Count, levelGen);
        }
        foreach (var value in rockList)
        {
            List<Vector3> list = value.GetComponent<Rock>().scenePos;
            AddList(list, list.Count, levelGen);
        }
        foreach (var value in pathList)
        {
            List<Vector3> list = value.GetComponent<PathPosition>().scenePos;
            AddList(list, list.Count, levelGen);
        }
        foreach(var value in barrelList)
        {
            List<Vector3> list = value.GetComponent<Box>().scenePos;
            AddList(list, list.Count, levelGen);
        }
        foreach(var value in stoneList)
        {
            List<Vector3> list = value.GetComponent<Stone>().scenePos;
            AddList(list, list.Count, levelGen);
        }
    }
    void AddList(List<Vector3> list ,int c,int LevelGen)
    {
        for (int i = c; i < LevelGen; i++)
        {
            list.Add(Vector3.zero);
        }
    }
    #endregion




    #region Move Level Script
    
    void MoveLevel()
    {
        if(Move)
        {
            Debug.Log("You sure you want to move ? Map that generate will not be save Position");
            Debug.Log("Tick on the isSure to Move Level");
            if (isSure)
            {
                int Scene = WhichLevel - 1;
                PlayerPrefs.SetInt("Scene",Scene);
                Debug.Log("Is Move to Level " + WhichLevel);
                /* Init Position Objects */
                PositionSingleObject();
                PositionArrayObject();
            } 
        }
    }
    
    
    void ActivateObject(GameObject obj)
    {
        if (obj.transform.localPosition == Vector3.zero) obj.SetActive(false);
        else obj.SetActive(true);
    }

                                                    /* Edit here if you have new Object */

        /* is Array  Object ? */
    void PositionArrayObject()
    {
        List<GameObject> list;
        #region rockList
                 list = rockList;
                foreach (var value in list)
                {
                    value.transform.localPosition = value.GetComponent<Rock>().scenePos[WhichLevel - 1];
                    ActivateObject(value);
                }
        #endregion


        #region enemyList
                list = enemyList;
                foreach (var value in list)
                {
                    value.transform.localPosition = value.GetComponent<Enemy>().scenePos[WhichLevel - 1];
                    ActivateObject(value);
                }
        #endregion


        #region pathList
                list = pathList;
                foreach (var value in list)
                {
                    value.transform.localPosition = value.GetComponent<PathPosition>().scenePos[WhichLevel - 1];
                    ActivateObject(value);
                }
        #endregion

        #region box 
                list = barrelList;
                foreach (var value in list)
                {
                    value.transform.localPosition = value.GetComponent<Box>().scenePos[WhichLevel - 1];
                    ActivateObject(value);
                }

        #endregion


        #region stoneList 
        list = stoneList;
        foreach (var value in list)
        {
            value.transform.localPosition = value.GetComponent<Stone>().scenePos[WhichLevel - 1];
            ActivateObject(value);
        }
        #endregion

    }

    /* is Single  Object ? */
    void PositionSingleObject()
    {
        #region Campfire Object
                GameObject obj = campfire;
                obj.transform.position = obj.GetComponent<Campfire>().scenePos[WhichLevel-1];
                ActivateObject(obj);
        #endregion
    }

#endregion


    void AddingListArray(GameObject parent, List<GameObject> list)
    {
        list.Clear();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject obj = parent.transform.GetChild(i).gameObject;
            list.Add(obj);
        }
    }
}
