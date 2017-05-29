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
    public Sprite _imagePause, _imagePlay; 

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
        _btnPause.GetComponent<Image>().sprite = _imagePause;
        //_imagePlay.enabled = false;
        //_imagePause.enabled = true;
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
        
        _btnPause.GetComponent<Image>().sprite = (_gamePause) ? _imagePause : _imagePlay;

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
