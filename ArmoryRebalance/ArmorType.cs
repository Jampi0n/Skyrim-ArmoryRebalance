using System.Collections.Generic;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;

namespace ArmoryRebalance {
    class ArmorType {
        public static Dictionary<FormKey, ArmorType> map = new();
        public readonly int baseLight;
        public readonly int baseHeavy;
        private ArmorType(int baseLight, int baseHeavy) {
            this.baseLight = baseLight;
            this.baseHeavy = baseHeavy;
        }

        public static void Create(FormLink<IKeywordGetter> keyword, int baseLight, int baseHeavy) {
            map.Add(keyword.FormKey, new ArmorType(baseLight, baseHeavy));
        }

        public static ArmorType? Get(FormKey keyword) {
            if(map.ContainsKey(keyword)) {
                return map[keyword];
            } else {
                return null;
            }
        }
    }
}
