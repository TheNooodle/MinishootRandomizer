using UnityEngine;

namespace MinishootRandomizer
{
    abstract public class Location
    {
        private readonly string _identifier;
        private readonly string _logicRule;
        private readonly LocationPool _pool;

        /**
         *  This is the unique identifier of the location where an item can be placed, such as "Green - Boost Arena".
         */
        public string Identifier => _identifier;


        /**
         * This is the logic rule that determines what item can be placed in this location.
         */
        public string LogicRule => _logicRule;

        /**
         * This is the pool of locations that this location belongs to.
         */
        public LocationPool Pool => _pool;

        public Location(string identifier, string logicRule, LocationPool pool)
        {
            _identifier = identifier;
            _logicRule = logicRule;
            _pool = pool;
        }

        abstract public IPatchAction Accept(ILocationVisitor visitor, Item item);
    }
}
