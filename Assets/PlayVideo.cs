using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class PlayVideoOnLoad : MonoBehaviour
{
    public VideoPlayer _VideoPlayer;
    // Use this for initialization
    void Start()
    {
        _VideoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
