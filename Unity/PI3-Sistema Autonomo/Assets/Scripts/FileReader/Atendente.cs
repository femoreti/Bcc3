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


}
