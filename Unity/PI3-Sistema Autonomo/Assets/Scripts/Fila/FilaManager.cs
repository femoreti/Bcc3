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
public class Fila : MonoBehaviour
{
    public FilaType _myType;
    public List<User> _userInside;
    public GameObject _parent;
    public int _totalTimeUserInside;
    public int _totalUsers;

    /// <summary>
    /// Inicia uma fila
    /// </summary>
    /// <param name="i"></param>
    /// <param name="parent"></param>
    public void setConfig(int i, GameObject parent)
    {
        _myType = (FilaType)i;

        gameObject.name = "Fila " + _myType.ToString();

        _userInside = new List<User>();
        _parent = parent;
        _totalTimeUserInside = 0; //tempo total que os usuarios gastaram na fila
        _totalUsers = 0; //total que passou na fila
    }

    /// <summary>
    /// Retira o primeiro da fila para os guiches
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Cria o objeto fila
    /// </summary>
    /// <param name="total"></param>
    public void CriarFilas(int total)
    {
        _filas.Clear();
        for(int i = 0; i < total; i++)
        {
            GameObject go = Instantiate(prefabFila, filaContainer.transform);
            Fila f = go.AddComponent<Fila>();

            f.setConfig(i, go);

            _filas.Add(f);
        }
    }

    /// <summary>
    /// Encontra a fila especifica
    /// </summary>
    /// <param name="_seachType"></param>
    /// <returns></returns>
    public Fila AchaFila(FilaType _seachType)
    {
        for(int i = 0; i < _filas.Count; i++)
        {
            if(_seachType == _filas[i]._myType)
            {
                return _filas[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Ao fim do sistema ira exibir o tempo medio de cada fila
    /// </summary>
    public void TempoMedioPorFila()
    {
        foreach(Fila f in _filas)
        {
            Debug.Log("Fila " + f._myType.ToString() + ": " + ((float)f._totalTimeUserInside / (float)f._totalUsers));
        }
    }
}
