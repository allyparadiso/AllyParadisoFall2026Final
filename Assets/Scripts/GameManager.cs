using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CharacterController _controller;
    private float scoringAmount = 300;
    private float airTime = 0;
    public float totalScore;
    public float currentScore;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        totalScore = 0;
    }

    public void StartScoring()
    {
        StartCoroutine(Timer());
    }

    public void EndScoring()
    {
        StopAllCoroutines();
    }

    public void AddToTotalScore(float score)
    {
        totalScore += score;
    }

    private IEnumerator Timer()
    {
        if (_controller.isGrounded == false)
        {
            airTime += Time.deltaTime;
        }
        if (_controller.isGrounded) 
        {
            currentScore = airTime * scoringAmount;
            AddToTotalScore(currentScore);
            yield return null;
        }
    }
}
