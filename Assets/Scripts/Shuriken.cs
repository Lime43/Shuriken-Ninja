using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shuriken : MonoBehaviour
{
    #region Header Control Points (need LineRenderer)
    [Header("Bezier Curve Settings")]
    public bool isDrawLine;
    public Transform[] controlP;
    public List<Vector3> setPos = new List<Vector3>();
    public LineRenderer line;
    public int numPoints = 50;
    #endregion 

    [Header("Shuriken Settings")]
    [SerializeField] GameObject ShurikenLine;
    MeshRenderer shurikenMesh;
    Vector3 startPos, endPos, direc;
    Vector3 currentFramePosition, lastFramePosition;
    bool isShurikenFly;
    bool isLoop;
    public int count;
    [Header("Scene Settings")]
    public List<Vector3> scenePos = new List<Vector3>();
    #region Implement Unity
    void Start()
    {
        shurikenMesh = GetComponent<MeshRenderer>();
    }
    void LateUpdate()
    {
        if(isShurikenFly)
        {
            ShurikenRotateXZ_axis();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !other.GetComponentInParent<Enemy>().isHit)
        {
            other.GetComponentInParent<Enemy>().isHit = true;
            other.SendMessage("Enemy_Die");

            if (isShurikenFly)
            {
                count++;
                GameManager.instance.canvasUI.playSpriteCombo();
            }
        }

        if (other.tag == "stone"&&isShurikenFly)
        {
            isShurikenFly = false;
            StoneHit(other.transform);
        }
    }

    #endregion
    
    #region void Shuriken Event Button 
    public void OnDragMoving()
    {
        bool isInteract = GameManager.instance.canvasUI.btnShuriken.interactable;
        if (!isInteract) return;
        StartCoroutine(C_OnDragMoving());
    }
    IEnumerator C_OnDragMoving()
    {
        while(isLoop)
        {
            /* Draw lineRender */
            GameManager.instance.bezierCurve._DrawCubicCurveInList(numPoints,controlP,line,isDrawLine,setPos);
            GameObject point2 = ShurikenLine.transform.GetChild(1).gameObject;
            GameObject point3 = ShurikenLine.transform.GetChild(2).gameObject;
            endPos = Input.mousePosition;
            direc = (endPos - startPos).normalized;
            direc = new Vector3(direc.x, 0f, 0f);
            if (direc.x > .2f || direc.x < -.2f)
            {
                if (direc != Vector3.zero)
                {
                    if (point2.transform.position.x < 5f && point2.transform.position.x > -5f)
                    {
                        point2.transform.Translate(direc * 5f * Time.deltaTime);
                        point3.transform.Translate(direc * 5f * Time.deltaTime);
                    }
                    else if (point2.transform.position.x > 5f)
                    {
                        point2.transform.position = new Vector3(4.9f, point2.transform.position.y, point2.transform.position.z);
                        point3.transform.position = new Vector3(4.9f, point3.transform.position.y, point3.transform.position.z);
                    }
                    else if (point2.transform.position.x < -5f)
                    {
                        point2.transform.position = new Vector3(-4.9f, point2.transform.position.y, point2.transform.position.z);
                        point3.transform.position = new Vector3(-4.9f, point3.transform.position.y, point3.transform.position.z);
                    }
                }
            }
            
            
            yield return null;
        }
    }
    
    public void PointerExit()
    {
        isLoop = false;
    }
    public void OnDragEnd()
    {
        bool isInteract = GameManager.instance.canvasUI.btnShuriken.interactable;
        if (!isInteract) return;
        isLoop = false;
        ShurikenLine.SetActive(false);
        StartCoroutine(C_OnDragEnd());
    }
    public void OnDragStart()
    {
        #region variable
        GameObject fxEffect = transform.GetChild(0).gameObject;
        bool isInteract = GameManager.instance.canvasUI.btnShuriken.interactable;
        if (!isInteract) return;
        #endregion
        if (!isLoop)
        {
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            gameObject.SetActive(true);
            fxEffect.active = ShurikenLine.active = shurikenMesh.enabled = true;
            GameManager.instance.bezierCurve._DrawCubicCurveInList(numPoints, controlP, line, isDrawLine, setPos);
            startPos = endPos = Input.mousePosition;
        }
        isLoop = true;
    }
    IEnumerator C_OnDragEnd()
    {
        #region variable 
        GameObject fxEffect = transform.GetChild(0).gameObject;
        isShurikenFly = true;
        float t = 0;
        count = 0;
        #endregion

        #region action 
        while (t < 1f)
        {
            GameManager.instance.canvasUI.btnShuriken.interactable = false;
            t += Time.deltaTime / 2f;
            /* Rotate Y Shuriken */
                transform.RotateAround(transform.position, Vector3.up, 720f * Time.deltaTime);
            /* ---- */
            Vector3 setPos = GameManager.instance.bezierCurve._CaculateCubicCurve
                (t, controlP[0].position, controlP[1].position, controlP[2].position, controlP[3].position);
            transform.position = setPos;
            if (!isShurikenFly) yield break;
            yield return null;
        }
        if(t > 1f)
        {
            GameManager.instance.canvasUI.btnShuriken.interactable = true;
            fxEffect.active = isShurikenFly = false;
        }
        #endregion 
    }

    #endregion

    #region Shuriken Script

    void StoneHit(Transform stone)
    {
        // Set Parent
        transform.localPosition = stone.transform.localPosition;
        // Particle Effect
        GameObject rockEffect = GameManager.instance.particleController.childrenRock;
        GameManager.instance.particleController.PlayEffect(rockEffect, stone);
        // Game Over 


    }


    #endregion 

    void ShurikenRotateXZ_axis()
    {
        currentFramePosition = transform.position;
        direc = currentFramePosition - lastFramePosition;
        if (direc != Vector3.zero)
        {
            Quaternion targetRotate = Quaternion.LookRotation(direc);
            Quaternion thisRotate = transform.rotation;
            thisRotate.x = Mathf.Lerp(thisRotate.x, targetRotate.x, 8f * Time.deltaTime);
            thisRotate.z = Mathf.Lerp(thisRotate.z, targetRotate.z, 8f * Time.deltaTime);
            transform.rotation = thisRotate;
        }
        lastFramePosition = transform.position;
    }
}
