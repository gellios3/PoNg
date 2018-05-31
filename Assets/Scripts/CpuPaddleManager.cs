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
    /// Ball game object
    /// </summary>
    [SerializeField] private GameObject _ball;

    /// <summary>
    /// Start Position
    /// </summary>
    private readonly Vector2 _startingPosition = new Vector2(13, 0);

    /// <summary>
    /// Gameplay manager
    /// </summary>
    private GamePlayManager _gamePlayManager;

    /// <summary>
    /// Cpu paddle attributes
    /// </summary>
    public PaddleAttrs CpuPaddleAttrs;

    /// <summary>
    /// Init metod
    /// </summary>
    private void Start()
    {
        _gamePlayManager = _game.GetComponent<GamePlayManager>();
        transform.localPosition = _startingPosition;

        CpuPaddleAttrs.Height = transform.GetComponent<SpriteRenderer>().bounds.size.y;
        CpuPaddleAttrs.Widht = transform.GetComponent<SpriteRenderer>().bounds.size.x;

        CpuPaddleAttrs.MaxX = transform.localPosition.x - CpuPaddleAttrs.Widht / 2;
        CpuPaddleAttrs.MinX = transform.localPosition.x + CpuPaddleAttrs.Widht / 2;
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

    public void InitVerticalParams()
    {
        CpuPaddleAttrs.MaxY = transform.localPosition.y + CpuPaddleAttrs.Height / 2;
        CpuPaddleAttrs.MinY = transform.localPosition.y - CpuPaddleAttrs.Height / 2;
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
        if (_ball.GetComponent<BallManager>().BallDirection != Vector2.right) return;

        var ballPos = _ball.transform.localPosition;

        if (transform.localPosition.y > Config.BottonBounds && ballPos.y < transform.localPosition.y)
        {
            transform.localPosition += new Vector3(0, -_moveSpeed * Time.deltaTime, 0);
        }

        if (transform.localPosition.y < Config.TopBounds && ballPos.y > transform.localPosition.y)
        {
            transform.localPosition += new Vector3(0, _moveSpeed * Time.deltaTime, 0);
        }
    }
}