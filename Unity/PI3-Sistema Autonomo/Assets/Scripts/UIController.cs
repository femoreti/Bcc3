using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject _containerSpeed, _btnPause, _btnStart, _endScreen;
    public Image _imagePause, _imagePlay;

    public Text _endScreenStatsText;

    private bool _gamePause;


    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        onReset();
    }

    public void onReset()
    {
        _containerSpeed.SetActive(false);
        _btnStart.SetActive(true);
        _btnPause.SetActive(false);
        _endScreen.SetActive(false);
        _imagePlay.enabled = false;
        _imagePause.enabled = true;
    }

    public void OnClickStart()
    {
        Controller.Instance.onClickStart();

        _btnStart.SetActive(false);
        _containerSpeed.SetActive(true);
        _btnPause.SetActive(true);
    }

    public void onClickPause()
    {
        Controller c = Controller.Instance;

        _imagePlay.enabled = !_imagePlay.enabled;
        _imagePause.enabled = !_imagePause.enabled;

        /*if (!_gamePause)
        {
            //Pausa o programa
            
            if (c.gameRoutine != null)
                c.StopCoroutine(c.gameRoutine);
        }
        else
        {
            //UnPause
            c.gameRoutine = c.StartCoroutine(c.GameTime());
        }*/

        _gamePause = !_gamePause;

        c._gamePause = _gamePause;
    }

    public void onClickSpeed(int speed)
    {
        Controller.Instance._gameSpeed = speed;
    }

    public void onClickReset()
    {
        onReset();
        Controller.Instance.onReset();
    }

    public void onShowEndScreen(string textToShow)
    {
        _endScreen.SetActive(true);
        _endScreenStatsText.text = textToShow;
    }

    public void onCloseEndSreen()
    {
        onClickReset();

        _endScreen.SetActive(false);
    }
}
