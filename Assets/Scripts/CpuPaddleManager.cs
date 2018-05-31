using Model;
using UnityEngine;

public class CpuPaddleManager : MonoBehaviour
{
    /// <summary>
    ///  Move speed paddle
    /// </summary>
    [SerializeField] private float _moveSpeed = 6.0f;

    /// <summary>
    /// Gameplay game object
    /// </summary>
    [SerializeField] private GameObject _game;
    
    /// <summary>
    /// Start Position
    /// </summary>
    private readonly Vector2 _startingPosition = new Vector2(13, 0);

    /// <summary>
    /// Ball game object
    /// </summary>
    private GameObject _ball;

    /// <summary>
    /// Gameplay manager
    /// </summary>
    private GamePlayManager _gamePlayManager;

    /// <summary>
    /// Init metod
    /// </summary>
    private void Start()
    {
        _gamePlayManager = _game.GetComponent<GamePlayManager>();
        transform.localPosition = _startingPosition;
    }
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (_gamePlayManager.GameState == GamePlayManager.GameStateEnum.Playing)
        {
            CpuMovePaddle();
        }
    }

    /// <summary>
    /// Reset move speed
    /// </summary>
    public void ResetMoveSpeed()
    {
        _moveSpeed = 6;
    }

    /// <summary>
    /// Increace move speed by value
    /// </summary>
    /// <param name="value"></param>
    public void IncreaceMoveSpeedBy(int value = 1)
    {
        if (_moveSpeed + value <= 13)
        {
            _moveSpeed += value;
        }
    }

    /// <summary>
    /// Move CPU pandle per frame
    /// </summary>
    private void CpuMovePaddle()
    {
        if (!_ball)
        {
            _ball = GameObject.FindGameObjectWithTag("ball");
        }

        if (_ball.GetComponent<BallManager>().BallDirection != Vector2.right) return;
        
        var ballPos = _ball.transform.localPosition;

        if (transform.localPosition.y > Model.Config.BottonBounds && ballPos.y < transform.localPosition.y)
        {
            transform.localPosition += new Vector3(0, -_moveSpeed * Time.deltaTime, 0);
        }

        if (transform.localPosition.y < Model.Config.TopBounds && ballPos.y > transform.localPosition.y)
        {
            transform.localPosition += new Vector3(0, _moveSpeed * Time.deltaTime, 0);
        }
    }
}