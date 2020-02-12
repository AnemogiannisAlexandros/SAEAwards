using UnityEngine;

namespace MyMultiplayerProject
{
    
    public class GameManager
    {
        public const float MYSTERY_ITEMS_SPAWN_TIME = 2.0f;

        public const float PLAYER_RESPAWN_TIME = 2.0f;

        public const int PLAYER_MAX_LIVES = 9;

        public const int PLAYER_ITEM = -1;


        public const string PLAYER_LIVES = "PlayerLives";
        public const string PLAYER_CURRENT_ITEM = "PlayerItem";
        public const string PLAYER_READY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
        public const string PLAYER_IS_DEAD = "IsDead";


        public Color color;


        public static Sprite GetTexutre(int textureChoice) 
        {
             return Resources.Load<Sprite>("Textures/Player0" + (textureChoice+1) + "_Icon");
        }

        public static Color32 GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return new Color(0.259434f, 0.5553225f, 1,1);
                case 1: return new Color(1, 0.2588235f, 0.2923419f,1);
                case 2: return new Color(0.3344353f, 0.764151f, 0.2919634f, 1);
                case 3: return new Color(1, 0.969906f, 0.2877358f, 1);
            }

            return Color.black;
        }
    }
}
