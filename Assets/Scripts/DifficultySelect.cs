using UnityEngine;
using System.Collections;

public class DifficultySelect : MonoBehaviour
{

        // Use this for initialization
        void Start ()
        {
    
        }
    
        // Update is called once per frame
        void Update ()
        {
    
        }

        public void EasyMode ()
        {
                Application.LoadLevel ("EasyGame");
        }

        public void NormalMode ()
        {
                Application.LoadLevel ("NormalGame");
        }

        public void HardMode ()
        {
                Application.LoadLevel ("HardGame");
        }

        public void InsaneMode ()
        {
                Application.LoadLevel ("InsaneGame");
        }

        public void ExtraMode ()
        {
                Application.LoadLevel ("ExtraGame");
        }



}
