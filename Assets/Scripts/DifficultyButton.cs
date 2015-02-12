using UnityEngine;
using System.Collections;

public class DifficultyButton : _PressableButton {
        public Material normalSprite;
        public Material hoverSprite;
        public Material pressedSprite;
        public int width = 512;
        public int height = 164;

        public Difficulty difficulty; 
        override
                public Material getNormalSprite ()
        {
                transform.rotation = Quaternion.identity;
                return normalSprite;
        }
        
        override
                public Material getHoverSprite ()
        {
                return hoverSprite;
        }
        
        override
                public Material getPressedSprite ()
        {
                return pressedSprite;
        }
        
        override
                public Vector2 getDimensions ()
        {
                return new Vector2 (width, height);
        }
        
        override
                public float getScaleFactor ()
        {
                return 80;
        }
        
        override
                public void onButtonPressed ()
        {
                switch (difficulty) {
                case Difficulty.Easy:
                        Application.LoadLevel ("EasyGame");
                        break;
                case Difficulty.Normal:
                        Application.LoadLevel ("NormalGame");
                        break;
                case Difficulty.Hard:
                        Application.LoadLevel ("HardGame");
                        break;
                case Difficulty.Lunatic:
                        Application.LoadLevel ("LunaticGame");
                        break;
                case Difficulty.Extra:
                        Application.LoadLevel ("ExtraGame");
                        break;
                default:
                        break;
                }

               
        }
}
