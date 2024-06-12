﻿using UnityEngine;

namespace Tqa.DungeonQuest.AbilitySystem
{
    public interface IAbility : IItem
    {
        public const string ITEM_TYPE = "Ability";

        AbilityCaster Caster { get; }
        void Install(AbilityCaster caster);
    }
}
