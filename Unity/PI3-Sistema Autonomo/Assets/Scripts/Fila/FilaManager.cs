using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FilaType
{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
    F = 5,
    G = 6,
    H = 7,
    I = 8,
    J = 9,
    K = 10,
    L = 11,
    M = 12,
    N = 13,
    O = 14,
    P = 15,
    Q = 16,
    R = 17,
    S = 18,
    T = 19,
    U = 20,
    V = 21,
    W = 22,
    Y = 23,
    X = 24,
    Z = 25
}

[System.Serializable]
public struct Fila
{
    public FilaType _myType;
    public List<User> _userInside;
    public GameObject _parent;

    public Fila(int i, GameObject parent)
    {
        _myType = (FilaType)i;
        _userInside = new List<User>();
        _parent = parent;
    }

    public User RetiraProximo()
    {
        User _proximo = _userInside[0];
        _proximo.RemoveDaFila();
        return _proximo;
    }
}

public class FilaManager : MonoBehaviour
{
    public GameObject prefabFila, filaContainer;
    public List<Fila> _filas;

	// Use this for initialization
	void Awake()
    {

	}

    public void CriarFilas(int total)
    {
        _filas.Clear();
        for(int i = 0; i < total; i++)
        {
            GameObject go = Instantiate(prefabFila, filaContainer.transform);
            Fila f = new Fila(i, go);

            _filas.Add(f);
        }
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
