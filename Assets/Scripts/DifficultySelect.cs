using UnityEngine;
using System.Collections;

public class DifficultySelect : MonoBehaviour
{

       
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
