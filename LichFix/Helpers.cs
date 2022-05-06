using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;

namespace LichFix
{
	// Token: 0x0200000E RID: 14
	public static class Helpers
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000044F4 File Offset: 0x000026F4
		public static T Create<T>(Action<T> init = null) where T : new()
		{
			T result = Activator.CreateInstance<T>();
			if (init != null)
			{
				init(result);
			}
			return result;
		}

		public static ContextActionOnRandomTargetsAroundSelective CreateContextActionOnRandomTargetsAroundSelective(ContextActionOnRandomTargetsAround originalAction)
		{
			ContextActionOnRandomTargetsAroundSelective actionWithSelective = new ContextActionOnRandomTargetsAroundSelective();
			actionWithSelective.m_FilterNoFact = originalAction.m_FilterNoFact;
			actionWithSelective.NumberOfTargets = originalAction.NumberOfTargets;
			actionWithSelective.Radius = originalAction.Radius;
			actionWithSelective.Actions = originalAction.Actions;

			return actionWithSelective;
		}

		// LichFix.Helpers
		// Token: 0x06000037 RID: 55 RVA: 0x0000374C File Offset: 0x0000194C
		public static ContextActionDealDamage CreateActionDealEnergyDrainDamage(ContextDiceValue damage, ContextDurationValue duration, bool isAoE = false)
		{
			ContextActionDealDamage c = Helpers.Create<ContextActionDealDamage>(null);
            c.m_Type = ContextActionDealDamage.Type.EnergyDrain;
			c.DamageType = new DamageTypeDescription
			{
				Type = 0,
				Common = new DamageTypeDescription.CommomData(),
				Physical = new DamageTypeDescription.PhysicalData
				{
					Form = PhysicalDamageForm.Slashing
				}
			};
			c.Duration = duration;
			c.Value = damage;
			c.IsAoE = isAoE;
			c.HalfIfSaved = false;
			c.IgnoreCritical = true;
			return c;
		}

