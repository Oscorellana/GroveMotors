using UnityEngine;
using TMPro;

/// <summary>
/// Countdown timer that displays remaining time in MM:SS format.
/// Activates <see cref="gameOverText"/> when time reaches zero.
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    [SerializeField] private TextMeshProUGUI gameOverText;

    void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                gameOverText.gameObject.SetActive(true);
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
