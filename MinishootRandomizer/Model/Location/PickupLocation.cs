namespace MinishootRandomizer
{
    public class PickupLocation: Location
    {
        private ISelector _selector;

        public ISelector Selector => _selector;

        public PickupLocation(
            string identifier,
            string logicRule,
            LocationPool pool,
            ISelector selector
        ): base(identifier, logicRule, pool)
        {
            _selector = selector;
        }

        public override IPatchAction Accept(ILocationVisitor visitor, Item item)
        {
            return visitor.VisitPickup(this, item);
        }
    }
}
