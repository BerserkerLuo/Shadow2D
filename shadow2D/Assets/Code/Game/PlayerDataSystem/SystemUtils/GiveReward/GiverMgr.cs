
using System.Collections.Generic;
using Tool;

namespace PlayerSystemData
{
    public interface BaseGiver
    {
        public void DoReward(int itemId, int itemCount);
        public bool CheckOnlyReward(int itemId) { return false; }
    }

    public class GiverMgr : SingletonBase<GiverMgr>
    {
        Dictionary<int, BaseGiver> m_dictGiverMap = new Dictionary<int, BaseGiver>();

        public GiverMgr() {
            //添加所有的奖励物品类型
            //m_dictGiverMap.Add((int)ItemType.Hero,new HeroGiver());
            //m_dictGiverMap.Add((int)ItemType.Equip,new EquipGiver());
            //m_dictGiverMap.Add((int)ItemType.Currency,new CurrencyGiver());
            //m_dictGiverMap.Add((int)ItemType.UserData,new UserDataGiver());
        }

        public BaseGiver GetRewardGiver(int itemType) {
            if (m_dictGiverMap.ContainsKey(itemType) == false)
                return null;
            return m_dictGiverMap[itemType];
        }
    }
}