		// LichFix.Helpers
		// Token: 0x06000033 RID: 51 RVA: 0x00003644 File Offset: 0x00001844
		public static ContextDiceValue CreateContextDiceValue(DiceType dice, ContextValue diceCount = null, ContextValue bonus = null)
		{
			return new ContextDiceValue
			{
				DiceType = dice,
				DiceCountValue = (diceCount ?? Helpers.CreateContextValueRank(0)),
				BonusValue = (bonus ?? 0)
			};
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000045C9 File Offset: 0x000027C9
		public static LevelEntry LevelEntry(int level, BlueprintFeatureBase feature)
		{
			return new LevelEntry
			{
				Level = level,
				Features =
				{
					feature
				}
			};
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000045E4 File Offset: 0x000027E4
		public static LevelEntry CreateLevelEntry(int level, params BlueprintFeatureBase[] features)
		{
			LevelEntry levelEntry = new LevelEntry();
			levelEntry.Level = level;
			features.ForEach(delegate (BlueprintFeatureBase f)
			{
				levelEntry.Features.Add(f);
			});
			return levelEntry;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004628 File Offset: 0x00002828
		public static UIGroup CreateUIGroup(params BlueprintFeatureBase[] features)
		{
			UIGroup uiGroup = new UIGroup();
			features.ForEach(delegate (BlueprintFeatureBase f)
			{
				uiGroup.Features.Add(f);
			});
			return uiGroup;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000465E File Offset: 0x0000285E
		public static ContextValue CreateContextValueRank(AbilityRankType value = AbilityRankType.Default)
		{
			return value.CreateContextValue();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004666 File Offset: 0x00002866
		public static ContextValue CreateContextValue(this AbilityRankType value)
		{
			return new ContextValue
			{
				ValueType = ContextValueType.Rank,
				ValueRank = value
			};
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000467B File Offset: 0x0000287B
		public static ContextValue CreateContextValue(this AbilitySharedValue value)
		{
			return new ContextValue
			{
				ValueType = ContextValueType.Shared,
				ValueShared = value
			};
		}

		public static AddFacts CreateAddFact(this BlueprintUnitFact fact)
		{
			AddFacts result = Helpers.Create<AddFacts>(null);
			result.name = "AddFacts$" + fact.name;
			AccessTools.Field(typeof(AddFacts), "m_Facts").SetValue(result, new BlueprintUnitFactReference[]
			{
		fact.ToReference<BlueprintUnitFactReference>()
			});
			return result;
		}

		public static ContextActionSavingThrow CreateContextActionSavingThrow(SavingThrowType saving_throw, ActionList action)
		{
			ContextActionSavingThrow c = Helpers.Create<ContextActionSavingThrow>(null);
			c.Type = saving_throw;
			c.Actions = action;
			return c;
		}

		public static ContextActionSavingThrow CreateContextActionSavingThrow(SavingThrowType saving_throw, ActionList action, int customDC)
		{
			ContextActionSavingThrow c = Helpers.Create<ContextActionSavingThrow>(null);
			c.Type = saving_throw;
			c.Actions = action;
			c.HasCustomDC = true;
			c.CustomDC = new ContextValue
			{
				Value = customDC
			};
			return c;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00043EA8 File Offset: 0x000420A8
		public static ContextActionConditionalSaved CreateContextSavedApplyBuff(BlueprintBuff buff, ContextDurationValue duration, bool is_from_spell = false, bool is_child = false, bool is_permanent = false, bool is_dispellable = true)
		{
			ContextActionConditionalSaved context_saved = Helpers.Create<ContextActionConditionalSaved>(null);
			context_saved.Succeed = new ActionList();
			ContextActionApplyBuff apply_buff = Helpers.Create<ContextActionApplyBuff>(null);
			apply_buff.IsFromSpell = true;
			apply_buff.m_Buff = buff.ToReference<BlueprintBuffReference>();
			apply_buff.DurationValue = duration;
			apply_buff.IsFromSpell = is_from_spell;
			apply_buff.AsChild = is_child;
			apply_buff.Permanent = is_permanent;
			apply_buff.IsNotDispelable = !is_dispellable;
			context_saved.Failed = Helpers.CreateActionList(new GameAction[]
			{
				apply_buff
			});
			return context_saved;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0004407C File Offset: 0x0004227C
		public static ConditionsChecker CreateConditionsCheckerAnd(params Condition[] conditions)
		{
			return new ConditionsChecker
			{
				Conditions = conditions,
				Operation = Operation.And
			};
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000440A4 File Offset: 0x000422A4
		public static ConditionsChecker CreateConditionsCheckerOr(params Condition[] conditions)
		{
			return new ConditionsChecker
			{
				Conditions = conditions,
				Operation = Operation.Or
			};
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00043944 File Offset: 0x00041B44
		public static Conditional CreateConditional(Condition condition, GameAction ifTrue, GameAction ifFalse = null)
		{
			Conditional c = Helpers.Create<Conditional>(null);
			c.ConditionsChecker = Helpers.CreateConditionsCheckerAnd(new Condition[]
			{
				condition
			});
			c.IfTrue = Helpers.CreateActionList(new GameAction[]
			{
				ifTrue
			});
			c.IfFalse = Helpers.CreateActionList(new GameAction[]
			{
				ifFalse
			});
			return c;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000439A0 File Offset: 0x00041BA0
		public static Conditional CreateConditional(Condition[] condition, GameAction ifTrue, GameAction ifFalse = null)
		{
			Conditional c = Helpers.Create<Conditional>(null);
			c.ConditionsChecker = Helpers.CreateConditionsCheckerAnd(condition);
			c.IfTrue = Helpers.CreateActionList(new GameAction[]
			{
				ifTrue
			});
			c.IfFalse = Helpers.CreateActionList(new GameAction[]
			{
				ifFalse
			});
			return c;
		}


		// Token: 0x06000054 RID: 84 RVA: 0x00004690 File Offset: 0x00002890
		public static ActionList CreateActionList(params GameAction[] actions)
		{
			if (actions == null || (actions.Length == 1 && actions[0] == null))
			{
				actions = Array.Empty<GameAction>();
			}
			return new ActionList
			{
				Actions = actions
			};
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004764 File Offset: 0x00002964
		public static void SetField(object obj, string name, object value)
		{
			AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004779 File Offset: 0x00002979
		public static object GetField(object obj, string name)
		{
			return AccessTools.Field(obj.GetType(), name).GetValue(obj);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000478D File Offset: 0x0000298D
		private static ulong ParseGuidLow(string id)
		{
			return ulong.Parse(id.Substring(id.Length - 16), NumberStyles.HexNumber);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000047A8 File Offset: 0x000029A8
		private static ulong ParseGuidHigh(string id)
		{
			return ulong.Parse(id.Substring(0, id.Length - 16), NumberStyles.HexNumber);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000047C4 File Offset: 0x000029C4
		public static string MergeIds(string guid1, string guid2, string guid3 = null)
		{
			ulong low = Helpers.ParseGuidLow(guid1);
			ulong high = Helpers.ParseGuidHigh(guid1);
			low ^= Helpers.ParseGuidLow(guid2);
			high ^= Helpers.ParseGuidHigh(guid2);
			if (guid3 != null)
			{
				low ^= Helpers.ParseGuidLow(guid3);
				high ^= Helpers.ParseGuidHigh(guid3);
			}
			return high.ToString("x16") + low.ToString("x16");
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004824 File Offset: 0x00002A24
		public static ContextRankConfig CreateContextRankConfig(ContextRankBaseValueType baseValueType = ContextRankBaseValueType.CasterLevel, ContextRankProgression progression = ContextRankProgression.AsIs, AbilityRankType type = AbilityRankType.Default, int? min = null, int? max = null, int startLevel = 0, int stepLevel = 0, bool exceptClasses = false, StatType stat = StatType.Unknown, BlueprintUnitProperty customProperty = null, BlueprintCharacterClass[] classes = null, BlueprintArchetype[] archetypes = null, BlueprintArchetype archetype = null, BlueprintFeature feature = null, BlueprintFeature[] featureList = null, ValueTuple<int, int>[] customProgression = null)
		{
			ContextRankConfig contextRankConfig = new ContextRankConfig();
			contextRankConfig.m_Type = type;
			contextRankConfig.m_BaseValueType = baseValueType;
			contextRankConfig.m_Progression = progression;
			contextRankConfig.m_UseMin = (min != null);
			contextRankConfig.m_Min = min.GetValueOrDefault();
			contextRankConfig.m_UseMax = (max != null);
			contextRankConfig.m_Max = max.GetValueOrDefault();
			contextRankConfig.m_StartLevel = startLevel;
			contextRankConfig.m_StepLevel = stepLevel;
			contextRankConfig.m_Feature = feature.ToReference<BlueprintFeatureReference>();
			contextRankConfig.m_ExceptClasses = exceptClasses;
			contextRankConfig.m_CustomProperty = customProperty.ToReference<BlueprintUnitPropertyReference>();
			contextRankConfig.m_Stat = stat;
			BlueprintCharacterClassReference[] @class;
			if (classes != null)
			{
				@class = (from c in classes
						  select c.ToReference<BlueprintCharacterClassReference>()).ToArray<BlueprintCharacterClassReference>();
			}
			else
			{
				@class = Array.Empty<BlueprintCharacterClassReference>();
			}
			contextRankConfig.m_Class = @class;
			contextRankConfig.Archetype = archetype.ToReference<BlueprintArchetypeReference>();
			BlueprintArchetypeReference[] additionalArchetypes;
			if (archetypes != null)
			{
				additionalArchetypes = (from c in archetypes
										select c.ToReference<BlueprintArchetypeReference>()).ToArray<BlueprintArchetypeReference>();
			}
			else
			{
				additionalArchetypes = Array.Empty<BlueprintArchetypeReference>();
			}
			contextRankConfig.m_AdditionalArchetypes = additionalArchetypes;
			BlueprintFeatureReference[] featureList2;
			if (featureList != null)
			{
				featureList2 = (from c in featureList
								select c.ToReference<BlueprintFeatureReference>()).ToArray<BlueprintFeatureReference>();
			}
			else
			{
				featureList2 = Array.Empty<BlueprintFeatureReference>();
			}
			contextRankConfig.m_FeatureList = featureList2;
			return contextRankConfig;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000497C File Offset: 0x00002B7C
		public static ContextRankConfig CreateContextRankConfig(Action<ContextRankConfig> init)
		{
			ContextRankConfig config = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CasterLevel, ContextRankProgression.AsIs, AbilityRankType.Default, null, null, 0, 0, false, StatType.Unknown, null, null, null, null, null, null, null);
			if (init != null)
			{
				init(config);
			}
			return config;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000433D4 File Offset: 0x000415D4
		public static LocalizedString CreateString(string key, string value)
		{
			Dictionary<string, LocalizationPack.StringEntry> strings = new Dictionary<string, LocalizationPack.StringEntry>();
			bool flag = LocalizationManager.CurrentPack != null;
			if (flag)
			{
				strings = LocalizationManager.CurrentPack.m_Strings;
			}
			string oldValue;
			LocalizationPack.StringEntry stringStruct = new LocalizationPack.StringEntry();
			bool flag2 = strings.TryGetValue(key, out stringStruct);
			oldValue = stringStruct.Text;
			if (flag2 && value != oldValue)
			{
				Main.Log("Info: duplicate localized string `" + key + "`, different text.");
			}
			stringStruct.Text = value;
			strings[key] = stringStruct;
			LocalizedString localized = new LocalizedString();
			localized.m_Key = key;
			localized.m_ShouldProcess = false;
			Helpers.textToLocalizedString[value] = localized;
			return localized;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0004336A File Offset: 0x0004156A
		public static void SetLocalizedStringProperty(BlueprintScriptableObject obj, string propertyName, string name, string value)
		{
			AccessTools.Field(obj.GetType(), propertyName).SetValue(obj, Helpers.CreateString(obj.name + "." + name, value));
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0004465C File Offset: 0x0004285C
		public static void SetDescriptionTagged(this BlueprintUnitFact feature, string description)
		{
			string taggedDescription = DescriptionTools.TagEncyclopediaEntries(description);
			Helpers.SetLocalizedStringProperty(feature, "m_Description", feature.NameSafe() + "_Item_Desc_WC", taggedDescription);
		}

		// Token: 0x04000006 RID: 6
		private static Dictionary<string, LocalizedString> textToLocalizedString = new Dictionary<string, LocalizedString>();
	}

	// Token: 0x02000324 RID: 804
	public class ArrayTraverse
	{
		// Token: 0x06000EA9 RID: 3753 RVA: 0x0003F09C File Offset: 0x0003D29C
		public ArrayTraverse(Array array)
		{
			this.maxLengths = new int[array.Rank];
			for (int i = 0; i < array.Rank; i++)
			{
				this.maxLengths[i] = array.GetLength(i) - 1;
			}
			this.Position = new int[array.Rank];
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0003F0F4 File Offset: 0x0003D2F4
		public bool Step()
		{
			for (int i = 0; i < this.Position.Length; i++)
			{
				if (this.Position[i] < this.maxLengths[i])
				{
					this.Position[i]++;
					for (int j = 0; j < i; j++)
					{
						this.Position[j] = 0;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x040008A3 RID: 2211
		public int[] Position;

		// Token: 0x040008A4 RID: 2212
		private int[] maxLengths;
	}

	// Token: 0x02000325 RID: 805
	public class ReferenceEqualityComparer : EqualityComparer<object>
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x0003F14F File Offset: 0x0003D34F
		public override bool Equals(object x, object y)
		{
			return x == y;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0003F158 File Offset: 0x0003D358
		public override int GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			WeakResourceLink wrl = obj as WeakResourceLink;
			if (wrl == null)
			{
				return obj.GetHashCode();
			}
			if (wrl.AssetId == null)
			{
				return "WeakResourceLink".GetHashCode();
			}
			return wrl.GetHashCode();
		}
	}
}
