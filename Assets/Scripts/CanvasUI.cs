using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CanvasUI : MonoBehaviour
{
    public Button btnShuriken;
    [Header("Game Complete UI")]
    public GameObject gameComplete;
    [Header("Main UI")]
    public Button btnPause;
    public Image imgCombo;
    public List<Sprite> spriteCombo = new List<Sprite>();
    [Header("Transition Settings")]
    public GameObject transition_Background;
    public List<GameObject> transition_In = new List<GameObject>();
    [Header(" ------- Pause Menu Refs -------")]
    [SerializeField] GameObject pauseMenu;
    public Text currentLevelText;
    #region MainUI 

    public void playSpriteCombo()
    {
        StartCoroutine(C_playSpriteCombo());
    }
    IEnumerator C_playSpriteCombo()
    {
        if (imgCombo.gameObject.active)
            imgCombo.gameObject.SetActive(false);
        imgCombo.gameObject.SetActive(true);
        int count = (GameManager.instance.shuriken.transform.GetChild(0).gameObject.GetComponent<Shuriken>().count - 1);
        imgCombo.sprite = spriteCombo[count];
        yield return null;
    }

    #endregion

    #region Transition
    public void _TransitionEnter()
    {
        StartCoroutine(C_TransitionEnter());
    }
    IEnumerator C_TransitionEnter()
    {
        transition_Background.GetComponent<Animator>().SetTrigger("enter");
        foreach(var value in transition_In)
        {
            value.GetComponent<Animator>().SetTrigger("enter");
        }
        yield return null;
    }
    #endregion


    #region Button Void 

    public void buttonCountinue()
    {
        StartCoroutine(C_buttonCountinue());
    }
    IEnumerator C_buttonCountinue()
    {
        PlayerPrefs.SetInt("Scene", PlayerPrefs.GetInt("Scene")+1);
        Debug.Log(PlayerPrefs.GetInt("Scene"));
        _TransitionEnter();
        yield return new WaitForSeconds(2f);
        GameManager.instance._DisableObjects();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(0);
    }
    public void buttonPause()
    {
        pauseMenu.SetActive(true);
    }
    public void buttonResume()
    {
        pauseMenu.SetActive(false);
    }
    public void buttonRestart()
    {
        StartCoroutine(C_buttonRestart());
    }
    IEnumerator C_buttonRestart()
    {
        _TransitionEnter();
        yield return new WaitForSeconds(2f);
        GameManager.instance._DisableObjects();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(0);
    }
    public void buttonQuit()
    {
        Application.Quit();
    }

    #endregion
}
