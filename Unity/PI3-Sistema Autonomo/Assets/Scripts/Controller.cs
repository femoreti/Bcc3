using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller Instance;
    public int _currentWorldTurn = 0;

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

    }

    public void onClickStart()
    {


        StartCoroutine(GameTime());
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
        for (int i = 0; i < UserCreator.Instance._userLine.Count; i++)
        {
            UserBasics ub = UserCreator.Instance._userLine[i];

            if (ub.arrivalTurn == _currentWorldTurn)
            {
                //User user = new User(ub);

                GameObject u = Instantiate(UserCreator.Instance._prefabUser);
                u.name = ub.name;
                User comp = u.GetComponent<User>();
                comp.userStats = ub;

                comp.ProximaFila();
            }
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
