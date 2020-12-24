using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    #region Singleton class : GameManager
    public static GameManager instance;
    private void Awake()
    {
        instance = this;   
    }
    #endregion

    // testing git hub abit 

    public PhysicMaterial slidingMat;
    [Header("References")]
    public BezierCurve bezierCurve;
    public ParticleController particleController;
    public GameObject shuriken;
    public List<GameObject> backGroundList = new List<GameObject>();
    public GameObject level;
    public CanvasUI canvasUI;
    [Header("Scene Settings")]
    public GameObject campfire;
    public GameObject rockObjs;
    public GameObject enemyObjs;
    public GameObject pathObjs;
    public GameObject barrelObjs;
    public GameObject stone;

    [Header("Disable Settings")]
    public GameObject levelDecorations;
    public GameObject background;
    public GameObject plane;
    void Start()
    {
        _InitGame();
    }
    void _InitGame()
    {
        #region variable
        GameObject obj;
        int Scene = PlayerPrefs.GetInt("Scene");
        #endregion 


        #region UI 
        canvasUI.currentLevelText.text = "Level " + (Scene+1);



        #endregion

        #region Init Background Script
        bool isActive = false;
        foreach(var value in backGroundList)
        {
            if (value.active) isActive = true;
        }
        if(!isActive)
        {
            int randomInt = Random.Range(0, backGroundList.Count);
            backGroundList[randomInt].SetActive(true);
        }
        #endregion


        #region Init Position Single Objects
        obj = campfire;
        obj.transform.position = obj.GetComponent<Campfire>().scenePos[Scene];
        ActiveObjectPosition(obj);
        #endregion



        #region Init Position Array Objects
        SetPositionArray(rockObjs, Scene);
        SetPositionArray(pathObjs, Scene);
        SetPositionArray(barrelObjs, Scene);
        SetPositionArray(stone, Scene);
        SetPositionArray(enemyObjs, Scene);
        #endregion



        #region NavMeshSurface baked Settings  - (Note : new Object should add script_Name: Nav Mesh Surface)
        NavMeshSurface[] listSurface = GameObject.FindObjectsOfType<NavMeshSurface>();
        for(int i=0;i<listSurface.Length;i++)
        {
            if(listSurface[i].isActiveAndEnabled)
                listSurface[i].BuildNavMesh();
        }
        #endregion
        
        #region Init Particle Controller

        particleController.dad = new GameObject("Particle Objects");
        particleController.childrenSmoke = new GameObject("SmokeEffect");
        particleController.childrenLightning = new GameObject("Lightning Effect");
        particleController.childrenRock = new GameObject("Rock Effect");
        /* Effect */
        GameObject smokeEffect = particleController.smokeEffect;
        GameObject lightningEffect = particleController.lightningStrikeEffect;
        GameObject rockEffect = particleController.rockEffect;

        /* Parent Objects */
        GameObject dad = particleController.dad;
        GameObject childrenSmoke = particleController.childrenSmoke;
        GameObject childrenLightning = particleController.childrenLightning;
        GameObject childrenRock = particleController.childrenRock;
        childrenSmoke.transform.SetParent(dad.transform);
        childrenLightning.transform.SetParent(dad.transform);
        childrenRock.transform.SetParent(dad.transform);
        /* Create Effect */
        int Count = 0;
        for (int i = 0; i < enemyObjs.transform.childCount; i++)
        {
            GameObject value = enemyObjs.transform.GetChild(i).gameObject;
            if (value.active) ++Count;
        }
        particleController.CreateEffect(smokeEffect,Count, childrenSmoke);
        particleController.CreateEffect(lightningEffect, Count, childrenLightning);
        Count = 0;
        for (int i = 0; i < stone.transform.childCount; i++)
        {
            GameObject value = enemyObjs.transform.GetChild(i).gameObject;
            if (value.active) ++Count;
        }
        particleController.CreateEffect(rockEffect, Count, childrenRock);
        #endregion


        #region Init Enemy Animator
        for(int i=0;i<enemyObjs.transform.childCount;i++)
        {
            GameObject value = enemyObjs.transform.GetChild(i).gameObject;
            if (value.activeSelf) value.GetComponent<Enemy>()._Animator();
        }
        #endregion 
    }

    public void _DisableObjects()
    {
        background.active = levelDecorations.active = plane.active = false;
        campfire.active = rockObjs.active = pathObjs.active = barrelObjs.active = stone.active = false;

        enemyObjs.active = false;
    }

    public void GameOver()
    {

        
    }
    public void CompleteGame()
    {



        // UI 
        canvasUI.btnPause.gameObject.SetActive(false);
        canvasUI.gameComplete.SetActive(true);
    }
    void SetPositionArray(GameObject obj,int Scene)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject value = obj.transform.GetChild(i).gameObject;
            switch (obj.name)
            {
                case "Rock":
                    value.transform.localPosition = value.GetComponent<Rock>().scenePos[Scene];
                    ActiveObjectPosition(value);
                    break;
                case "Enemy":
                    value.transform.localPosition = value.GetComponent<Enemy>().scenePos[Scene];
                    ActiveObjectPosition(value);
                    break;
                case "PathPosition":
                    value.transform.localPosition = value.GetComponent<PathPosition>().scenePos[Scene];
                    ActiveObjectPosition(value);
                    break;
                case "BoxObjs":
                    value.transform.localPosition = value.GetComponent<Box>().scenePos[Scene];
                    ActiveObjectPosition(value);
                    break;
                case "Stone":
                    value.transform.localPosition = value.GetComponent<Stone>().scenePos[Scene];
                    ActiveObjectPosition(value);
                    break;
            }
        }
    }
    void ActiveObjectPosition(GameObject obj)
    {
        if (obj.transform.localPosition == Vector3.zero) obj.SetActive(false);
        else obj.SetActive(true);
    }
}
