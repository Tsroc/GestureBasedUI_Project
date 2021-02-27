using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _as;

    void Awake()
    {
        SetupSingleton();
    }

    /*
        Setup as a singleton
    */
    private void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public void Play()
    {
        _as.Play();
    }

    public void Stop()
    {
        _as.Stop();
    }

}