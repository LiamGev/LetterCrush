using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject Letter1;
    public GameObject Letter2;
    public GameObject Letter3;
    public GameObject Letter4;


    // Start is called before the first frame update
    void Start()
    {
       startGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void startGrid()
    {
        for (int i = 0; i < 5; i++)
        {
            float xPos = -5;
            if (i == 0)
            {
                xPos = -1.81f;
            }
            if (i == 1)
            {
                xPos = -0.925f;
            }
            if (i == 2)
            {
                xPos = -0.029f;
            }
            if (i == 3)
            {
                xPos = 0.864f;
            }
            if (i == 4)
            {
                xPos = 1.75f;
            }
            for (int j = 0; j < 5; j++)
            {
                float type = Random.Range(1, 5);
                Vector3 position = new Vector3(xPos, 6 + j, 0);

                if (type == 1)
                {
                    Instantiate(Letter1, position, Quaternion.identity);
                }
                if (type == 2)
                {
                    Instantiate(Letter2, position, Quaternion.identity);
                }
                if (type == 3)
                {
                    Instantiate(Letter3, position, Quaternion.identity);
                }
                if (type == 4)
                {
                    Instantiate(Letter4, position, Quaternion.identity);
                }


            }

        }
    }
}
