using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameObject _gameManager;
    [SerializeField]
    private Image[] _shieldCounters;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Image _healthBar;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);

        foreach (Image i in _shieldCounters)
            i.enabled = false;
    }



    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateShields(int shields)
    {
        switch (shields)
        {
            case 1:
                _shieldCounters[0].enabled = true;
                _shieldCounters[1].enabled = false;
                _shieldCounters[2].enabled = false;
                break;
            case 2:
                _shieldCounters[0].enabled = true;
                _shieldCounters[1].enabled = true;
                _shieldCounters[2].enabled = false;
                break;
            case 3:
                _shieldCounters[0].enabled = true;
                _shieldCounters[1].enabled = true;
                _shieldCounters[2].enabled = true;
                break;
            default:
                foreach (Image i in _shieldCounters)
                    i.enabled = false;
                break;
        }
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
    }

    public void UpdateBoosterBar(float b)
    {
        _healthBar.fillAmount = b / 100;
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOver());
        _restartText.gameObject.SetActive(true);
        _gameManager.GetComponent<GameManager>().GameOver();
    }

    IEnumerator FlickerGameOver()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }
}
