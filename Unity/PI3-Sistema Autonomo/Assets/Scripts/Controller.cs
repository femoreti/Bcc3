using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller Instance;
    public int _currentWorldTurn = 0;

    // Tempo medio usuario //
    public List<UserBasics> tempUserList;
    public int userInSistem;
    public int userTotalTime;
    public int totalUsers;

    public GerenciadorPosto _gerenciadorDePosto;
    public FilaManager _filaManager;
    public List<Posto> _postos = new List<Posto>();

    private GameObject _containerUsers;
    private Text _contadorTurnos;

	// Use this for initialization
	void Awake () {
        Instance = this;
	}

	// Update is called once per frame
	void Start () {
        _contadorTurnos = GameObject.Find("Turno").GetComponent<Text>();

        _gerenciadorDePosto.Init();
        _postos = _gerenciadorDePosto.postos;

        _filaManager.CriarFilas(_gerenciadorDePosto.totalPostosDistindos);
    }

    private Coroutine gameRoutine;
    public void onClickStart()
    {
        tempUserList = UserCreator.Instance._userLine;
        gameRoutine = StartCoroutine(GameTime());
    }

    IEnumerator GameTime()
    {
        while (true)
        {
            _contadorTurnos.text = "Turnos: " + _currentWorldTurn.ToString();
            yield return new WaitForSeconds(1f);
            OnAddTurn();
        }
    }

    public void OnAddTurn()
    {
        _currentWorldTurn++;

        AvancaPostos(); //Faz os postos verificarem se podem retirar alguem das filas

        if (_containerUsers == null)
            _containerUsers = new GameObject("Usuarios");

        //Verifica se algum usuario ira ser criado neste turno
        for (int i = 0; i < tempUserList.Count; i++)
        {
            UserBasics ub = tempUserList[i];

            if (ub.arrivalTurn == _currentWorldTurn)
            {
                //User user = new User(ub);

                GameObject u = Instantiate(UserCreator.Instance._prefabUser);
                u.name = ub.name;
                User comp = u.GetComponent<User>();
                comp.userStats = ub;

                comp.ProximaFila();

                tempUserList.RemoveAt(i);
                i--;
            }
        }

        if(userInSistem == 0 && tempUserList.Count == 0)
        {
            Debug.Log("fim de sistema");
            Debug.Log("total user: " + totalUsers.ToString());
            Debug.Log("tempo medio user: " + ((float)userTotalTime / (float)totalUsers).ToString());
            StopCoroutine(gameRoutine);
        }
    }

    public void AvancaPostos()
    {
        foreach(Posto p in _postos)
        {
            p.PassaTurno();
        }
    }
}
