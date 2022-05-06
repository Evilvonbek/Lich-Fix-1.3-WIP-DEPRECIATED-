using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace LichFix
{
    class ResourcesFinder
    {
        public static BlueprintBuff blessingOfUnlifeBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("e4e9f9169c9b28e40aa2c9d10c369254");
            }
        }

        public static BlueprintUnitFact undeadTypeFact
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintUnitFact>("734a29b693e9ec346ba2951b27987e33");
            }
        }

        public static BlueprintBuff corruptedBloodBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("1419d2e2eee432849b0a596e82b9e0a2");
            }
        }

        public static BlueprintAbility corruptedBloodAbility
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("bbbcaa880ac0fa0479ce3ee8ac937d50");
            }
        }

        public static BlueprintAbilityAreaEffect lichBolsterUndeadAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("47d52975b5b1b8c4699fdd43c6d797f0");
            }
        }

        public static BlueprintBuff eyeOfTheBodakBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("618a7e0d54149064ab3ffa5d9057362c");
            }
        }
        public static BlueprintAbilityAreaEffect eyeOfTheBodakAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("4d05e8decac186940961882647e03c93");
            }
        }
        public static BlueprintAbility eyeOfTheBodakAbility
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d404c44b919667347877e7580e1e7498");
            }
        }
        public static BlueprintAbilityAreaEffect deathGazeAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("d6c0ab2f2828dc0479867fe173984016");
            }
        }

        public static BlueprintAbilityAreaEffect insightfulContemplationAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("cad5dd5157db3304c80399472bb48bdf");
            }
        }

        public static BlueprintAbilityAreaEffect inspireGreatnessAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("23ddd38738bd1d84595f3cdbb8512873");
            }
        }
        ///
        public static BlueprintBuff eclipseChillBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("1d585582fbe72e14aadc5cd7985c06f4");
            }
        }
        public static BlueprintBuff eclipseChillEffectBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("1e82cabbfc9b30c44bcc1354b3daa6f4");
            }
        }
        public static BlueprintFeature eclipseChillFeature
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("731bebb09171d5748b6f08cbe88f8af7");
            }
        }
        public static BlueprintActivatableAbility eclipseChillActivatbleAbility
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintActivatableAbility>("a34b61de2713f604c9971d640ec50b8a");
            }
        }

        public static BlueprintFeature taintedSneakAttackFeature
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("e6ce101a94ac9034b8b55c546e74b9dd");
            }
        }

        public static BlueprintBuff taintedSneakAttackBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("7860e92789511a24dba5906ac8d65f90");
            }
        }

        public static BlueprintAbility negativeEruptionAbility
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("5c377ad96e3fc4f4d9b74eba9d38f4f8");
            }
        }
    }
}