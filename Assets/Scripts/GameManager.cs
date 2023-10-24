using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCubeTurn = true;
    public TextMeshProUGUI label;
    public Cell[] cells;
    public GameObject restartButton;
    public GameObject backToMenuButton;
    public AudioClip clipWin;
    public AudioClip clipDraw;
    public bool modeAI;
    private bool hasAIMoved = false;
    public TextMeshProUGUI wPlayer;

    void Start()
    {
        ChangeTurn();
        restartButton.SetActive(false);
        backToMenuButton.SetActive(false);
        int flag = PlayerPrefs.GetInt("AI", 1);
        modeAI = flag == 1;
        wPlayer.gameObject.SetActive(modeAI);

    }

    public void CheckWinner()
    {
        bool isDraw = true;
        for (int i = 0; i < 9; i += 3)
        {
            if (cells[i].status != CellType.EMPTY && cells[i].status == cells[i + 1].status && cells[i + 1].status == cells[i + 2].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
            if (cells[i].status == CellType.EMPTY || cells[i + 1].status == CellType.EMPTY || cells[i + 2].status == CellType.EMPTY) isDraw = false;
        }
        for (int i = 0; i < 3; i++)
        {
            if (cells[i].status != 0 && cells[i].status == cells[i + 3].status && cells[i + 3].status == cells[i + 6].status)
            {
                DeclareWinner(cells[i].status);
                return;
            }
        }
        if (cells[0].status != 0 && cells[0].status == cells[4].status && cells[4].status == cells[8].status)
        {
            DeclareWinner(cells[0].status);
            return;
        }
        if (cells[2].status != 0 && cells[2].status == cells[4].status && cells[4].status == cells[6].status)
        {
            DeclareWinner(cells[2].status);
            return;
        }
        if (isDraw)
        {
            label.text = "It's a draw!";
            SetupGameFinished(false);
        }
    }

    public void ChangeTurn()
{
    isCubeTurn = !isCubeTurn;
    if (modeAI)  // Si es el modo Player vs IA
    {
        if (isCubeTurn)
        {
            label.text = "Torn del jugador local";
        }
        else
        {
            label.text = "Torn de la IA";
        }
    }
    else  // Si es el modo Player vs Player
    {
        if (isCubeTurn)
        {
            label.text = "Torn dels cube";
        }
        else
        {
            label.text = "Torn de les sphere";
        }
    }
}

    void DeclareWinner(CellType status)
{
    if (modeAI)  // Si es el modo Player vs IA
    {
        if (status == CellType.SPHERE)
        {
            label.text = "Victoria de la IA!";
        }
        else
        {
            label.text = "Vicotria del Jugador!";
        }
    }
    else  // Si es el modo Player vs Player
    {
        if (status == CellType.SPHERE)
        {
            label.text = "Victoria de les sphere!";
        }
        else
        {
            label.text = "Victoria dels cube!";
        }
    }

    SetupGameFinished(true);
}

    void Update()
{
    if (modeAI && !isCubeTurn && !hasAIMoved) // Esto asegura que solo se ejecute durante el turno de la esfera
    {
        hasAIMoved = true;
        StartCoroutine(MakeAIMove());
    }
}

    IEnumerator MakeAIMove()
    {
        if (isCubeTurn) yield break;
        yield return new WaitForSeconds(3f);

        int move = -1;
        move = FindWinningMove(CellType.CUBE);
        if (move != -1)
        {
            cells[move].onClick();
            hasAIMoved = false;
            yield break;
        }

        move = FindBlockingMove(CellType.SPHERE);
        if (move != -1)
        {
            cells[move].onClick();
            hasAIMoved = false;
            yield break;
        }

        if (cells[4].status == CellType.EMPTY)
        {
            cells[4].onClick();
            hasAIMoved = false;
            yield break;
        }

        int[] corners = new int[] { 0, 2, 6, 8 };
        foreach (int i in corners)
        {
            if (cells[i].status == CellType.EMPTY)
            {
                cells[i].onClick();
                hasAIMoved = false;
                yield break;
            }
        }

        for (int i = 1; i <= 7; i += 2)
        {
            if (cells[i].status == CellType.EMPTY)
            {
                cells[i].onClick();
                break;
            }
        }
        hasAIMoved = false;
    }

    int FindWinningMove(CellType type)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].status == CellType.EMPTY)
            {
                cells[i].status = type;
                if (IsWinningMove())
                {
                    cells[i].status = CellType.EMPTY;
                    return i;
                }
                cells[i].status = CellType.EMPTY;
            }
        }
        return -1;
    }

    int FindBlockingMove(CellType type)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].status == CellType.EMPTY)
            {
                cells[i].status = type;
                if (IsWinningMove())
                {
                    cells[i].status = CellType.EMPTY;
                    return i;
                }
                cells[i].status = CellType.EMPTY;
            }
        }
        return -1;
    }

    bool IsWinningMove()
    {
        for (int i = 0; i < 9; i += 3)
        {
            if (cells[i].status != CellType.EMPTY && cells[i].status == cells[i + 1].status && cells[i + 1].status == cells[i + 2].status)
            {
                return true;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (cells[i].status != CellType.EMPTY && cells[i].status == cells[i + 3].status && cells[i + 3].status == cells[i + 6].status)
            {
                return true;
            }
        }
        if (cells[0].status != CellType.EMPTY && cells[0].status == cells[4].status && cells[4].status == cells[8].status)
        {
            return true;
        }
        if (cells[2].status != CellType.EMPTY && cells[2].status == cells[4].status && cells[4].status == cells[6].status)
        {
            return true;
        }
        return false;
    }

    void SetupGameFinished(bool hasWinner)
    {
        restartButton.SetActive(true);
        backToMenuButton.SetActive(true);

        if (hasWinner)
        {
            // TODO: play win sound
        }
        else
        {
            // TODO: play draw sound
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
