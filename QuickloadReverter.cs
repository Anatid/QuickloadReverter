using UnityEngine;

namespace QuickloadReverter
{
    [KSPAddonFixed(KSPAddon.Startup.MainMenu, true, typeof(QuickloadReverter))]
    public class QuickloadReverter : MonoBehaviour
    {
        bool keyWasReleased = true;

        float lastBackupTime = 0;

        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;

            // Detecting the keypress is pretty hacky :(
            if (Input.GetKeyDown(GameSettings.QUICKLOAD.primary) || Input.GetKeyDown(GameSettings.QUICKLOAD.secondary))
            {
                if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
                {
                    if (keyWasReleased)
                    {
                        if (Time.time - lastBackupTime > 10)
                        {
                            ScreenMessages.PostScreenMessage("Saving pre_quickload.sfs.", 1.5f, ScreenMessageStyle.UPPER_RIGHT);
                            GamePersistence.SaveGame("pre_quickload", HighLogic.SaveFolder, SaveMode.OVERWRITE);
                            keyWasReleased = false;
                            lastBackupTime = Time.time;
                        }
                    }
                }
            }

            // Unfortunately, GetKeyDown resets when the quicksave is loaded, so we have to detect
            // the key release ourselves.
            if (!Input.GetKey(GameSettings.QUICKLOAD.primary) && !Input.GetKey(GameSettings.QUICKLOAD.secondary))
            {
                keyWasReleased = true;
            }
        }
    }
}
