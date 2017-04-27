using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Recebe os dados do arquivo Setup.txt
public class Posto: MonoBehaviour {
    //public static Posto INSTANCE;
    public char letra;
    public int turnos;
    public bool temAtendente;

    private User _userSendoAtendido;
    private int _atendimentoRestante;
    public FilaType _myType;
    public Fila _minhaFila;

    void Awake()
    {
        //INSTANCE = this;
    }

    void Start()
    {
        transform.FindChild("Text").GetComponent<Text>().text = gameObject.name;
        //_minhaFila = Controller.Instance._filaManager.AchaFila(_myType);
    }

    public void PassaTurno()
    {
        if (!temAtendente)
            return;
        
        if (_userSendoAtendido != null || _minhaFila._userInside.Count > 0)
            _atendimentoRestante--;

        if (_atendimentoRestante <= 0) // verifica se terminou de atender o usuario e passa ele para proxima fila
        {
            if (_userSendoAtendido != null)
            {
                _userSendoAtendido.ProximaFila();
                _userSendoAtendido = null;
            }
            if(_minhaFila._userInside.Count > 0) //se a fila contem usuarios ela retira o proximo de la
            {
                //chama o proximo da sua fila
                _userSendoAtendido = _minhaFila.RetiraProximo();
                _userSendoAtendido.transform.parent = this.transform;
                Vector3 userNewPos = new Vector3(this.transform.position.x + GetComponent<RectTransform>().sizeDelta.x / 2 + 5f, this.transform.position.y, this.transform.position.z);



                if (_userSendoAtendido.transform.localPosition.x > 0)
                {
                    NightTween.Create(_userSendoAtendido.gameObject, (1f / Controller.Instance._gameSpeed) / 2f, new NightTweenParams()
                    .Property(NTPropType.transformPosition, userNewPos)
                    );
                }
                else
                {
                    Debug.Log("valor0");
                    _userSendoAtendido.transform.position = userNewPos;
                }

                _atendimentoRestante = turnos;
            }
        }
    }

    public bool setAtendente
    {
        get
        {
            return temAtendente;
        }
        set
        {
            GetComponent<Image>().enabled = value;
            temAtendente = value;
        }
    }
}
