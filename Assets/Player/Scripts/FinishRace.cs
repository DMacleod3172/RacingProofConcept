using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishRace : MonoBehaviour
{
    public Text raceFinishedText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (raceFinishedText != null)
            {
                raceFinishedText.text = "Race Finished";
            }
        }
    }
}
