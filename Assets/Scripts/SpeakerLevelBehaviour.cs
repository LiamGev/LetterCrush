using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerLevelBehaviour : MonoBehaviour
{

    public int goal;
    int streak = 0;
    int answer;
    public Slider progress;
    


    string selectedAudio;

    // Start is called before the first frame update
    void Start()
    {
        progress.maxValue = goal;
        selectSound();
        playSound();

    }

    // Update is called once per frame
    void Update()
    {
        progress.value = streak;

        if (streak == goal)
        {
            Debug.Log("level completed");
        }
    }


    public void playSound()
    {
        FindObjectOfType<AudioManager>().Play(selectedAudio);
    }

    public void selectSound()
    {
        float choice = Random.Range(1, 9);
        Debug.Log(choice);
        if(choice == 1 || choice == 5 )
        {
            selectedAudio = "1";
            answer = 1;
        }
        if(choice == 2 || choice == 6 )
        {
            selectedAudio = "2";
            answer = 2;
        }
        if (choice == 3 || choice == 7)
        {
            selectedAudio = "3";
            answer = 3;
        }
        if (choice == 4 || choice == 8)
        {
            selectedAudio = "4";
            answer = 4;
        }
    }

    public void checkAnswer(int choice)
    {

        if (choice == answer)
        {
            Debug.Log("Correct");
            streak += 1;
            if (streak != goal)
            {
                selectSound();
                playSound();
                
            }

        }
        else if (choice != answer)
        {
            Debug.Log("Fout");
            streak = 0;
        }

    }
}
