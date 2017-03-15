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
    private Fila _minhaFila;

    void Awake()
    {
        //INSTANCE = this;
    }

    void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = gameObject.name;

        _minhaFila = Controller.Instance._filaManager.AchaFila((FilaType)Enum.Parse(typeof(FilaType), letra.ToString()));
    }

    public void PassaTurno()
    {
        if (!temAtendente)
            return;
        //Debug.Log("temAtendente");
        if (_userSendoAtendido != null || _minhaFila._userInside.Count > 0)
            _atendimentoRestante--;

        

        if (_atendimentoRestante <= 0) // verifica se terminou de atender o usuario e passa ele para proxima fila
        {
            if (_userSendoAtendido != null)
            {
                //Debug.Log(_userSendoAtendido.userStats.name + " proxima etapa");
                _userSendoAtendido.ProximaFila();
                _userSendoAtendido = null;
            }
            if(_minhaFila._userInside.Count > 0)
            {
                //Debug.Log("_minhaFila._userInside.Count > 0");

                //chama o proximo da sua fila
                _userSendoAtendido = _minhaFila.RetiraProximo();
                _userSendoAtendido.transform.parent = this.transform;
                _userSendoAtendido.transform.position = new Vector3(this.transform.position.x + 100, this.transform.position.y, this.transform.position.z);
                _atendimentoRestante = turnos;
            }
        }
    }
}
