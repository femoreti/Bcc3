using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atendente : MonoBehaviour
{
    public bool _canChangePosto;
    public Posto _postoAtual, _postoFuturo;
    public int _totalTimeToChange;

    private int _arrivalTurn;

    private void SetArrivalTurn()
    {
        _arrivalTurn = Controller.Instance._currentWorldTurn + _totalTimeToChange;
    }

    public void onCheckIfArrive()
    {
        if(_arrivalTurn == Controller.Instance._currentWorldTurn)
        {
            //Cheguei caralho, me coloca pra trabalhar no meu posto
            _postoFuturo.setAtendente(this);
        }
    }

    public void CheckTroca() {
        Posto postoFuturo = null;
        
        if (_postoAtual.atendentes.Count > 1) {
            var tempoFila = 0;
            foreach (var item in Controller.Instance._gerenciadorDePosto.postos) {
                if (item._minhaFila._totalUsers * item.turnos > tempoFila) {
                    tempoFila = item._minhaFila._totalUsers * item.turnos;
                    postoFuturo = item;
                }

                // Pensar na condicao de troca
                this._postoFuturo = postoFuturo;
                Troca();
            }
        }
    }

    public void Troca() {
        this._postoAtual = this._postoFuturo;
    }
}
