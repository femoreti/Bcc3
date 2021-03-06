﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct UserBasics
{
    public string name;
    public int arrivalTurn;
    public int exitTurn;
    public List<char> walkOrder;
    public string order;
}

public class User : MonoBehaviour
{
    public int _currentGuiche = -1;
    public UserBasics userStats;

    private List<User> _currentFila;
    private Fila _line;
    private int lineArrivalTurn = 0;

	// Use this for initialization
	public void Init ()
    {
        transform.GetChild(0).GetComponent<Text>().text = userStats.name;
        GetComponent<Image>().sprite = Controller.Instance._sptUsers[UnityEngine.Random.Range(0, Controller.Instance._sptUsers.Count)];
        Controller.Instance.totalUsers++;
        Controller.Instance.userInSistem++;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    /// <summary>
    /// Usuario se insere na fila desejada e se remove da fila a qual pertence
    /// </summary>
    /// <param name="f"></param>
    public void ProximaFila()
    {
        //Debug.Log(userStats.name);

        _currentGuiche++; //Proximo guiche caso ja tenha passado a triagem

        if (_currentGuiche < userStats.walkOrder.Count)
        {
            Fila f = Controller.Instance._filaManager.AchaFila((FilaType)Enum.Parse(typeof(FilaType), userStats.walkOrder[_currentGuiche].ToString())); //Localiza a Fila deste guichê
            _line = f;
            f._totalUsersMedia++;

            if (f._userInside != null)
            {
                f._userInside.Add(this); //Se insere na fila
                f._myTextCount.text = f._userInside.Count.ToString();
                if (f._userInside.Count > 8)
                    f._myTextCount.enabled = true;
                else
                    f._myTextCount.enabled = false;

                _currentFila = f._userInside;
                lineArrivalTurn = Controller.Instance._currentWorldTurn;
                
                if (f._parent != null)
                {
                    transform.parent = f._parent.transform;
                    transform.localScale = Vector3.one;
                }
            }
            //Debug.Log(userStats.name + " entoru no posto ");
            gameObject.name = userStats.name + " / " + f._myType.ToString();
        }
        else
        {
            //Debug.Log(userStats.name + " saiu do sistema " + totalTimeInSistem);
            //Destroy(this.gameObject);
            gameObject.name = userStats.name + " / saiu";
            RemoveDaFila();
            userStats.exitTurn = Controller.Instance._currentWorldTurn;
            Controller.Instance.UserLeavingSistem(this);
            //Debug.Log("user total time: " + Controller.Instance.userTotalTime.ToString());

            Controller.Instance.userInSistem--;

            //Debug.Log(userStats.name + " saiu do sistema " + totalTimeInSistem);
            Destroy(this.gameObject);
        }

        
    }

    /// <summary>
    /// retira o usuario da fila    
    /// </summary>
    public void RemoveDaFila()
    {
        if (_currentFila != null)
            if (_currentFila.Contains(this))
            {
                _line._totalTimeUserInside += (Controller.Instance._currentWorldTurn - lineArrivalTurn);
                _currentFila.Remove(this);
                _currentFila = null;
                _line = null;
            }
    }

    /// <summary>
    /// Calcula o tempo total que o usuario passou no sistema
    /// </summary>
    public int totalTimeInSistem
    {
        get
        {
            return userStats.exitTurn - userStats.arrivalTurn;
        }
    }
}
