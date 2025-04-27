using UnityEngine;

namespace MinishootRandomizer
{
    public class LinearShopLocation: Location
    {
        private readonly string _npcName;
        private readonly int _defaultPrice;
        private readonly Currency _defaultCurrency;

        public string NpcName => _npcName;
        public int DefaultPrice => _defaultPrice;
        public Currency DefaultCurrency => _defaultCurrency;

        public LinearShopLocation(
            string identifier,
            string logicRule,
            LocationPool pool,
            string npcName,
            int defaultPrice,
            Currency defaultCurrency
        ): base(identifier, logicRule, pool)
        {
            _npcName = npcName;
            _defaultPrice = defaultPrice;
            _defaultCurrency = defaultCurrency;
        }

        public override IPatchAction Accept(ILocationVisitor visitor, Item item)
        {
            return visitor.VisitLinearShop(this, item);
        }
    }

    public class LinearShopLocationReplacementData
    {
        private readonly string _npcName;
        private readonly int _defaultPrice;
        private readonly Currency _defaultCurrency;

        public string NpcName => _npcName;
        public int DefaultPrice => _defaultPrice;
        public Currency DefaultCurrency => _defaultCurrency;

        public LinearShopLocationReplacementData(string npcName, int defaultPrice, Currency defaultCurrency)
        {
            _npcName = npcName;
            _defaultPrice = defaultPrice;
            _defaultCurrency = defaultCurrency;
        }
    }
}
