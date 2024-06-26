using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonPlayerPrefs
    {
        public GameObject gameObject;
        public string playerPrefKey;
        public Sprite noStar;
        public Sprite Star;
    }

    public ButtonPlayerPrefs[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int score = PlayerPrefs.GetInt(buttons[i].playerPrefKey, 0);


            Image buttonImage = buttons[i].gameObject.GetComponent<Image>();

            if (buttonImage == null)
            {
                buttonImage = buttons[i].gameObject.GetComponentInChildren<Image>();
            }

            for (int starId = 1; starId <= 3;  starId++)
            {
                Transform star = buttons[i].gameObject.transform.Find("star" + starId);

                if (starId <= score)
                {
                    buttonImage.sprite = buttons[i].Star;
                } else
                {
                    buttonImage.sprite = buttons[i].noStar;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Button clicked");
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelOverview()
    {
        SceneManager.LoadScene("LevelOverview");
    }
}
