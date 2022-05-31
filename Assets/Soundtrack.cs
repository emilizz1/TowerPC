using UnityEngine;
using UnityEngine.SceneManagement;

public class Soundtrack : MonoBehaviour
{
    [SerializeField] AudioClip forestSoundtrack;
    [SerializeField] AudioClip desertSoundtrack;
    [SerializeField] AudioSource mySource;

    private void Awake()
    {
       // if(SceneManager.GetActiveScene().name == "LevelSelection")
        //{
            //if (SavedData.GetInt("DesertLevelsUnlocked") == 1)
            //{
            //    mySource.clip = desertSoundtrack;
            //    mySource.Play();
            //}
            //else
            //{
            //    mySource.clip = forestSoundtrack;
            //    mySource.Play();
            //}
            //mySource.clip = forestSoundtrack;
            //mySource.Play();
        //}

        if (FindObjectsOfType<Soundtrack>().Length > 1)
        {
            foreach (Soundtrack soundtrack in FindObjectsOfType<Soundtrack>())
            {
                if (soundtrack != this)
                {
                    if (soundtrack.mySource.clip != mySource.clip)
                    {
                        Destroy(soundtrack.gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
