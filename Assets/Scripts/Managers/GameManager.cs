using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _ins;
    public static GameManager Ins
    {
        get
        {
            if (!_ins)
                _ins = FindObjectOfType<GameManager>();
            return _ins;
        }
    }

    [SerializeField] private GameObject _startCanvas;
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _finishCanvas;
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private TMP_Text _bestTimeWASD;
    [SerializeField] private TMP_Text _bestTimeArrows;
    [SerializeField] private List<PlayerController> _playerList = new();

    private int _count;

    public void Finish(string winner)
    {
        _gameCanvas.SetActive(false);
        _finishCanvas.SetActive(true);
        Timer.Ins.StopTimer();
        _winnerText.text = $"{winner} WON!";
        Timer.Ins.SetBestTime(winner == "WASD PLAYER" ? true : false);
        _bestTimeWASD.text = Timer.Ins.GetBestTimeWASD();
        _bestTimeArrows.text = Timer.Ins.GetBestTimeArrows();

        foreach (var item in _playerList)
            item.canMove = false;
    }

    public void StartGame()
    {
        _count++;
        if(_count == 2)
        {
            _startCanvas.SetActive(false);
            _gameCanvas.SetActive(true);
            foreach (var item in _playerList)
                item.canMove = true;
            Timer.Ins.timeText.gameObject.SetActive(true);
            Timer.Ins.StartTimer();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
