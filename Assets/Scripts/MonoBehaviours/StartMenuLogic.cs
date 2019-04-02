using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuLogic : MonoBehaviour
{
    private int counter = 0;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            counter++;

            switch (counter)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:
                    
                    break;
                case 4:

                    break;
            }
        }
    }
}
