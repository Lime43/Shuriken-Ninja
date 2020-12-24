using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isHit;
    [SerializeField] GameObject target;
    [Header("Scene Settings")]
    public List<Vector3> scenePos = new List<Vector3>();
    NavMeshAgent agent;
    
    #region Script Enemy 
    void Enemy_Die()
    {
        StartCoroutine(C_Die());
    }
    IEnumerator C_Die()
    {
        #region Initialize variable
        Rigidbody ownRg = GetComponent<Rigidbody>();
        GameObject childrenSmoke = GameManager.instance.particleController.childrenSmoke;
        GameObject childrenLightning = GameManager.instance.particleController.childrenLightning;
        agent = GetComponent<NavMeshAgent>();
        GameObject mesh = gameObject.transform.GetChild(0).gameObject;
        GameObject modelCharacter = gameObject.transform.GetChild(1).gameObject;
        GameObject ragdoll = gameObject.transform.GetChild(2).gameObject;
        Rigidbody[] rgList = GetComponentsInChildren<Rigidbody>();
        BoxCollider[] bcList = GetComponentsInChildren<BoxCollider>();
        int rdInt;
        #endregion


        #region action 

        /* Sound Effect */
        rdInt = Random.Range(0, 2);
        AudioClip adiClip = AudioManager.instance.spark[rdInt];
        AudioManager.instance.PlaySound(this.gameObject,adiClip,.3f);
        /* Play effect */
        GameManager.instance.particleController.PlayEffect(childrenLightning,transform);

        /* Enable ragdoll */
        agent.enabled = mesh.active = modelCharacter.active = false;
        ragdoll.SetActive(true);
        float forceY = Random.Range(10,25f);
        float forceZ = Random.Range(5f ,10f);
        ownRg.AddForce(0f, forceY, forceZ, ForceMode.Impulse);


        #endregion


        #region End action
        yield return new WaitForSeconds(2f);
        ownRg.velocity = Vector3.zero;
        GameManager.instance.particleController.PlayEffect(childrenSmoke,ragdoll.transform.GetChild(1));
        ragdoll.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        #endregion
    }
    public void _Animator()
    {
        #region Initialize variable
        Animator _anim = GetComponent<Animator>();
        GameObject campFire = GameObject.FindGameObjectWithTag("Campfire");
        #endregion


        #region action
        if (campFire != null)
        {
            _anim.SetTrigger("isSit");
            Vector3 direc = campFire.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direc);
        }
        else
        {
            /* Anim walking */
            _anim.SetTrigger("isWalk");
            StartCoroutine(C_Walking());
        }
        #endregion

    }
    IEnumerator C_Walking()
    {
        #region variable
        GameObject ragdoll = gameObject.transform.GetChild(2).gameObject;
        agent = GetComponent<NavMeshAgent>();
        bool isLoop = true;
        float _time = .0f;
        #endregion

        #region action

        GameObject[] pathList = GameObject.FindGameObjectsWithTag("Path");
        foreach(var value in pathList)
        {
            if(!value.GetComponent<PathPosition>().isEnemyMove)
            {
                value.GetComponent<PathPosition>().isEnemyMove = true;
                target = value;
                break;
            }
        }
        while (isLoop)
        {
            #region Enemy Position Generate
            if (_time<1f)
            {
                _time += Time.deltaTime / 2f;
                agent.speed = 2f;
            }

            else
            {
                agent.speed = .6f;
            }

            #endregion



            #region enemy Move 
            if (agent.enabled)
            {
                agent.SetDestination(target.transform.position);
            }
            if (ragdoll.activeSelf) isLoop = false;
            #endregion
            yield return null;
        }
        #endregion
    }
    #endregion
}
