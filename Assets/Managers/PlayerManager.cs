using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class PlayerManager {

    public static Dictionary<int, int> controllerId = new Dictionary<int, int>();
    public static List<int> controllers = new List<int>(); 
    public static List<GameObject> PlayerObjects = new List<GameObject>();
    private static bool hasSwitched = false;
    public static CharacterClassesEnum[] playerClass = {CharacterClassesEnum.Magician, CharacterClassesEnum.Magician};
    public static int players = 0;
    public static bool[] playerReady = {false, false};
    public static int playersReady = 0;
    public static bool gameStarted = false;
    public static float time = 0f;


	public enum CharacterClassesEnum
    {
        Melee,
        Bowman,
        Magician,
        Dartblower
    }


    public static void Reset() {
        controllerId = new Dictionary<int, int>();
        controllers = new List<int>(); 
        PlayerObjects = new List<GameObject>();
        hasSwitched = false;
        playerClass = new CharacterClassesEnum[] {CharacterClassesEnum.Magician, CharacterClassesEnum.Magician};
        players = 0;
        playerReady = new bool[] {false, false};
        playersReady = 0;
        gameStarted = false;
        time = 0f;
    }

    //public static Ability GetAbility(CharacterClassesEnum playerClass, GameObject go)
    //{
    //    Ability ability = new DashAbility(go);

    //    switch (playerClass)
    //    {
    //        case CharacterClassesEnum.Melee:
    //            ability = new DashAbility(go);
    //            break;
    //        case CharacterClassesEnum.Bowman:
    //            ability = new InvisibilityAbility(go);
    //            break;
    //        case CharacterClassesEnum.Magician:
    //            ability = new SiphonAoeBlood(go);
    //            break;
    //        case CharacterClassesEnum.Dartblower:
    //            ability = new ImmolationAbility(go);
    //            break;
    //        default:
    //            break;
    //    }

    //    return ability;
    //}


    //public static Weapon GetWeapon(CharacterClassesEnum playerClass, GameObject go)
    //{
    //    Weapon weapon = new Gun(go);

    //    switch (playerClass)
    //    {
    //        case CharacterClassesEnum.Melee:
    //            weapon = new PlayerMeleeGun(go);
    //            break;
    //        case CharacterClassesEnum.Bowman:
    //            weapon = new SpecialGun(go);
    //            break;
    //        case CharacterClassesEnum.Magician:
    //            weapon = new ShieldGun(go);
    //            break;
    //        case CharacterClassesEnum.Dartblower:
    //            weapon = new Gun(go);
    //            break;
    //        default:
    //            weapon = new Gun(go);
    //            break;
    //    }
    //    return weapon;
    //}

    // Remove for production?
    public static void MapControllerToPlayer() 
    {
        if (players < 2) {
            for (int i = 1; i <= 16; i++) {
                if (Input.GetButton(Inputs.AButton(i)) && !PlayerManager.controllers.Contains(i)) {
                    controllerId[players] = i;
                    controllers.Add(i);
                    playerReady[players] = true;
                    players += 1;
                    playersReady += 1;
                }
            }

            if (Input.GetKeyDown("space"))
            {
                controllerId[players] = -1;
                controllers.Add(-1);
                playerReady[players] = true;
                players += 1;
                playersReady += 1;
            }
        }
    }

}

public static class LevelManager
{
    public static int TempleFloor = 1;
}
