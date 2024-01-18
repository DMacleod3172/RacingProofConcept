using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{

    public TimeTrialTimer timer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimeTrialTimer timer = GetComponentInParent<TimeTrialTimer>();
            timer.AddCheckpointTime();

            gameObject.SetActive(false);
        }
    }
}

