using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class CountdownCoroutine : MonoBehaviour
{
    [SerializeField] int startCount = 3;
    [SerializeField] UnityEvent onCountdownFinished;

    TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Execute()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        for (int count = startCount; count > 0; count--)
        {
            text.text = count.ToString();
            yield return new WaitForSeconds(1f);
        }

        onCountdownFinished.Invoke();
    }
}
