using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atendente : MonoBehaviour
{
    public string _myName;
    public bool _isBusy, _isChanging;
    public Posto _postoAtual, _postoFuturo;
    public int _totalTimeToChange;

    private int _arrivalTurn;

    private void SetArrivalTurn()
    {
        _arrivalTurn = Controller.Instance._currentWorldTurn + _totalTimeToChange;
    }

    public void onCheckIfArrive()
    {
        if (!_isChanging)
            return;

        if(_arrivalTurn >= Controller.Instance._currentWorldTurn)
        {
            //Cheguei caralho, me coloca pra trabalhar no meu posto
            _postoFuturo.setAtendente(this);

            Debug.Log("Cheguei caralho, " + _myName + " posto " + _postoAtual._myType);
            _isChanging = false;
        }
    }

    public void CheckTroca() {
        //Debug.Log("check troca atendente " + _myName);

        Posto postoFuturo = null;

        if (_isChanging || _isBusy || _totalTimeToChange == 0)
            return;
        
        float tempoFila = 0;

        foreach (var posto in Controller.Instance._gerenciadorDePosto.postos)
        {
            if (posto.temAtendente || posto._atendenteVindo != null || posto == _postoAtual || posto._minhaFila._userInside.Count == 0)
                continue;
            
            // Conta total de atendentes nos postos de mesma letra que o item
            int countAtendentes = 0;
            foreach (var item in Controller.Instance._gerenciadorDePosto.postos) {
                if (item.letra == posto.letra) {
                    if (item.temAtendente)
                        countAtendentes++;
                }
            }

            float a = posto._minhaFila._userInside.Count * posto.turnos;
            float b = 0;
            if (countAtendentes == 1)
            {
                b = ((_postoAtual._minhaFila._userInside.Count * _postoAtual.turnos) + (_totalTimeToChange * Controller.Instance.multiplicadorDoTempoDeTroca)*1.65f);
            }
            else
            {
                b = ((_postoAtual._minhaFila._userInside.Count * _postoAtual.turnos) + (_totalTimeToChange * Controller.Instance.multiplicadorDoTempoDeTroca)*0.5f);
            }

            if (a > b) //Verifica o posto atual
            {
                //Considerar se compensa
                if (tempoFila < a)
                {
                    postoFuturo = posto;
                    tempoFila = a;
                }
            }
        }

        if(postoFuturo != null)
        {
            // Pensar na condicao de troca
            this._postoFuturo = postoFuturo;

            // tempoFilaFutura / 2 + tempoTroca < tempoFilaFutura
            Troca();
        }
    }

    public void Troca()
    {
        Debug.Log("atendente troca " + _myName + " posto " + _postoAtual._myType + " -> " + _postoFuturo._myType);
        this._postoAtual.leaveAtendente();
        this._postoAtual = null;

        this._postoFuturo._atendenteVindo = this;
        //this._postoAtual = this._postoFuturo;

        SetArrivalTurn();
        _isChanging = true;
    }
}
