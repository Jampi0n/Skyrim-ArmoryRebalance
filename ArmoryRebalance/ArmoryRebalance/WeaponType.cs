using System.Collections.Generic;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;

namespace ArmoryRebalance {
    class WeaponType {
        public static Dictionary<FormKey, WeaponType> map = new();
        public readonly int baseDamage;
        private WeaponType(int baseDamage) {
            this.baseDamage = baseDamage;
        }
        public static void Create(FormLink<IKeywordGetter> keyword, int baseDamage) {
            map.Add(keyword.FormKey, new WeaponType(baseDamage));
        }

        public static WeaponType? Get(FormKey keyword) {
            if(map.ContainsKey(keyword)) {
                return map[keyword];
            } else {
                return null;
            }
        }
    }
}
