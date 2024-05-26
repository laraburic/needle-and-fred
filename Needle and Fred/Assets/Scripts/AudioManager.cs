using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] rSounds;
    public AudioMixer mixer;

    public static bool pause;

    private System.Random random = new System.Random();

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in rSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        while (!pause)
        {
            float randomDelay = Random.Range(5f, 15f); //Adjust the delay between sounds as needed
            yield return new WaitForSeconds(randomDelay);

            int randomIndex = random.Next(rSounds.Length);
            Sound randomSound = rSounds[randomIndex];
            randomSound.source.Play();

            Debug.Log($"Played random sound: {randomSound.name}");
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
        }
        s.source.Play();
    }
}