using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct UserBasics
{
    public string name;
    public int arrivalTurn;
    public List<char> walkOrder;
}

public class User : MonoBehaviour
{
    public int _currentGuiche = -1;
    public int _turnosRestantesNoGuiche = 0;
    public UserBasics userStats;

    private List<User> _currentFila;

	// Use this for initialization
	void Start ()
    {
        transform.GetChild(0).GetComponent<Text>().text = userStats.name;

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
            if (f._userInside != null)
            {
                f._userInside.Add(this); //Se insere na fila
                _currentFila = f._userInside;
                
                if (f._parent != null)
                {
                    transform.parent = f._parent.transform;
                }
            }
            
            gameObject.name = userStats.name + " / " + f._myType.ToString();
        }
        else
        {
            Debug.Log(userStats.name + " saiu do sistema");
            //Destroy(this.gameObject);
            gameObject.name = userStats.name + " / saiu";
            RemoveDaFila();

            Destroy(this.gameObject);
        }

        
    }

    public void RemoveDaFila()
    {
        if (_currentFila != null)
            if (_currentFila.Contains(this))
            {
                _currentFila.Remove(this);
                _currentFila = null;
            }
    }

    //public int avancaTurnoFila()
    //{
    //    if (_currentFila.Count > 0 && _currentFila[0] == this)
    //        return _turnosRestantesNaFilaGuiche--;
    //    else
    //        return _turnosRestantesNaFilaGuiche;
    //}
}
