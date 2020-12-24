using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("Enviroment Effect Objects")]
    public GameObject fireFlies;
    public GameObject leavesDay;
    public GameObject leavesNoon;
    public GameObject rainyDay;
    [Header("Hit Effect")]
    public GameObject smokeEffect;
    public GameObject bloodEffect;
    public GameObject lightningStrikeEffect;
    public GameObject rockEffect;
    [Header("Parent Settings")]
    public GameObject dad;
    public GameObject childrenSmoke;
    public GameObject childrenLightning;
    public GameObject childrenRock;
    public void PlayEffect(GameObject childrenEffect, Transform targetPosition)
    {
        StartCoroutine(C_PlayEffect(childrenEffect,1f,targetPosition));
    }
    IEnumerator C_PlayEffect(GameObject childrenEffect,float delayTime,Transform targetPosition)
    {
        #region Initialize variable
        GameObject value = null;
        bool isActive = true;
        #endregion

        #region action
        for (int i=0;i< childrenEffect.transform.childCount;i++)
        {
            value = childrenEffect.transform.GetChild(i).gameObject;
            if(!value.activeSelf)
            {
                isActive = false;
                value.SetActive(true);
                /* Set position to target Position */
                value.transform.SetParent(targetPosition);
                value.transform.localPosition = Vector3.zero;
                break;
            }
            else
            {
                CreateEffect(value,1, childrenEffect);
                yield return C_PlayEffect(childrenEffect, delayTime, targetPosition);
            }
        }
        if (!isActive)
        {
            value.transform.SetParent(childrenEffect.transform);
            yield return new WaitForSeconds(delayTime);
                value.transform.SetParent(childrenEffect.transform);
                value.SetActive(false);
                yield break;
        }
        else yield return C_PlayEffect(childrenEffect, delayTime, targetPosition);
        #endregion
    }
    public void CreateEffect(GameObject prefab,int Max,GameObject hisParent)
    {
        for(int i=0;i<Max;i++)
        {
            GameObject kid = Instantiate(prefab,hisParent.transform);
            kid.SetActive(false);
        }
    }
}