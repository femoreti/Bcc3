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
    public Atendente atendente, _atendenteVindo;
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
        //_minhaFila = Controller.Instance._filaManager.AchaFila(_myType);
    }

    public void PassaTurno()
    {
        if (!temAtendente)
            return;

        if (_userSendoAtendido != null || _minhaFila._userInside.Count > 0)
        {
            atendente._isBusy = true;
            _atendimentoRestante--;
        }

        if (_atendimentoRestante <= 0) // verifica se terminou de atender o usuario e passa ele para proxima fila
        {
            if (_userSendoAtendido != null)
            {
                _userSendoAtendido.ProximaFila();
                _userSendoAtendido = null;

                atendente._isBusy = false;
            }

            atendente.CheckTroca();

            if(atendente != null)
                ChamaFila();
        }
    }

    public void ChamaFila()
    {
        if (_minhaFila._userInside.Count > 0) //se a fila contem usuarios ela retira o proximo de la
        {
            //chama o proximo da sua fila
            _userSendoAtendido = _minhaFila.RetiraProximo();
            _userSendoAtendido.transform.parent = this.transform;
            Vector3 userNewPos = new Vector3(this.transform.position.x + GetComponent<RectTransform>().sizeDelta.x / 2 + 5f, this.transform.position.y, this.transform.position.z);

            if (_userSendoAtendido.transform.localPosition.x > 0) //Coloca em frente o posto
            {
                NightTween.Create(_userSendoAtendido.gameObject, (1f / Controller.Instance._gameSpeed) / 2f, new NightTweenParams()
                .Property(NTPropType.transformPosition, userNewPos)
                );
            }
            else
            {
                _userSendoAtendido.transform.position = userNewPos;
            }

            _atendimentoRestante = turnos;
        }
    }

    /// <summary>
    /// Entrando atendente
    /// </summary>
    /// <param name="a"></param>
    public void setAtendente(Atendente a)
    {
        //GetComponent<Image>().enabled = true;
        //GetComponent<Image>().color = Color.yellow;
        temAtendente = true;

        if (a.tween)
        {
            Destroy(a.tween);
            a.tween = null;
        }

        a.transform.position = transform.FindChild("ref").position;
        atendente = a;
        a._postoAtual = this;
        _atendenteVindo = null;

        //Debug.Log("posto: " + _myType + " recebeu atendente " + a._myName);
    }

    /// <summary>
    /// Saindo atendente
    /// </summary>
    public void leaveAtendente()
    {
        temAtendente = false;
        atendente = null;
        //GetComponent<Image>().enabled = false;
        //GetComponent<Image>().color = Color.red;
    }

    public void OnReset()
    {
        temAtendente = false;
        atendente = null;
        _userSendoAtendido = null;
        _atendimentoRestante = 0;
        _atendenteVindo = null;
    }
}
