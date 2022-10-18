using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    [System.Serializable]
    public struct Music
    {
        public string SceneType;
        public AudioClip audio;
    }
    public Music[] MusicList;

    private AudioSource MusicType;
    string NowMusic = "Main Menu";

    private void Awake()
    {
        MusicType = gameObject.AddComponent<AudioSource>();
        MusicType.loop = true;
    }
    private void Update()
    {
        if (MusicList.Length >= 0)
        {
            if (SceneManager.GetActiveScene().name == "Main Menu")
            {
                PlayMusic(MusicList[0].SceneType);
            }
            else if (SceneManager.GetActiveScene().name == "Village Scene")
            {
                PlayMusic(MusicList[1].SceneType);
            }
            else if (SceneManager.GetActiveScene().name == "Dungeon Scene")
            {
                PlayMusic(MusicList[2].SceneType);
            }

        }
    }

    public void PlayMusic(string name)
    {
        if (NowMusic.Equals(name)) { return; }

        for (int i = 0; i< MusicList.Length; ++i)
        {
            if (MusicList[i].SceneType.Equals(name))
            {
                MusicType.clip = MusicList[i].audio;
                MusicType.Play();
                NowMusic = name;
            }
        }
        
    }

   

}
