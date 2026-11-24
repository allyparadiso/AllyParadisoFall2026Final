using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private GameManager gameManager;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI currentScoreText;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void UpdateCurrentScoreUI()
    {
        
    }

    public void UpdateTotalScoreUI()
    {
        
    }
}
