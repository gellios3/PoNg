using System;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    /// <summary>
    /// Ball Move speed
    /// </summary>
    [SerializeField] private float _ballMoveSpeed = Config.DefaultBallSpeed;

    public float BallMoveSpeed
    {
        get { return _ballMoveSpeed; }
        set { _ballMoveSpeed = value; }
    }

    /// <summary>
    /// Ball direction
    /// </summary>
    [SerializeField] private Vector2 _ballDirection = Vector2.left;

    public Vector2 BallDirection
    {
        get { return _ballDirection; }
    }

    /// <summary>
    /// Game Play
    /// </summary>
    [SerializeField] private GameObject _gamePlay;

    /// <summary>
    /// Speed increace timer
    /// </summary>
    private float _speedIncreaseTimer;

    /// <summary>
    /// Bounce angle
    /// </summary>
    private float _bounceAngle;

    /// <summary>
    /// Vector X Y 
    /// </summary>
    private float _vx, _vy;

    /// <summary>
    /// Gameplay manager
    /// </summary>
    public GamePlayManager PlayManager { get; private set; }

    /// <summary>
    /// Initialization
    /// </summary>
    private void Start()
    {
        PlayManager = _gamePlay.GetComponent<GamePlayManager>();

        if (BallMoveSpeed < 0)
        {
            BallMoveSpeed = -1 * BallMoveSpeed;
        }

        _bounceAngle = GetRandomBounceAngle();

        _vx = BallMoveSpeed * Mathf.Cos(_bounceAngle);
        _vy = BallMoveSpeed * -Mathf.Sign(_bounceAngle);
    }

    /// <summary>
    ///   Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (PlayManager.GameState == GamePlayManager.GameStateEnum.Paused) return;

        MoveBall();
        UpdateSpeedIncrease();
    }

    /// <summary>
    /// Update speed increece
    /// </summary>
    private void UpdateSpeedIncrease()
    {
        if (_speedIncreaseTimer >= Config.SpeedIncreaseInterval)
        {
            _speedIncreaseTimer = 0;

            if (BallMoveSpeed > 0)
            {
                BallMoveSpeed += Config.SpeedIncreaseBy;
            }
            else
            {
                BallMoveSpeed -= Config.SpeedIncreaseBy;
            }

            gameObject.GetComponent<ColisionManager>().CpuPaddleManager.IncreaceMoveSpeedBy();
        }
        else
        {
            _speedIncreaseTimer += Time.deltaTime;
        }
    }

    /// <summary>
    ///  Move ball
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void MoveBall()
    {
        var colisionManager = gameObject.GetComponent<ColisionManager>();
        if (colisionManager.CheckCollision())
        {
            if (BallMoveSpeed < 0)
            {
                BallMoveSpeed = -1 * BallMoveSpeed;
            }

            switch (colisionManager.ColliderWith)
            {
                case ColisionManager.ColliderWithEnum.Player:
                    _ballDirection.x = 1;
                    _bounceAngle = colisionManager.GetBounceAngle(GamePlayManager.PaddleState.Player);
                    break;
                
                case ColisionManager.ColliderWithEnum.Cpu:
                    _ballDirection.x = -1;  
                    _bounceAngle = colisionManager.GetBounceAngle(GamePlayManager.PaddleState.Cpu);
                    break;
                
                case ColisionManager.ColliderWithEnum.Wall:
                    colisionManager.ColliderWith = ColisionManager.ColliderWithEnum.None;
                    _bounceAngle = -_bounceAngle;
                    break;
                
                case ColisionManager.ColliderWithEnum.None:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        _vx = BallMoveSpeed * Mathf.Cos(_bounceAngle);
        _vy = BallMoveSpeed > 0 ? BallMoveSpeed * -Mathf.Sin(_bounceAngle) : BallMoveSpeed * Mathf.Sin(_bounceAngle);

        transform.localPosition += new Vector3(BallDirection.x * _vx * Time.deltaTime, _vy * Time.deltaTime, 0);
    }

    /// <summary>
    /// Get random bounce angle
    /// </summary>
    /// <param name="minDegreese"></param>
    /// <param name="maxDegreese"></param>
    /// <returns></returns>
    private static float GetRandomBounceAngle(float minDegreese = 160f, float maxDegreese = 260f)
    {
        var minRad = minDegreese * Mathf.PI / 180;
        var maxRad = maxDegreese * Mathf.PI / 180;

        return Random.Range(minRad, maxRad);
    }
}