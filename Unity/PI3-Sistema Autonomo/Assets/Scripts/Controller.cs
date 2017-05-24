using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private static Controller instance;
    public static Controller Instance
    {
        get
        {
            return instance;
        }
    }

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

    //Atendentes
    public List<Atendente> _totalAtendentes;
    public float multiplicadorDoTempoDeTroca = 2f;

    //GameTime
    public bool _gamePause, _gameStarted;
    private float _secDrop = 1;

	// Use this for initialization
	void Awake () {
        instance = this;
	}

    private void Update()
    {
        if (!_gameStarted || _gamePause)
            return;

        _secDrop -= Time.deltaTime * _gameSpeed;

        if(_secDrop <= 0)
        {
            _secDrop = 1;

            //NovoTurno
            OnAddTurn();
            _contadorTurnos.text = "Turnos: " + _currentWorldTurn.ToString();
        }
    }

    // Update is called once per frame
    IEnumerator Start () {
        _contadorTurnos = GameObject.Find("Turno").GetComponent<Text>();

        onReset();

        _gerenciadorDePosto.Init();
        yield return new WaitForEndOfFrame();
        _gerenciadorDePosto.PopulaObjetoPosto();
        _postos = _gerenciadorDePosto.postos;

        yield return new WaitForEndOfFrame();
        _filaManager.CriarFilas(_gerenciadorDePosto.totalPostosDistindos);
    }

    [HideInInspector]
    public Coroutine gameRoutine;
    public void onClickStart()
    {
        tempUserList = UserCreator.Instance._userLine;
        //gameRoutine = StartCoroutine(GameTime());
        _gameStarted = true;
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
                RectTransform re = u.GetComponent<RectTransform>();
                re.sizeDelta = new Vector2(UserCreator.Instance.userSize, UserCreator.Instance.userSize);
                comp.userStats = ub;

                comp.ProximaFila();

                tempUserList.RemoveAt(i);
                i--;
            }
        }

        onAvancaAtendentes();
        AvancaPostos(); //Faz os postos verificarem se podem retirar alguem das filas


        if (userInSistem == 0 && tempUserList.Count == 0)
        {
            Debug.Log("fim de sistema");

            string str = "Turnos: " + _currentWorldTurn.ToString();

            str += "\n\nTempo médio do usuario no sistema: " + ((float)userTotalTime / (float)totalUsers).ToString("00s") + "\n";
            //Debug.Log("tempo medio user: " + ((float)userTotalTime / (float)totalUsers).ToString());

            str += "\ntempo medio espera fila:\n";
            //Debug.Log("tempo medio espera fila:\n");
            
            str += _filaManager.TempoMedioPorFila();

            str += "\n\nUser com maior tempo no sistema: " + userHighestTime.userStats.name + " - tempo: "+ userHighestTime.totalTimeInSistem + "s\n";
            //Debug.Log("User com maior tempo no sistema: " + userHighestTime.userStats.name);

            foreach(KeyValuePair<string, int> d in _totalTimeByType)
            {
                //Debug.Log(d.Key + " ---- " + d.Value);
                //Debug.Log(_totalUserByType[d.Key]);
                str += "\nTempo medio no percurso " + d.Key + " foi de: " + ((float)d.Value / (float)_totalUserByType[d.Key]).ToString("00s");
                //Debug.Log("Tempo medio no percurso " + d.Key + " foi de: " + ((float)d.Value / (float)_totalUserByType[d.Key]));
            }

            UIController.Instance.onShowEndScreen(str);

            _gameStarted = false;
            //StopCoroutine(gameRoutine);
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

    public void onAvancaAtendentes()
    {
        //Atendente deve verificar aqui
        foreach (Atendente a in _totalAtendentes)
        {
            a.onCheckIfArrive(); //verifica se o atendente esta no posto
        }
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

    public void onReset()
    {
        UserCreator.Instance.ReadFilaFile();

        _currentWorldTurn = 0;
        _gameSpeed = 1;
        _secDrop = 1;

        foreach (Posto p in _postos)
        {
            p.OnReset();
        }
        if (_totalAtendentes == null)
            _totalAtendentes = new List<Atendente>();
        else if(_totalAtendentes.Count > 0)
        {
            foreach (Atendente a in _totalAtendentes)
            {
                a.OnReset();
            }
        }
        //_totalAtendentes.Clear();

        _filaManager.onReset();
        if (_totalTimeByType == null)
        {
            _totalTimeByType = new Dictionary<string, int>();
            _totalUserByType = new Dictionary<string, int>();
        }
        _totalTimeByType.Clear();
        _totalUserByType.Clear();
    }
}
