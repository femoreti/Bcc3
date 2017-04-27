using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller Instance;
    public int _currentWorldTurn = 0;
    public int _gameSpeed = 1;

    // Tempo medio usuario //
    public List<UserBasics> tempUserList;
    public int userInSistem;
    public int userTotalTime;
    public int totalUsers;

    //User que ficou mais tempo
    private User userHighestTime;

    // Tempo médio por combinação //
    public Dictionary<string, int> _totalTimeByType, _totalUserByType;

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
	IEnumerator Start () {
        _contadorTurnos = GameObject.Find("Turno").GetComponent<Text>();

        _gameSpeed = 1;
        if (_totalTimeByType == null)
        {
            _totalTimeByType = new Dictionary<string, int>();
            _totalUserByType = new Dictionary<string, int>();
        }
        _totalTimeByType.Clear();
        _totalUserByType.Clear();

        _gerenciadorDePosto.Init();
        _postos = _gerenciadorDePosto.postos;

        yield return new WaitForEndOfFrame();
        _filaManager.CriarFilas(_gerenciadorDePosto.totalPostosDistindos);
    }

    [HideInInspector]
    public Coroutine gameRoutine;
    public void onClickStart()
    {
        tempUserList = UserCreator.Instance._userLine;
        gameRoutine = StartCoroutine(GameTime());
    }

    public IEnumerator GameTime()
    {
        while (true)
        {
            _contadorTurnos.text = "Turnos: " + _currentWorldTurn.ToString();
            yield return new WaitForSeconds(1f / _gameSpeed);
            OnAddTurn();
        }
    }

    /// <summary>
    /// Cada novo turno comeca nesse metodo
    /// </summary>
    public void OnAddTurn()
    {
        _currentWorldTurn++;

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

        AvancaPostos(); //Faz os postos verificarem se podem retirar alguem das filas


        if (userInSistem == 0 && tempUserList.Count == 0)
        {
            Debug.Log("fim de sistema");

            Debug.Log("tempo medio user: " + ((float)userTotalTime / (float)totalUsers).ToString());
            Debug.Log("tempo medio espera fila:\n");
            _filaManager.TempoMedioPorFila();
            Debug.Log("User com maior tempo no sistema: " + userHighestTime.userStats.name);

            foreach(KeyValuePair<string, int> d in _totalTimeByType)
            {
                //Debug.Log(d.Key + " ---- " + d.Value);
                //Debug.Log(_totalUserByType[d.Key]);
                Debug.Log("Tempo medio no percurso " + d.Key + " foi de: " + ((float)d.Value / (float)_totalUserByType[d.Key]));
            }

            StopCoroutine(gameRoutine);
        }
    }

    /// <summary>
    /// Cada vez q um usuario sair do sistema ira cair aqui
    /// </summary>
    /// <param name="u"></param>
    public void UserLeavingSistem(User u)
    {
        if(userHighestTime == null || u.totalTimeInSistem > userHighestTime.totalTimeInSistem)
        {
            userHighestTime = u;
        }

        //Controla o tempo medio por tipo de percurso
        if (!_totalTimeByType.ContainsKey(u.userStats.order))
        {
            _totalTimeByType.Add(u.userStats.order, 0);
            _totalUserByType.Add(u.userStats.order, 0);
        }
        _totalTimeByType[u.userStats.order] += u.totalTimeInSistem;
        _totalUserByType[u.userStats.order]++;


        userTotalTime += u.totalTimeInSistem;
    }

    /// <summary>
    /// Passa o turno para os postos chamarem o proximo user
    /// </summary>
    public void AvancaPostos()
    {
        foreach(Posto p in _postos)
        {
            p.PassaTurno();
        }
    }
}
