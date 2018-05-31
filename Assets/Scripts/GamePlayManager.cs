using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    /// <summary>
    /// Game state enum
    /// </summary>
    public enum GameStateEnum
    {
        Playing,
        GameOver,
        Paused,
        Launched
    }

    public GameStateEnum GameState { get; private set; }

    /// <summary>
    /// Paddle state
    /// </summary>
    public enum PaddleState
    {
        Cpu,
        Player
    }

    /// <summary>
    /// Hud canvas
    /// </summary>
    [SerializeField] private GameObject _hudCanvas;

    /// <summary>
    /// CPU paddle
    /// </summary>
    [SerializeField] private GameObject _cpuPaddle;

    /// <summary>
    /// Ball Game onject
    /// </summary>
    [SerializeField] private GameObject _ball;

    /// <summary>
    /// CPU Score
    /// </summary>
    private int _cpuScore;

    /// <summary>
    /// Plauer Score
    /// </summary>
    private int _playerScore;

    /// <summary>
    /// Hud manager
    /// </summary>
    private HudManager _hudManager;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        GameState = GameStateEnum.Launched;
        _hudManager = _hudCanvas.GetComponent<HudManager>();
        _hudManager.PlayAgain.text = "PRESS SPACE TO PLAY";
    }

    /// <summary>
    /// Update is called once per frame 
    /// </summary>
    private void Update()
    {
        CheckScore();
        CheckInput();
    }

    /// <summary>
    /// Increace CPU point
    /// </summary>
    public void IncreaceCpuPoint()
    {
        _cpuScore++;
        _hudManager.CpuScore.text = _cpuScore.ToString();
        InitNextRound();
    }

    /// <summary>
    /// Increace player point
    /// </summary>
    public void IncreacePlayerPoint()
    {
        _playerScore++;
        _hudManager.PlayerScore.text = _playerScore.ToString();
        InitNextRound();
    }

    /// <summary>
    /// Check input
    /// </summary>
    private void CheckInput()
    {
        if (GameState == GameStateEnum.Paused || GameState == GameStateEnum.Playing)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                PauseResumeGame();
            }
        }

        if (GameState == GameStateEnum.Launched || GameState == GameStateEnum.GameOver)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartGame();
            }
        }
    }

    /// <summary>
    /// Check Score
    /// </summary>
    private void CheckScore()
    {
        if (_playerScore < Model.Config.WinnigScore && _cpuScore < Model.Config.WinnigScore) return;

        if (_playerScore >= Model.Config.WinnigScore && _cpuScore < _playerScore - 1)
        {
            // - Player Wins
            _hudManager.WinPlayer.enabled = true;
            GameOver();
        }
        else if (_cpuScore >= Model.Config.WinnigScore && _playerScore < _cpuScore - 1)
        {
            // - Computer Wins
            _hudManager.WinCpu.enabled = true;
            GameOver();
        }
    }

    /// <summary>
    /// Spawn Ball
    /// </summary>
    private void SpawnBall()
    {
        _ball.SetActive(true);
        _ball.transform.localPosition = new Vector3(12, 0, -2);
        _ball.GetComponent<BallManager>().BallMoveSpeed = 12f;
    }

    /// <summary>
    /// Init Start Game
    /// </summary>
    private void StartGame()
    {
        _playerScore = 0;
        _cpuScore = 0;

        _hudManager.PlayAgain.transform.parent.gameObject.SetActive(false);
        _hudManager.WinPlayer.enabled = false;
        _hudManager.WinCpu.enabled = false;

        _hudManager.PlayerScore.text = "0";
        _hudManager.CpuScore.text = "0";

        GameState = GameStateEnum.Playing;
        _cpuPaddle.transform.localPosition = new Vector3(13, 0, -2);
        SpawnBall();
    }

    /// <summary>
    /// Init next round
    /// </summary>
    private void InitNextRound()
    {
        if (GameState != GameStateEnum.Playing) return;

        _cpuPaddle.transform.localPosition = new Vector3(13, 0, -2);

        _cpuPaddle.GetComponent<CpuPaddleManager>().ResetMoveSpeed();
        _cpuPaddle.GetComponent<CpuPaddleManager>().IncreaceMoveSpeedBy(_cpuScore + _playerScore);

        SpawnBall();
        _ball.GetComponent<BallManager>().BallMoveSpeed += _cpuScore + _playerScore;
    }

    /// <summary>
    /// Pause or Resume Game
    /// </summary>
    private void PauseResumeGame()
    {
        if (GameState == GameStateEnum.Paused)
        {
            GameState = GameStateEnum.Playing;
            _ball.SetActive(true);
            _hudManager.PlayAgain.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            GameState = GameStateEnum.Paused;
            _ball.SetActive(false);
            _hudManager.PlayAgain.transform.parent.gameObject.SetActive(true);
            _hudManager.PlayAgain.text = "GAME IS PAUSED PRESS SPACE TO CONTINUE";
        }
    }

    /// <summary>
    /// Init game over
    /// </summary>
    private void GameOver()
    {
        _ball.SetActive(false);

        _cpuPaddle.GetComponent<CpuPaddleManager>().ResetMoveSpeed();

        _hudManager.PlayAgain.transform.parent.gameObject.SetActive(true);
        _hudManager.PlayAgain.text = "PRESS SPACE TO PLAY AGAIN";

        GameState = GameStateEnum.GameOver;
    }
}