using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using Kingmaker.RuleSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace LichFix
{
    class LichPatcher
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsPatcher
        {
            static bool loaded = false;
            static void Postfix()
            {
                if (loaded) return;
                loaded = true;

                //patchLordBeyondTheGrave();
                patchCorruptedBlood();
                patchEyesOfTheBodak();
                patchEclipseChill();
                patchTaintedSneakAttack();
                patchNegativeEruption();
                patchInsightfulContemplation();
            }
        }

        //static void patchLordBeyondTheGrave()
        //{
        //    //1. patch lord beyond the grave
        //    if (Main.Settings.allowLichAuraAffectLivingWithBlessingOfUnlife)
        //    {
        //        //remove the original undead-like resistance effect
        //        ResourcesFinder.blessingOfUnlifeBuff.RemoveComponents<AddFacts>();
        //        ResourcesFinder.blessingOfUnlifeBuff.RemoveComponents<AddFeatureIfHasFact>();

        //        //add the undead Fact directly
        //        ResourcesFinder.blessingOfUnlifeBuff.AddComponents(new BlueprintComponent[] { Helpers.CreateAddFact(ResourcesFinder.undeadTypeFact) });
        //        Main.Log("Patched: Blessing of Unlife Buff >> add undead Fact >> Lord Beyond The Grave can affect allies with blessing of unlife buff now");

        //        ResourcesFinder.lichBolsterUndeadAura.GetComponent<AbilityAreaEffectBuff>().CheckConditionEveryRound = true;
        //        Main.Log("Patched: Lord Beyond The Grave will check condition every round to apply the buff now");
        //    }

        //    if (Main.Settings.fixBlessingOfUnlifeDoubleSavingWithWorldCrawl)
        //    {
        //        ResourcesFinder.blessingOfUnlifeBuff.AddComponents(new BlueprintComponent[] { 
        //            Helpers.CreateAddSavingThrowBonusAgainstDescriptorComponent(SpellDescriptor.MindAffecting,-8),
        //            Helpers.CreateAddSavingThrowBonusAgainstDescriptorComponent(SpellDescriptor.Death,-8),
        //        });
        //    }
        //}

        static void patchCorruptedBlood()
        {
            //2. patch corrupted blood + metamagic selective affect
            if (Main.Settings.allowCorruptedBloodToSelective)
            {
                ResourcesFinder.corruptedBloodAbility.AvailableMetamagic = Metamagic.Selective | Metamagic.Quicken | Metamagic.Heighten | Metamagic.CompletelyNormal | Metamagic.Extend | Metamagic.Extend;
            }

            var actionOnRandomTargetsAround = ResourcesFinder.corruptedBloodBuff.GetComponent<AddIncomingDamageTrigger>().Actions.Actions.OfType<ContextActionOnRandomTargetsAround>().First<ContextActionOnRandomTargetsAround>();
            var actionOnRandomTargetsAroundSelective = Helpers.CreateContextActionOnRandomTargetsAroundSelective(actionOnRandomTargetsAround);

            //corrupted blood explosion range
            actionOnRandomTargetsAroundSelective.Radius = new Feet(Main.Settings.corruptedBloodRange);

            Main.Log("Patched: corrupted blood buff range from 15 >> " + Main.Settings.corruptedBloodRange);

            ResourcesFinder.corruptedBloodBuff.GetComponent<AddIncomingDamageTrigger>().Actions.Actions[0] = actionOnRandomTargetsAroundSelective;
            Main.Log("Patched: corrupted blood buff will now affect by selective metamagic");

            if (Main.Settings.applySaveOnCorruptedBloodExplosion)
            {
                actionOnRandomTargetsAroundSelective.Actions.Actions.OfType<ContextActionDealDamage>().First<ContextActionDealDamage>().Value.DiceType = DiceType.D2;
                actionOnRandomTargetsAroundSelective.Actions.Actions.OfType<ContextActionDealDamage>().First<ContextActionDealDamage>().Value.DiceCountValue.ValueType = ContextValueType.Rank;
                Main.Log("Patched: corrupted blood now deal 1d2 damage per caster level");

                //add save when cast corrupted blood on target
                var originalActionApplyBuffOnCast = ResourcesFinder.corruptedBloodAbility.GetComponent<AbilityEffectRunAction>().Actions.Actions.OfType<ContextActionApplyBuff>().First<ContextActionApplyBuff>();
                var actionSavingOnCast = Helpers.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(new GameAction[]{
                    Helpers.CreateContextSavedApplyBuff(ResourcesFinder.corruptedBloodBuff, originalActionApplyBuffOnCast.DurationValue, true, false, false, true)
                }));

                ResourcesFinder.corruptedBloodAbility.GetComponent<AbilityEffectRunAction>().Actions.Actions[0] = actionSavingOnCast;

                Main.Log("Patched: corrupted blood now have fortitude save");

                //when target is dead, apply corrupted blood with fortitude save
                var originalActionApplyBuff = actionOnRandomTargetsAroundSelective.Actions.Actions.OfType<ContextActionApplyBuff>().First<ContextActionApplyBuff>();

                var actionSavingOnDead = Helpers.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(new GameAction[]{
                    Helpers.CreateContextSavedApplyBuff(ResourcesFinder.corruptedBloodBuff, originalActionApplyBuff.DurationValue, true, false, false, true)
                }));

                actionOnRandomTargetsAroundSelective.Actions.Actions[0] = actionSavingOnDead;

                Main.Log("Patched: corrupted blood now apply to surround unit with fortitude save when the target is dead");

                ResourcesFinder.corruptedBloodAbility.SetDescription("You corrupt an enemy's blood, making it contagious. If the target failed a fortitude save, it is nauseated for 1 {g|Encyclopedia:Combat_Round}round{/g} per {g|Encyclopedia:Caster_Level}caster level{/g}. " +
                "Every round it can make a new {g|Encyclopedia:Saving_Throw}saving throw{/g} to remove the nauseated condition. If the saving throw is successful, the target instead becomes sickened. If the target dies while nauseated condition, " +
                "it deals {g|Encyclopedia:Dice}1d2{/g} {g|Encyclopedia:Damage}damage{/g} per caster level to all creatures in " + Main.Settings.corruptedBloodRange + " feet radius and applies Corrupted Blood to them if they failed a fortitude save.");

                ResourcesFinder.corruptedBloodBuff.SetDescription("You are nauseated. Additionally, if you die, you deal {g|Encyclopedia:Dice}1d2{/g} {g|Encyclopedia:Damage}damage{/g} per caster level to all creatures in " + Main.Settings.corruptedBloodRange + " feet radius and apply Corrupted Blood to them if they failed a fortitude save.");
            }
        }

        static void patchEyesOfTheBodak()
        {
            //3. patch eyes of the bodak fix fx and floating numbers on negative level
            ResourcesFinder.eyeOfTheBodakAura.Fx = ResourcesFinder.deathGazeAura.Fx;

            var onEnterConditionCheckerTrueAction = ResourcesFinder.eyeOfTheBodakAura.GetComponent<AbilityAreaEffectRunAction>().UnitEnter.Actions.OfType<Conditional>().First<Conditional>();
            var originalDuration = onEnterConditionCheckerTrueAction.IfTrue.Actions.OfType<ContextActionApplyBuff>().First<ContextActionApplyBuff>().DurationValue;
            var actionDealDamage = Helpers.CreateActionDealEnergyDrainDamage(Helpers.CreateContextDiceValue(DiceType.One, 1, null), originalDuration, false);
            onEnterConditionCheckerTrueAction.IfTrue = Helpers.CreateActionList(new GameAction[]{
                actionDealDamage
            });

            var roundConditionCheckerTrueAction = ResourcesFinder.eyeOfTheBodakAura.GetComponent<AbilityAreaEffectRunAction>().Round.Actions.OfType<Conditional>().First<Conditional>();
            roundConditionCheckerTrueAction.IfTrue = Helpers.CreateActionList(new GameAction[]{
                actionDealDamage
            });

            Main.Log("Patched: Eye of the Bodak now works correctly and have FX");

            if (Main.Settings.removeDeathAndGazeDescriptorOnEyeOfTheBodak)
            {
                ResourcesFinder.eyeOfTheBodakAbility.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.SightBased;
                Main.Log("Patched: Eye of the Bodak now have no Gaze Attack / Death descriptor and works correctly when the Lich have such immunites.");
            }
        }

        static void patchEclipseChill()
        {
            //4. patch eclipse chill DC value now = 10 + mythic level + character level

            if (Main.Settings.allowEclipseChillDCBaseOnCharacterLevel)
            {
                var intRank = ResourcesFinder.eclipseChillBuff.GetComponents<ContextRankConfig>().FirstOrDefault(rank => rank.m_Stat == StatType.Intelligence);
                intRank.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                intRank.m_Progression = ContextRankProgression.AsIs;

                var newDesc = "For three {g|Encyclopedia:Combat_Round}rounds{/g} per day, Lich can imbue all {g|Encyclopedia:Spell}spells{/g} he casts with the powers of the Eclipse chill. " +
                    "Creatures affected by such spells must pass a {g|Encyclopedia:Saving_Throw}Fortitude saving throw{/g} ({g|Encyclopedia:DC}DC{/g} = 10 + {g|Encyclopedia:Character_Level}Character Level{/g} + Lich's mythic rank) or become blinded and suffer" +
                    " ({g|Encyclopedia:Dice}2d8{/g} + mythic rank) {g|Encyclopedia:Energy_Damage}cold damage{/g}, becoming vulnerable to cold and negative energy until the end of the combat.";

                ResourcesFinder.eclipseChillEffectBuff.SetDescription(newDesc);
                ResourcesFinder.eclipseChillFeature.SetDescription(newDesc);
                ResourcesFinder.eclipseChillActivatbleAbility.SetDescription(newDesc);

                Main.Log("Patched: Eclipse Chill now have DC = 10 + Mythic Rank + Character Level.");
            }
        }

        static void patchTaintedSneakAttack()
        {
            //5. patch TaintedSneakAttack DC value now = 10 + mythic level + character level

            if (Main.Settings.allowTaintedSneakAttackDCBaseOnCharacterLevel)
            {
                var intRank = ResourcesFinder.taintedSneakAttackFeature.GetComponents<ContextRankConfig>().FirstOrDefault(rank => rank.m_Stat == StatType.Intelligence);
                intRank.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                intRank.m_Progression = ContextRankProgression.AsIs;

                var newDesc = "Whenever Lich lands a successful sneak {g|Encyclopedia:Attack}attack{/g}, the enemy must pass {g|Encyclopedia:Saving_Throw}Fortitude saving throw{/g} ({g|Encyclopedia:DC}DC{/g} = 10 + {g|Encyclopedia:Character_Level}Character Level{/g} + Lich's mythic rank) or become tainted. " +
                    "The tainted creature is vulnerable to all weapon and elemental {g|Encyclopedia:Damage}damage{/g}, as well as suffers a ?2 {g|Encyclopedia:Penalty}penalty{/g} on all attack {g|Encyclopedia:Dice}rolls{/g} and weapon damage rolls, until the end of the combat." +
                    "\nAdditionally, Lich's sneak attack damage is increased by 1d6.";

                ResourcesFinder.taintedSneakAttackFeature.SetDescription(newDesc);
                ResourcesFinder.taintedSneakAttackBuff.SetDescription(newDesc);

                Main.Log("Patched: Tainted Sneak Attack now have DC = 10 + Mythic Rank + Character Level.");
            }
        }

        static void patchNegativeEruption()
        {
            //6. give negative eruption correct description + put a limit on it

            if (Main.Settings.setMaximumDamageOnNegativeEruption)
            {
                var dmgRank = ResourcesFinder.negativeEruptionAbility.GetComponents<ContextRankConfig>().FirstOrDefault(rank => rank.m_Progression == ContextRankProgression.MultiplyByModifier);
                dmgRank.m_Max = 250;
                dmgRank.m_UseMax = true;

                var newDesc = "This {g|Encyclopedia:Spell}spell{/g} acts as harm spell, but affects all creatures in 30 feet radius. Additionally, all affected undead creatures gain " +
                    "+2 {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Attack}attack rolls{/g}, {g|Encyclopedia:Damage}damage rolls{/g}, Will {g|Encyclopedia:Saving_Throw}saving throws{/g} and twice the {g|Encyclopedia:Caster_Level}caster level{/g} temporary {g|Encyclopedia:HP}hit points{/g}." +
                    "\nHarm: Harm charges a subject with negative energy that deals 10 points of damage per caster level (to a maximum of 250 points at 25th level). If the creature successfully saves, harm deals half this amount.\nIf used on an undead creature, harm acts like {g|Encyclopedia:Healing}heal{/g}.";

                ResourcesFinder.negativeEruptionAbility.SetDescription(newDesc);

                Main.Log("Patched: Negative Eruption now have a damage limit of 250.");
            }
        }


        static void patchInsightfulContemplation()
        {
            //n patch court poet singing fx
            ResourcesFinder.insightfulContemplationAura.Fx = ResourcesFinder.inspireGreatnessAura.Fx;

            Main.Log("Patched: Insightful Contemplation now have FX.");
        }
    }
}
