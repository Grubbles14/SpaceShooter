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
    [SerializeField]
    private Text _waveCountText;
    [SerializeField]
    private Text _waveCountdownTimer;

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

    public void UpdateAmmo(int ammo, int max)
    {
        _ammoText.text = "Ammo: " + ammo + "/" + max;
    }

    public void UpdateBoosterBar(float b)
    {
        _healthBar.fillAmount = b / 100;
    }

    public void UpdateWaveCount(int w)
    {
        _waveCountText.text = "Wave: " + w;
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

    IEnumerator WaveCountdownTimer(float t, bool b)
    {
        _waveCountdownTimer.enabled = true;
        float secondsLeft = t;
        while (secondsLeft > 0)
        {
            Debug.Log("Wave countdown: " + secondsLeft);
            if(b)
                _waveCountdownTimer.text = "Boss wave starting in: " + secondsLeft;
            else
                _waveCountdownTimer.text = "Next wave starting in: " + secondsLeft;
            yield return new WaitForSeconds(1.0f);
            secondsLeft--;
        }
        _waveCountdownTimer.enabled = false;
    }

    public void StartWaveTimer(float time, bool boss)
    {
        StopCoroutine("WaveCountdownTimer");
        StartCoroutine(WaveCountdownTimer(time, boss));
    }
}
