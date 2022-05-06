using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using UnityEngine;
using Owlcat.Runtime.Visual;

namespace LichFix
{
	// Token: 0x02000027 RID: 39
	internal static class ExtentionMethods
	{

		// Token: 0x06000100 RID: 256 RVA: 0x00007B38 File Offset: 0x00005D38
		public static IEnumerable<BlueprintAbility> AbilityAndVariants(this BlueprintAbility ability)
		{
			List<BlueprintAbility> List = new List<BlueprintAbility>
			{
				ability
			};
			AbilityVariants varriants = ability.GetComponent<AbilityVariants>();
			if (varriants != null)
			{
				List.AddRange(varriants.Variants);
			}
			return List;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007B70 File Offset: 0x00005D70
		public static V PutIfAbsent<K, V>(this IDictionary<K, V> self, K key, V value) where V : class
		{
			V oldValue;
			if (!self.TryGetValue(key, out oldValue))
			{
				self.Add(key, value);
				return value;
			}
			return oldValue;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007B94 File Offset: 0x00005D94
		public static V PutIfAbsent<K, V>(this IDictionary<K, V> self, K key, Func<V> ifAbsent) where V : class
		{
			V value;
			if (!self.TryGetValue(key, out value))
			{
				self.Add(key, value = ifAbsent());
				return value;
			}
			return value;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public static T[] AppendToArray<T>(this T[] array, T value)
		{
			int len = array.Length;
			T[] result = new T[len + 1];
			Array.Copy(array, result, len);
			result[len] = value;
			return result;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007BEC File Offset: 0x00005DEC
		public static T[] RemoveFromArrayByType<T, V>(this T[] array)
		{
			List<T> list = new List<T>();
			foreach (T c in array)
			{
				if (!(c is V))
				{
					list.Add(c);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007C34 File Offset: 0x00005E34
		public static T[] AppendToArray<T>(this T[] array, params T[] values)
		{
			int len = array.Length;
			int valueLen = values.Length;
			T[] result = new T[len + valueLen];
			Array.Copy(array, result, len);
			Array.Copy(values, 0, result, len, valueLen);
			return result;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007C65 File Offset: 0x00005E65
		public static T[] AppendToArray<T>(this T[] array, IEnumerable<T> values)
		{
			return array.AppendToArray(values.ToArray<T>());
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007C74 File Offset: 0x00005E74
		public static T[] InsertBeforeElement<T>(this T[] array, T value, T element)
		{
			int len = array.Length;
			T[] result = new T[len + 1];
			int x = 0;
			bool added = false;
			for (int i = 0; i < len; i++)
			{
				if (array[i].Equals(element) && !added)
				{
					result[x++] = value;
					added = true;
				}
				result[x++] = array[i];
			}
			return result;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007CE4 File Offset: 0x00005EE4
		public static T[] InsertAfterElement<T>(this T[] array, T value, T element)
		{
			int len = array.Length;
			T[] result = new T[len + 1];
			int x = 0;
			bool added = false;
			for (int i = 0; i < len; i++)
			{
				if (array[i].Equals(element) && !added)
				{
					result[x++] = array[i];
					result[x++] = value;
					added = true;
				}
				else
				{
					result[x++] = array[i];
				}
			}
			return result;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007D6C File Offset: 0x00005F6C
		public static T[] RemoveFromArray<T>(this T[] array, T value)
		{
			List<T> list = array.ToList<T>();
			if (!list.Remove(value))
			{
				return array;
			}
			return list.ToArray();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007D91 File Offset: 0x00005F91
		public static string StringJoin<T>(this IEnumerable<T> array, Func<T, string> map, string separator = " ")
		{
			return string.Join(separator, array.Select(map));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007DF8 File Offset: 0x00005FF8
		public static void RemoveFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features)
		{
			for (int i = 0; i < features.Length; i++)
			{
				BlueprintFeature feature2 = features[i];
				BlueprintFeatureReference featureReference = feature2.ToReference<BlueprintFeatureReference>();
				if (selection.m_AllFeatures.Contains(featureReference))
				{
					selection.m_AllFeatures = (from f in selection.m_AllFeatures
											   where !f.Equals(featureReference)
											   select f).ToArray<BlueprintFeatureReference>();
				}
				if (selection.m_Features.Contains(featureReference))
				{
					selection.m_Features = (from f in selection.m_Features
											where !f.Equals(featureReference)
											select f).ToArray<BlueprintFeatureReference>();
				}
			}
			selection.m_AllFeatures = (from feature in selection.m_AllFeatures
									   orderby feature.Get().Name
									   select feature).ToArray<BlueprintFeatureReference>();
			selection.m_Features = (from feature in selection.m_Features
									orderby feature.Get().Name
									select feature).ToArray<BlueprintFeatureReference>();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007F08 File Offset: 0x00006108
		public static void AddFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features)
		{
			for (int i = 0; i < features.Length; i++)
			{
				BlueprintFeatureReference featureReference = features[i].ToReference<BlueprintFeatureReference>();
				if (!selection.m_AllFeatures.Contains(featureReference))
				{
					selection.m_AllFeatures = selection.m_AllFeatures.AppendToArray(featureReference);
				}
				if (!selection.m_Features.Contains(featureReference))
				{
					selection.m_Features = selection.m_Features.AppendToArray(featureReference);
				}
			}
			selection.m_AllFeatures = (from feature in selection.m_AllFeatures
									   orderby feature.Get().Name
									   select feature).ToArray<BlueprintFeatureReference>();
			selection.m_Features = (from feature in selection.m_Features
									orderby feature.Get().Name
									select feature).ToArray<BlueprintFeatureReference>();
		}


		// Token: 0x0600011A RID: 282 RVA: 0x00008318 File Offset: 0x00006518
		public static void InsertComponent(this BlueprintScriptableObject obj, int index, BlueprintComponent component)
		{
			List<BlueprintComponent> components = obj.ComponentsArray.ToList<BlueprintComponent>();
			components.Insert(index, component);
			obj.SetComponents(components);
		}


		// Token: 0x0600011C RID: 284 RVA: 0x0000834E File Offset: 0x0000654E


		// Token: 0x0600011E RID: 286 RVA: 0x00008380 File Offset: 0x00006580
		public static void RemoveComponent(this BlueprintScriptableObject obj, BlueprintComponent component)
		{
			obj.SetComponents(obj.ComponentsArray.RemoveFromArray(component));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008394 File Offset: 0x00006594
		public static void RemoveComponents<T>(this BlueprintScriptableObject obj) where T : BlueprintComponent
		{
			foreach (T c in obj.GetComponents<T>().ToArray<T>())
			{
				obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000083DC File Offset: 0x000065DC
		public static void RemoveComponents<T>(this BlueprintScriptableObject obj, Predicate<T> predicate) where T : BlueprintComponent
		{
			foreach (T c in obj.GetComponents<T>().ToArray<T>())
			{
				if (predicate(c))
				{
					obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000842B File Offset: 0x0000662B
		public static void AddComponents(this BlueprintScriptableObject obj, IEnumerable<BlueprintComponent> components)
		{
			obj.AddComponents(components.ToArray<BlueprintComponent>());
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000843C File Offset: 0x0000663C
		public static void AddComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components)
		{
			List<BlueprintComponent> c = obj.ComponentsArray.ToList<BlueprintComponent>();
			c.AddRange(components);
			obj.SetComponents(c.ToArray());
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008468 File Offset: 0x00006668
		public static void SetComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components)
		{
			HashSet<string> names = new HashSet<string>();
			foreach (BlueprintComponent c in components)
			{
				if (string.IsNullOrEmpty(c.name))
				{
					c.name = "$" + c.GetType().Name;
				}
				if (!names.Add(c.name))
				{
					int i = 0;
					string name;
					while (!names.Add(name = string.Format("{0}${1}", c.name, i)))
					{
						i++;
					}
					c.name = name;
				}
			}
			obj.ComponentsArray = components;
			obj.OnEnable();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008507 File Offset: 0x00006707
		public static void SetComponents(this BlueprintScriptableObject obj, IEnumerable<BlueprintComponent> components)
		{
			obj.SetComponents(components.ToArray<BlueprintComponent>());
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008518 File Offset: 0x00006718
		public static T CreateCopy<T>(this T original, Action<T> action = null) where T : UnityEngine.Object
		{
			T clone = UnityEngine.Object.Instantiate<T>(original);
			if (action != null)
			{
				action(clone);
			}
			return clone;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000855C File Offset: 0x0000675C
		public static void SetNameDescription(this BlueprintUnitFact feature, BlueprintUnitFact other)
		{
			feature.m_DisplayName = other.m_DisplayName;
			feature.m_Description = other.m_Description;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008576 File Offset: 0x00006776
		public static void SetName(this BlueprintUnitFact feature, LocalizedString name)
		{
			feature.m_DisplayName = name;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000085BB File Offset: 0x000067BB
		public static void SetDescription(this BlueprintUnitFact feature, LocalizedString description)
		{
			feature.m_Description = description;
		}

		public static void SetDescription(this BlueprintUnitFact feature, string description)
		{
			feature.SetDescriptionTagged(description);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0004463B File Offset: 0x0004283B
		public static void SetDescription(this BlueprintItem item, string description)
		{
			Helpers.SetLocalizedStringProperty(item, "m_Description", item.NameSafe() + "_Item_Desc_WC", description);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000086F4 File Offset: 0x000068F4
		public static bool HasAreaEffect(this BlueprintAbility spell)
		{
			return spell.AoERadius.Meters > 0f || spell.ProjectileType > AbilityProjectileType.Simple;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00008721 File Offset: 0x00006921
		internal static IEnumerable<BlueprintComponent> WithoutSpellComponents(this IEnumerable<BlueprintComponent> components)
		{
			return from c in components
				   where !(c is SpellComponent) && !(c is SpellListComponent)
				   select c;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00008748 File Offset: 0x00006948
		internal static int GetCost(this BlueprintAbility.MaterialComponentData material)
		{
			BlueprintItem item = (material != null) ? material.Item : null;
			if (item != null)
			{
				return item.Cost * material.Count;
			}
			return 0;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008774 File Offset: 0x00006974
		public static AddConditionImmunity CreateImmunity(this UnitCondition condition)
		{
			return new AddConditionImmunity
			{
				Condition = condition
			};
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008782 File Offset: 0x00006982
		public static AddCondition CreateAddCondition(this UnitCondition condition)
		{
			return new AddCondition
			{
				Condition = condition
			};
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00008790 File Offset: 0x00006990
		public static BuffDescriptorImmunity CreateBuffImmunity(this SpellDescriptor spell)
		{
			return new BuffDescriptorImmunity
			{
				Descriptor = spell
			};
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000087A3 File Offset: 0x000069A3
		public static SpellImmunityToSpellDescriptor CreateSpellImmunity(this SpellDescriptor spell)
		{
			return new SpellImmunityToSpellDescriptor
			{
				Descriptor = spell
			};
		}
	}
}
