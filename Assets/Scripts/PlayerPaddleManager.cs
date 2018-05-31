using Model;
using UnityEngine;

public class PlayerPaddleManager : MonoBehaviour
{
    /// <summary>
    ///  Move speed paddle
    /// </summary>
    [SerializeField] private float _moveSpeed = 8.0f;

    /// <summary>
    /// Gameplay game object
    /// </summary>
    [SerializeField] private GameObject _game;

    /// <summary>
    /// Start Position
    /// </summary>
    private readonly Vector2 _startingPosition = new Vector2(-13, 0);

    /// <summary>
    /// Gameplay manager
    /// </summary>
    private GamePlayManager _gamePlayManager;

    /// <summary>
    /// Player paddle attriibutes
    /// </summary>
    public PaddleAttrs PlayerPaddleAttrs;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _gamePlayManager = _game.GetComponent<GamePlayManager>();
        transform.localPosition = _startingPosition;

        var playerSprite = transform.GetComponent<SpriteRenderer>();

        PlayerPaddleAttrs.Height = playerSprite.bounds.size.y;
        PlayerPaddleAttrs.Widht = playerSprite.bounds.size.x;

        PlayerPaddleAttrs.MaxX = transform.localPosition.x + PlayerPaddleAttrs.Widht / 2;
        PlayerPaddleAttrs.MinX = transform.localPosition.x - PlayerPaddleAttrs.Widht / 2;
    }

    public void InitVerticalParams()
    {
        PlayerPaddleAttrs.MaxY = transform.localPosition.y + PlayerPaddleAttrs.Height / 2;
        PlayerPaddleAttrs.MinY = transform.localPosition.y - PlayerPaddleAttrs.Height / 2;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (_gamePlayManager.GameState == GamePlayManager.GameStateEnum.Playing)
        {
            CheckUserInput();
        }
    }

    /// <summary>
    /// Check user input
    /// </summary>
    private void CheckUserInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveDown();
        }
    }

    /// <summary>
    /// Move down
    /// </summary>
    private void MoveDown()
    {
        var position = transform.localPosition;
        if (position.y <= Config.BottonBounds)
        {
            transform.localPosition = new Vector3(position.x, Config.BottonBounds, position.z);
        }
        else
        {
            transform.localPosition += Vector3.down * _moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Move Up
    /// </summary>
    private void MoveUp()
    {
        var position = transform.localPosition;
        if (position.y >= Config.TopBounds)
        {
            transform.localPosition = new Vector3(position.x, Config.TopBounds, position.z);
        }
        else
        {
            transform.localPosition += Vector3.up * _moveSpeed * Time.deltaTime;
        }
    }
}