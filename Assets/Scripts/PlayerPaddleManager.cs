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
    /// initialization
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
        if (transform.localPosition.y <= Model.Config.BottonBounds)
        {
            transform.localPosition =
                new Vector3(transform.localPosition.x, Model.Config.BottonBounds, transform.localPosition.z);
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
        if (transform.localPosition.y >= Model.Config.TopBounds)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Model.Config.TopBounds,
                transform.localPosition.z);
        }
        else
        {
            transform.localPosition += Vector3.up * _moveSpeed * Time.deltaTime;
        }
    }
}