using UnityEngine;

namespace Ozel
{
    public class LevelManager : Singleton<LevelManager>
    {
        private Level level;
        public int levelIndex { get => PlayerPrefs.GetInt(Constants.LEVEL_INDEX, 1); set => PlayerPrefs.SetInt(Constants.LEVEL_INDEX, value); }

        private void Start()
        {
            print("Level Index : " + levelIndex);
            level = Resources.Load<Level>("Levels/Level" + levelIndex);
            if (level != null)
            {
                Instantiate(level.LevelPrefab);
            }
            else
            {
                levelIndex = 1;
                level = Resources.Load<Level>("Levels/Level" + levelIndex);
                Instantiate(level.LevelPrefab);
            }
        }

    }

}
