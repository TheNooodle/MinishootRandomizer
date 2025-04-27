using UnityEngine;

namespace MinishootRandomizer
{
    public class ChoiceShopLocation: Location
    {
        private readonly string _npcName;
        private readonly int _defaultPrice;
        private readonly Currency _defaultCurrency;
        private readonly ISelector _selector;

        public string NpcName => _npcName;
        public int DefaultPrice => _defaultPrice;
        public Currency DefaultCurrency => _defaultCurrency;
        public ISelector Selector => _selector;

        public ChoiceShopLocation(
            string identifier,
            string logicRule,
            LocationPool pool,
            string npcName,
            int defaultPrice,
            Currency defaultCurrency,
            ISelector selector
        ): base(identifier, logicRule, pool)
        {
            _npcName = npcName;
            _defaultPrice = defaultPrice;
            _defaultCurrency = defaultCurrency;
            _selector = selector;
        }

        public override IPatchAction Accept(ILocationVisitor visitor, Item item)
        {
            return visitor.VisitParallelShop(this, item);
        }
    }

    public class ChoiceShopLocationReplacementData
    {
        private readonly string _npcName;
        private readonly int _defaultPrice;
        private readonly Currency _defaultCurrency;
        private string _replacedGameObjectName;

        public string NpcName => _npcName;
        public int DefaultPrice => _defaultPrice;
        public Currency DefaultCurrency => _defaultCurrency;
        public string ReplacedGameObjectName => _replacedGameObjectName;

        public ChoiceShopLocationReplacementData(string npcName, int defaultPrice, Currency defaultCurrency, string replacedGameObjectName)
        {
            _npcName = npcName;
            _defaultPrice = defaultPrice;
            _defaultCurrency = defaultCurrency;
            _replacedGameObjectName = replacedGameObjectName;
        }
    }
}
