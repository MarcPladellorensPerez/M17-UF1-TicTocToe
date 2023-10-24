using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    EMPTY,
    SPHERE,
    CUBE
}
public class Cell : MonoBehaviour
{
    public CellType status;
    public GameManager gameManger;
    public GameObject sphere;
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        sphere.SetActive(false);
        cube.SetActive(false);
        status = CellType.EMPTY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnMouseDown()
{
    // Si estamos en modo IA y no es el turno de los cubos, no permitir el clic del jugador
    if (gameManger.modeAI && !gameManger.isCubeTurn)
        return;

    onClick();
}
    public void onClick()
    {
        Debug.Log("Entrant a onclick");
        if (cube.activeSelf==true ||  sphere.activeSelf == true)
        {
            //nos vamos si cubo y sphere activos
            Debug.Log("Eips que la cel·la ja està clicada");
            return;
        }
        
        if (gameManger.isCubeTurn == true)
        {
            Debug.Log("Entrant a onclick iscubeturn");
            status = CellType.CUBE;
            cube.SetActive(true);
            sphere.SetActive(false);
            gameManger.ChangeTurn();
        }
        else
        {
            status = CellType.SPHERE;
            sphere.SetActive(true);    
            cube.SetActive(false);
            gameManger.ChangeTurn();
        }
        
        gameManger.CheckWinner();
    }
}
