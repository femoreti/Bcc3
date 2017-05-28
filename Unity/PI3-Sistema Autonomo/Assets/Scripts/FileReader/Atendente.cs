using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atendente : MonoBehaviour
{
    public string _myName;
    public bool _isBusy, _isChanging;
    public Posto _postoAtual, _postoFuturo, _postoInicial;
    public int _totalTimeToChange;

    private int _arrivalTurn;

    public void OnReset()
    {
        if (_postoAtual != _postoInicial)
        {
            //this._postoAtual.leaveAtendente();
            _postoAtual = _postoInicial;
            transform.position = _postoAtual.transform.Find("ref at").position;
        }

        _postoAtual.temAtendente = true;
        _postoAtual.atendente = this;
        _isChanging = false;
        _isBusy = false;
        this._postoFuturo = null;
        //Debug.Break();
    }

    private void SetArrivalTurn()
    {
        _arrivalTurn = Controller.Instance._currentWorldTurn + _totalTimeToChange;
    }

    public void onCheckIfArrive()
    {
        if (!_isChanging)
            return;

        //if (_arrivalTurn >= Controller.Instance._currentWorldTurn)
        if (Controller.Instance._currentWorldTurn >= _arrivalTurn)
        {
            _postoFuturo.setAtendente(this);

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
            foreach (var item in Controller.Instance._gerenciadorDePosto.postos)
            {
                if (item.letra == posto.letra)
                {
                    if (item.temAtendente)
                        countAtendentes++;
                }
            }

            float a = posto._minhaFila._userInside.Count * posto.turnos;
            float b = (_postoAtual._minhaFila._userInside.Count * _postoAtual.turnos) + 
                (_totalTimeToChange * Controller.Instance.multiplicadorDoTempoDeTroca * ((_postoAtual._minhaFila._userInside.Count != 0) ? 1 : 0));
            // if (countAtendentes == 1)
            // {
            //    b = ((_postoAtual._minhaFila._userInside.Count * _postoAtual.turnos) + (_totalTimeToChange * Controller.Instance.multiplicadorDoTempoDeTroca)*1.75f);
            // }
            // else
            // {
            //    b = ((_postoAtual._minhaFila._userInside.Count * _postoAtual.turnos) + (_totalTimeToChange * Controller.Instance.multiplicadorDoTempoDeTroca)*0.5f);
            // }

            if (a >= b) //Verifica o posto atual
            {
                //Considerar se compensa
                if (tempoFila <= a)
                {
                    postoFuturo = posto;
                    tempoFila = a;
                }
            }
        }

        if(postoFuturo != null)
        {
            Debug.Log(gameObject.name + " decidiu trocar para " + postoFuturo.name);
            Debug.Break();

            // Pensar na condicao de troca
            this._postoFuturo = postoFuturo;

            // tempoFilaFutura / 2 + tempoTroca < tempoFilaFutura
            Troca();
        }
    }

    public GameObject tween;
    public void Troca()
    {
        //Debug.Log("atendente troca " + _myName + " posto " + _postoAtual._myType + " -> " + _postoFuturo._myType);
        this._postoAtual.leaveAtendente();

        RectTransform a = GetComponent<RectTransform>();
        transform.position = new Vector3(transform.position.x, transform.position.y - 40f, 0);
        this._postoAtual = null;

        this._postoFuturo._atendenteVindo = this;

        a.transform.eulerAngles = new Vector3(0, 0, (this.transform.position.x < this._postoFuturo.transform.position.x) ? 90 : -90);

        //Debug.Log("time to change " + (_totalTimeToChange - 1));
        //Debug.Break();
        tween = NightTween.Create(gameObject, (_totalTimeToChange) / Controller.Instance._gameSpeed, new NightTweenParams()
            .Property(NTPropType.transformPosition, new Vector3(this._postoFuturo.transform.position.x, transform.position.y, 0))
            );
        //this._postoAtual = this._postoFuturo;

        SetArrivalTurn();
        _isChanging = true;
    }
}
