using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    /// <summary>
    /// Text plauer Score
    /// </summary>
    [SerializeField] private Text _playerScore;
    
    public Text PlayerScore
    {
        get { return _playerScore; }
    }
    
    /// <summary>
    /// Text CPU Score
    /// </summary>
    [SerializeField] private Text _cpuScore;
    
    public Text CpuScore
    {
        get { return _cpuScore; }
    }
    
    /// <summary>
    /// Text Win player
    /// </summary>
    [SerializeField] private Text _winPlayer;
    
    public Text WinPlayer
    {
        get { return _winPlayer; }
    }
    
    /// <summary>
    /// Text Win CPU
    /// </summary>
    [SerializeField] private Text _winCpu;
    
    public Text WinCpu
    {
        get { return _winCpu; }
    }
    
    /// <summary>
    /// Text play again
    /// </summary>
    [SerializeField] private Text _playAgain;

    public Text PlayAgain
    {
        get { return _playAgain; }
    }
}