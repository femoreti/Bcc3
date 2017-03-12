using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FilaType
{
    A,B,C,D,E
}

[System.Serializable]
public struct Fila
{
    public FilaType _myType;
    public List<User> _userInside;
    public GameObject _parent;

    public User RetiraProximo()
    {
        User _proximo = _userInside[0];
        _proximo.RemoveDaFila();
        return _proximo;
    }
}

public class FilaManager : MonoBehaviour
{    
    public List<Fila> _filas;

	// Use this for initialization
	void Awake()
    {
	}

    public Fila AchaFila(FilaType _seachType)
    {
        for(int i = 0; i < _filas.Count; i++)
        {
            if(_seachType == _filas[i]._myType)
            {
                return _filas[i];
            }
        }

        return new Fila();
    }
}
