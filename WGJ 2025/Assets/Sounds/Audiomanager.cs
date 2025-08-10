using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    [SerializeField] AudioSource musicsource;
   
    public AudioClip backgroud;

    private void Start()
    {
        musicsource.clip = backgroud;
        musicsource.Play();
    }
}
