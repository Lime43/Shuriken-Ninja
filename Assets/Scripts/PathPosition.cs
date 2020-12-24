using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPosition : MonoBehaviour
 {
    public bool isEnemyMove;
    [Header("MoveZ Settings")]
    [Range(0, 5)] [SerializeField] private float _amplitudeZ;
    [Range(0, 2)] [SerializeField] private float _frequencyZ;
    [SerializeField] bool moveZ;
    [Header("MoveX Settings")]
    [Range(0,5)] [SerializeField] private float _amplitudeX;
    [Range(0,2)] [SerializeField] private float _frequencyX;
    [SerializeField] bool moveX;
    [Header("Scene Settings")]
    public List<Vector3> scenePos = new List<Vector3>();
    private void Start()
    {
        _PathMove();
    }
    void _PathMove()
    {
        StartCoroutine(C_PathMove());
    }
    IEnumerator C_PathMove()
    {
        bool isLoop = true;
        while(isLoop)
        {
            float cosX = transform.position.x, sinY = transform.position.y, tanZ = transform.position.z;
            if(moveX)
                cosX = Mathf.Cos(Time.time * _frequencyX) * _amplitudeX;
            if (moveZ)
                tanZ = Mathf.Tan(Time.time * _frequencyZ) * _amplitudeZ;
            transform.position = new Vector3(cosX,sinY,tanZ);
            /* If complete Game - stop the Loop */
            yield return null;
        }
    }
    
}
