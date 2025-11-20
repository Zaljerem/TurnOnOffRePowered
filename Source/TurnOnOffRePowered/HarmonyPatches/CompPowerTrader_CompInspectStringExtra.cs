using System.Linq;
using System.Text.RegularExpressions;
using HarmonyLib;
using RimWorld;
using Verse;

namespace TurnOnOffRePowered.HarmonyPatches;

[HarmonyPatch(typeof(CompPowerTrader), nameof(CompPowerTrader.CompInspectStringExtra))]
public static class CompPowerTrader_CompInspectStringExtra
{
    public static void Postfix(CompPowerTrader __instance, ref string __result)
    {
        var parent = __instance.parent;
        if (!TurnItOnUtility.buildingsToModifyPowerOn.Contains(parent)
            || !TurnItOnUtility.powerLevels.ContainsKey(parent.def.defName))
        {
            return;
        }

        var newString = TurnItOnUtility.buildingsThatWereUsedLastTick.Contains(parent)
            ? $"{"PowerNeeded".Translate()}: {TurnItOnUtility.powerLevels[parent.def.defName][1] * -1} {"unitOfPower".Translate()} ({TurnItOnUtility.powerLevels[parent.def.defName][0] * -1} {"unitOfPower".Translate()} {"powerNeededInactive".Translate()})\n"
            : $"{"PowerNeeded".Translate()}: {TurnItOnUtility.powerLevels[parent.def.defName][0] * -1} {"unitOfPower".Translate()} ({TurnItOnUtility.powerLevels[parent.def.defName][1] * -1} {"unitOfPower".Translate()} {"powerNeededActive".Translate()})\n";
        var pattern = $"{"PowerNeeded".Translate()}.*\\n";
        __result = Regex.Replace(__result, pattern, newString);
    }
}