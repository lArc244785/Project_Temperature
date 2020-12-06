using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumInfo 
{
    public enum DamageType
    {
        Nomal, Hot, Cold
    };

    public enum Command
    {
        NoCommand ,Attack
    };

    public enum Materia
    {
        Idle, Hit, Ghost
    };

    public enum PrefabEnemy
    {
        MallangBasic = 0, MallangSpring = 1,
        EggDragon=2
    };
}
