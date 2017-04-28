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
    public int _totalUsersMedia;

    /// <summary>
    /// Inicia uma fila
    /// </summary>
    /// <param name="i"></param>
    /// <param name="lineObj"></param>
    public void setConfig(int i, GameObject lineObj)
    {
        _myType = (FilaType)i;

        gameObject.name = "Fila " + _myType.ToString();

        _userInside = new List<User>();
        _parent = lineObj;
        _totalTimeUserInside = 0; //tempo total que os usuarios gastaram na fila
        _totalUsersMedia = 0; //total que passou na fila
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
        List<Posto> tempPosto = Controller.Instance._postos;

        for (int i = 0; i < total; i++)
        {
            GameObject go = Instantiate(prefabFila, filaContainer.transform);
            Fila f = go.AddComponent<Fila>();

            f.setConfig(i, go);
            _filas.Add(f);

            Vector3 posPosto = Vector3.zero;
            Vector2 delta = Vector2.zero;
            int totalPostos = 0;
            for (int j = 0; j < tempPosto.Count; j++)
            {
                if (tempPosto[j]._myType == f._myType)
                {
                    //Debug.Log("fila " + tempPosto[j]._myType + " encontrou ");
                    posPosto += tempPosto[j].transform.position;
                    totalPostos++;
                    RectTransform postoSize = tempPosto[j].GetComponent<RectTransform>();
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(postoSize.sizeDelta.y, postoSize.sizeDelta.y);

                    delta = postoSize.sizeDelta;

                    tempPosto[j]._minhaFila = f;
                    //tempPosto.RemoveAt(j);
                    //j--;
                }
                else
                    continue;
            }

            go.transform.position = new Vector3(posPosto.x/totalPostos + delta.x + 20f, posPosto.y/totalPostos, 0);
            go.transform.localScale = Vector3.one;
            //Debug.Log("fila " + i + " pos " + posPosto);
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
    public string TempoMedioPorFila()
    {
        string str = string.Empty;
        foreach(Fila f in _filas)
        {
            str += "\nFila " + f._myType.ToString() + ": " + ((float)f._totalTimeUserInside / (float)f._totalUsersMedia);
            //Debug.Log("Fila " + f._myType.ToString() + ": " + ((float)f._totalTimeUserInside / (float)f._totalUsers));
        }

        return str;
    }
}
