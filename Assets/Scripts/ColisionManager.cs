using Model;
using UnityEngine;


public class ColisionManager : MonoBehaviour
{
    /// <summary>
    /// Player Paddle
    /// </summary>
    [SerializeField] private GameObject _playerPaddle;

    /// <summary>
    /// Cpu paddle
    /// </summary>
    [SerializeField] private GameObject _cpuPaddle;

    /// <summary>
    /// Collider wight state
    /// </summary>
    private ColliderWithEnum _colliderWith = ColliderWithEnum.None;

    public ColliderWithEnum ColliderWith
    {
        get { return _colliderWith; }
        set { _colliderWith = value; }
    }

    /// <summary>
    /// Collider With Enum
    /// </summary>
    public enum ColliderWithEnum
    {
        Player,
        Cpu,
        Wall,
        None
    }

    /// <summary>
    /// Ball Width and Height
    /// </summary>
    private float _ballWidth, _ballHeight;

    /// <summary>
    /// Is assigned point
    /// </summary>
    private bool _isAssingnedPoint;

    /// <summary>
    /// Player paddle manager
    /// </summary>
    private PlayerPaddleManager _playerPaddleManager;

    /// <summary>
    /// Cpu paddle manager
    /// </summary>
    public CpuPaddleManager CpuPaddleManager { get; private set; }

    private void Start()
    {
        // Init ball attributes
        var ballSprite = transform.GetComponent<SpriteRenderer>();

        _playerPaddleManager = _playerPaddle.GetComponent<PlayerPaddleManager>();
        CpuPaddleManager = _cpuPaddle.GetComponent<CpuPaddleManager>();

        _ballHeight = ballSprite.bounds.size.y;
        _ballWidth = ballSprite.bounds.size.x;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal bool CheckCollision()
    {
        return CheckCpuColision() || CheckPlayerColision() || CheckWallColision();
    }

    /// <summary>
    /// Get bounce angle
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    internal float GetBounceAngle(GamePlayManager.PaddleState state)
    {
        _colliderWith = ColliderWithEnum.None;

        var paddleAttrs = _playerPaddleManager.PlayerPaddleAttrs;
        var paddle = _playerPaddle;

        if (state == GamePlayManager.PaddleState.Cpu)
        {
            paddleAttrs = CpuPaddleManager.CpuPaddleAttrs;
            paddle = _cpuPaddle;
        }

        var relativeIntersectY = paddle.transform.localPosition.y - transform.localPosition.y;
        var normalizeRalativeIntersectY = relativeIntersectY / (paddleAttrs.Height / 2);
        return normalizeRalativeIntersectY * (Config.MaxAngle * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Check player position
    /// </summary>
    /// <returns></returns>
    private bool CheckPlayerColision()
    {
        _playerPaddle.GetComponent<PlayerPaddleManager>().InitVerticalParams();

        var cpuPaddleAttrs = _cpuPaddle.GetComponent<CpuPaddleManager>().CpuPaddleAttrs;

        var ballMaxX = transform.localPosition.x - _ballWidth / 2;
        var ballMinX = transform.localPosition.x + _ballWidth / 2;

        if (!(ballMinX > cpuPaddleAttrs.MaxX) || !(ballMaxX / 2 < cpuPaddleAttrs.MinX)) return false;

        if (transform.localPosition.y - _ballHeight / 2 < cpuPaddleAttrs.MaxY &&
            transform.localPosition.y + _ballHeight / 2 > cpuPaddleAttrs.MinY)
        {
            _colliderWith = ColliderWithEnum.Cpu;
            return true;
        }

        // if ball out bounce and screen recet ball position
        if (Mathf.Abs(ballMinX - cpuPaddleAttrs.MaxX) > 1 && !_isAssingnedPoint)
        {
            _isAssingnedPoint = true;
            gameObject.GetComponent<BallManager>().PlayManager.IncreacePlayerPoint();
        }
        else
        {
            _isAssingnedPoint = false;
        }

        return false;
    }

    /// <summary>
    /// Check Cpu position
    /// </summary>
    /// <returns></returns>
    private bool CheckCpuColision()
    {
        // init vertical params
        _cpuPaddle.GetComponent<CpuPaddleManager>().InitVerticalParams();

        // get player and CPU paddle attrs
        var playerPaddleAttrs = _playerPaddle.GetComponent<PlayerPaddleManager>().PlayerPaddleAttrs;

        var ballMaxX = transform.localPosition.x - _ballWidth / 2;
        var ballMinX = transform.localPosition.x + _ballWidth / 2;

        // check for x collision
        if (!(ballMaxX < playerPaddleAttrs.MaxX) || !(ballMinX > playerPaddleAttrs.MinX)) return false;

        // then check for y collision
        if (transform.localPosition.y - _ballHeight / 2 < playerPaddleAttrs.MaxY &&
            transform.localPosition.y + _ballHeight / 2 > playerPaddleAttrs.MinY)
        {
            _colliderWith = ColliderWithEnum.Player;
            return true;
        }

        // if ball out bounce and screen recet ball position
        if (Mathf.Abs(ballMaxX - playerPaddleAttrs.MaxX) > 1 && !_isAssingnedPoint)
        {
            _isAssingnedPoint = true;
            gameObject.GetComponent<BallManager>().PlayManager.IncreaceCpuPoint();
        }
        else
        {
            _isAssingnedPoint = false;
        }

        return false;
    }

    /// <summary>
    /// Check wall 
    /// </summary>
    /// <returns></returns>
    private bool CheckWallColision()
    {
        if (transform.localPosition.y > Config.TopBounds)
        {
            _colliderWith = ColliderWithEnum.Wall;
        }

        if (_colliderWith != ColliderWithEnum.Wall && transform.localPosition.y < Config.BottonBounds)
        {
            _colliderWith = ColliderWithEnum.Wall;
        }

        return _colliderWith == ColliderWithEnum.Wall;
    }
}