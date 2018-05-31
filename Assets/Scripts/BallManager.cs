using Model;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    /// <summary>
    /// Ball Move speed
    /// </summary>
    [SerializeField] private float _ballMoveSpeed = 12f;

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
    /// Speed increace timer
    /// </summary>
    private float _speedIncreaseTimer;

    private float _ballWidth, _ballHeight;

    private GameObject _playerPaddle, _cpuPaddle;

    private float _bounceAngle;

    private float _vx, _vy;

    private const float MaxAngle = 45;

    private bool _collidedWithPlayer, _collidedWithAi, _collidedWithWall;

    private GamePlayManager _gamePlayManager;

    private bool _isAssingnedPoint;

    private PaddleAttrs _playerPaddleAttrs;
    private PaddleAttrs _cpuPaddleAttrs;

    // Use this for initialization
    private void Start()
    {
        _gamePlayManager = GameObject.Find("GamePlay").GetComponent<GamePlayManager>();

        if (BallMoveSpeed < 0)
        {
            BallMoveSpeed = -1 * BallMoveSpeed;
        }

        _playerPaddle = GameObject.Find("PlayelPaddle");
        _cpuPaddle = GameObject.Find("CpuPaddle");

        var playerSprite = _playerPaddle.transform.GetComponent<SpriteRenderer>();
        var aiSprite = _cpuPaddle.transform.GetComponent<SpriteRenderer>();
        var ballSprite = transform.GetComponent<SpriteRenderer>();

        _playerPaddleAttrs.State = GamePlayManager.PaddleState.Player;
        _cpuPaddleAttrs.State = GamePlayManager.PaddleState.Cpu;

        _playerPaddleAttrs.Height = playerSprite.bounds.size.y;
        _playerPaddleAttrs.Widht = playerSprite.bounds.size.x;

        _cpuPaddleAttrs.Height = aiSprite.bounds.size.y;
        _cpuPaddleAttrs.Widht = aiSprite.bounds.size.x;

        _ballHeight = ballSprite.bounds.size.y;
        _ballWidth = ballSprite.bounds.size.x;

        _playerPaddleAttrs.MaxX = _playerPaddle.transform.localPosition.x + _playerPaddleAttrs.Widht / 2;
        _playerPaddleAttrs.MinX = _playerPaddle.transform.localPosition.x - _playerPaddleAttrs.Widht / 2;

        _cpuPaddleAttrs.MaxX = _cpuPaddle.transform.localPosition.x - _cpuPaddleAttrs.Widht / 2;
        _cpuPaddleAttrs.MinX = _cpuPaddle.transform.localPosition.x + _cpuPaddleAttrs.Widht / 2;

        _bounceAngle = GetRandomBounceAngle();

        _vx = BallMoveSpeed * Mathf.Cos(_bounceAngle);
        _vy = BallMoveSpeed * -Mathf.Sign(_bounceAngle);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gamePlayManager.GameState != GamePlayManager.GameStateEnum.Paused)
        {
            MoveBall();
            UpdateSpeedIncrease();
        }
    }

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

            _cpuPaddle.GetComponent<CpuPaddleManager>().IncreaceMoveSpeedBy();
        }
        else
        {
            _speedIncreaseTimer += Time.deltaTime;
        }
    }

    private bool CheckCollision()
    {
        // The top & bottom edges of the paddles in motion:
        _playerPaddleAttrs.MaxY = _playerPaddle.transform.localPosition.y + _playerPaddleAttrs.Height / 2;
        _playerPaddleAttrs.MinY = _playerPaddle.transform.localPosition.y - _playerPaddleAttrs.Height / 2;

        _cpuPaddleAttrs.MaxY = _cpuPaddle.transform.localPosition.y + _cpuPaddleAttrs.Height / 2;
        _cpuPaddleAttrs.MinY = _cpuPaddle.transform.localPosition.y - _cpuPaddleAttrs.Height / 2;

        var ballMaxX = transform.localPosition.x - _ballWidth / 2;
        var ballMinX = transform.localPosition.x + _ballWidth / 2;

        // check for x collision
        if (ballMaxX < _playerPaddleAttrs.MaxX && ballMinX > _playerPaddleAttrs.MinX)
        {
            // then check for y collision
            if (transform.localPosition.y - _ballHeight / 2 < _playerPaddleAttrs.MaxY &&
                transform.localPosition.y + _ballHeight / 2 > _playerPaddleAttrs.MinY)
            {
                _collidedWithPlayer = true;
                return true;
            }

            // if ball out bounce and screen recet ball position
            if (Mathf.Abs(ballMaxX - _playerPaddleAttrs.MaxX) > 1 && !_isAssingnedPoint)
            {
                _isAssingnedPoint = true;
                _gamePlayManager.IncreaceCpuPoint();
            }
        }

        if (ballMinX > _cpuPaddleAttrs.MaxX && ballMaxX / 2 < _cpuPaddleAttrs.MinX)
        {
            if (transform.localPosition.y - _ballHeight / 2 < _cpuPaddleAttrs.MaxY &&
                transform.localPosition.y + _ballHeight / 2 > _cpuPaddleAttrs.MinY)
            {
                _collidedWithAi = true;
                return true;
            }

            // if ball out bounce and screen recet ball position
            if (Mathf.Abs(ballMinX - _cpuPaddleAttrs.MaxX) > 1 && !_isAssingnedPoint)
            {
                _isAssingnedPoint = true;
                _gamePlayManager.IncreacePlayerPoint();
            }
        }

        if (transform.localPosition.y > Config.TopBounds)
        {
            _collidedWithWall = true;
            return true;
        }

        if (transform.localPosition.y < Config.BottonBounds)
        {
            _collidedWithWall = true;
            return true;
        }

        return false;
    }


    private void MoveBall()
    {
        if (CheckCollision())
        {
            if (BallMoveSpeed < 0)
            {
                BallMoveSpeed = -1 * BallMoveSpeed;
            }

            if (_collidedWithPlayer)
            {
                _ballDirection.x = 1;
                _collidedWithPlayer = false;

                var relativeIntersectY = _playerPaddle.transform.localPosition.y - transform.localPosition.y;
                var normalizeRalativeIntersectY = relativeIntersectY / (_playerPaddleAttrs.Height / 2);
                _bounceAngle = normalizeRalativeIntersectY * (MaxAngle * Mathf.Deg2Rad);
            }
            else if (_collidedWithAi)
            {
                _ballDirection.x = -1;
                _collidedWithAi = false;

                var relativeIntersectY = _cpuPaddle.transform.localPosition.y - transform.localPosition.y;
                var normalizeRalativeIntersectY = relativeIntersectY / (_cpuPaddleAttrs.Height / 2);
                _bounceAngle = normalizeRalativeIntersectY * (MaxAngle * Mathf.Deg2Rad);
            }
            else if (_collidedWithWall)
            {
                _collidedWithWall = false;
                _bounceAngle = -_bounceAngle;
            }
        }

        _vx = BallMoveSpeed * Mathf.Cos(_bounceAngle);

        if (BallMoveSpeed > 0)
        {
            _vy = BallMoveSpeed * -Mathf.Sin(_bounceAngle);
        }
        else
        {
            _vy = BallMoveSpeed * Mathf.Sin(_bounceAngle);
        }

        transform.localPosition += new Vector3(BallDirection.x * _vx * Time.deltaTime, _vy * Time.deltaTime, 0);
    }

    private static float GetRandomBounceAngle(float minDegreese = 160f, float maxDegreese = 260f)
    {
        var minRad = minDegreese * Mathf.PI / 180;
        var maxRad = maxDegreese * Mathf.PI / 180;

        return Random.Range(minRad, maxRad);
    }
}