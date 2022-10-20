using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Mutagen.Bethesda.FormKeys.SkyrimLE;

namespace ArmoryRebalance
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimLE, "YourPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            ArmorType.Create(Skyrim.Keyword.ArmorHelmet, 10, 15);
            ArmorType.Create(Skyrim.Keyword.ArmorCuirass, 20, 25);
            ArmorType.Create(Skyrim.Keyword.ArmorGauntlets, 5, 10);
            ArmorType.Create(Skyrim.Keyword.ArmorBoots, 5, 10);
            ArmorType.Create(Skyrim.Keyword.ArmorShield, 15, 20);

            WeaponType.Create(Skyrim.Keyword.WeapTypeSword, 7);
            WeaponType.Create(Skyrim.Keyword.WeapTypeWarAxe, 8);
            WeaponType.Create(Skyrim.Keyword.WeapTypeMace, 9);
            WeaponType.Create(Skyrim.Keyword.WeapTypeDagger, 4);

            WeaponType.Create(Skyrim.Keyword.WeapTypeGreatsword, 15);
            WeaponType.Create(Skyrim.Keyword.WeapTypeBattleaxe, 16);
            WeaponType.Create(Skyrim.Keyword.WeapTypeWarhammer, 18);

            WeaponType.Create(Skyrim.Keyword.WeapTypeBow, 9);

            foreach(var armorGetter in state.LoadOrder.PriorityOrder.Armor().WinningOverrides()) {
                if(armorGetter.Keywords == null || armorGetter.MajorFlags.HasFlag(Armor.MajorFlag.NonPlayable)) {
                    continue;
                }
                foreach(var keywordGetter in armorGetter.Keywords) {
                    var armorType = ArmorType.Get(keywordGetter.FormKey);
                    if(armorType == null) {
                        continue;
                    }
                    if(armorGetter.BodyTemplate == null) {
                        continue;
                    }
                    var armor = state.PatchMod.Armors.GetOrAddAsOverride(armorGetter);
                    var baseArmor = int.MinValue;
                            
                    if(armor.BodyTemplate!.ArmorType == Mutagen.Bethesda.Skyrim.ArmorType.LightArmor) {
                        baseArmor = armorType.baseLight;
                    } else if(armor.BodyTemplate!.ArmorType == Mutagen.Bethesda.Skyrim.ArmorType.HeavyArmor) {
                        baseArmor = armorType.baseHeavy;
                    }
                    if(baseArmor != int.MinValue) {
                        float armorRating = armor.ArmorRating;
                        if(armorRating >= baseArmor) {
                            armor.ArmorRating = (armorRating - baseArmor) * 2 + baseArmor;
                        }
                    }
                }
            }
            foreach(var weaponGetter in state.LoadOrder.PriorityOrder.Weapon().WinningOverrides()) {
                if(weaponGetter.Keywords == null || weaponGetter.MajorFlags.HasFlag(Weapon.MajorFlag.NonPlayable)) {
                    continue;
                }
                foreach(var keywordGetter in weaponGetter.Keywords) {
                    var weaponType = WeaponType.Get(keywordGetter.FormKey);
                    if(weaponType == null) {
                        continue;
                    }
                    var weapon = state.PatchMod.Weapons.GetOrAddAsOverride(weaponGetter);
                    var baseDamage = weaponType.baseDamage;
                    if(weapon.BasicStats != null) {
                        int damage = weapon.BasicStats.Damage;
                        if(damage >= baseDamage) {
                            weapon.BasicStats.Damage = (ushort)((damage - baseDamage) * 2 + baseDamage);
                        }
                    }
                }
            }
        }
    }
}
