using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DietFiltersInStorage
{
    [StaticConstructorOnStartup]
    public class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("llunak.DietFiltersInStorage");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [DefOf]
    public static class ThingCategoryDefOf
    {
        public static ThingCategoryDef Foods;

        static ThingCategoryDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingCategoryDefOf));
        }
    }

    [HarmonyPatch(typeof(ITab_Storage))]
    public static class ITab_Storage_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HiddenSpecialThingFilters))]
        private static void HiddenSpecialThingFilters(ref IEnumerable<SpecialThingFilterDef> __result)
        {
            if(__result != null)
            {
                List<SpecialThingFilterDef> l = new List<SpecialThingFilterDef>();
                foreach( SpecialThingFilterDef filter in __result )
                    if( filter.parentCategory != ThingCategoryDefOf.Foods )
                        l.Add( filter );
                __result = l;
            }
        }
    }
}
