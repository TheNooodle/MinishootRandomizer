using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinishootRandomizer
{
    public class GameObjectCreationVisitor : ILocationVisitor
    {
        private List<ShopReplacementData> _shopReplacementData = new List<ShopReplacementData>
        {
            new ShopReplacementData("Blacksmith", new List<string> { 
                "Town - Blacksmith Item 1",
                "Town - Blacksmith Item 2",
                "Town - Blacksmith Item 3",
                "Town - Blacksmith Item 4",
            }),
            new ShopReplacementData("ScarabCollector", new List<string> { 
                "Town - Scarab Collector Item 1",
                "Town - Scarab Collector Item 2",
                "Town - Scarab Collector Item 3",
                "Town - Scarab Collector Item 4",
                "Town - Scarab Collector Item 5",
                "Town - Scarab Collector Item 6",
            }),
            new ShopReplacementData("MercantHub", new List<string> {
                "Town - Mercant Item 1",
                "Town - Mercant Item 2",
                "Town - Mercant Item 3",
            }),
            new ShopReplacementData("MercantBusher", new List<string> {
                "Forest Shop 1",
                "Forest Shop 2",
                "Forest Shop 3",
            }),
            new ShopReplacementData("MercantJar", new List<string> {
                "Jar Shop 1",
                "Jar Shop 2",
                "Jar Shop 3",
            }),
            new ShopReplacementData("MercantFrogger", new List<string> {
                "Swamp Shop 1",
                "Swamp Shop 2",
                "Swamp Shop 3",
            }),
        };

        private readonly IGameObjectFactory _factory;
        private readonly IObjectFinder _objectFinder;
        private readonly PickupManager _pickupManager;
        private readonly ILogger _logger;

        public GameObjectCreationVisitor(IGameObjectFactory factory, IObjectFinder objectFinder, PickupManager pickupManager, ILogger logger = null)
        {
            _factory = factory;
            _objectFinder = objectFinder;
            _pickupManager = pickupManager;
            _logger = logger ?? new NullLogger();
        }

        public IPatchAction VisitPickup(PickupLocation location, Item item)
        {
            _logger.LogDebug($"GameObjectCreationVisitor is visiting PickupLocation {location.Identifier}");
            GameObject replacementGameObject = _factory.CreateGameObjectForItem(item);
            GameObject originalGameObject;
            try {
                originalGameObject = _objectFinder.FindObject(location.Selector);
            } catch (ObjectNotFoundException) {
                throw new GameObjectCreationException($"Could not find {location.Selector} for location {location.Identifier}");
            }

            RandomizerPickup randomizerPickup = replacementGameObject.GetComponent<RandomizerPickup>();
            if (randomizerPickup == null) {
                throw new GameObjectCreationException($"GameObject does not have RandomizerPickup component for location {location.Identifier}");
            }
            
            
            replacementGameObject.name = "Randomized " + item.Identifier + " at " + location.Identifier;
            replacementGameObject.transform.position = originalGameObject.transform.position;
            randomizerPickup.SetLocation(location);
            
            ReplacePickupAction replaceAction = new ReplacePickupAction(_pickupManager, originalGameObject, randomizerPickup);
            _logger.LogDebug($"GameObjectCreationVisitor has replaced {location.Selector} with {item.Identifier}");

            return new LoggableAction(replaceAction, _logger);
        }

        public IPatchAction VisitShard(ShardLocation location, Item item)
        {
            _logger.LogDebug($"GameObjectCreationVisitor is visiting ShardLocation {location.Identifier}");
            GameObject gameObject = _factory.CreateGameObjectForItem(item);
            
            RandomizerPickup randomizerPickup = gameObject.GetComponent<RandomizerPickup>();
            if (randomizerPickup == null) {
                throw new GameObjectCreationException($"GameObject does not have RandomizerPickup component for location {location.Identifier}");
            }

            gameObject.transform.position = location.Position;
            gameObject.name = "Randomized " + item.Identifier + " at " + location.Identifier;

            randomizerPickup.SetLocation(location);

            _logger.LogDebug($"GameObjectCreationVisitor has created {item.Identifier} at {location.Position}");
            CreatePickupAction createAction = new CreatePickupAction(_pickupManager, randomizerPickup);

            return new LoggableAction(createAction, _logger);
        }

        public IPatchAction VisitCrystalNpc(CrystalNpcLocation location, Item item)
        {
            _logger.LogDebug($"GameObjectCreationVisitor is visiting PickupLocation {location.Identifier}");
            GameObject replacementGameObject = _factory.CreateGameObjectForItem(item);
            GameObject originalGameObject;
            try {
                originalGameObject = _objectFinder.FindObject(location.Selector);
            } catch (ObjectNotFoundException) {
                throw new GameObjectCreationException($"Could not find GameObject {location.Selector} for location {location.Identifier}");
            }

            RandomizerPickup randomizerPickup = replacementGameObject.GetComponent<RandomizerPickup>();
            if (randomizerPickup == null) {
                throw new GameObjectCreationException($"GameObject does not have RandomizerPickup component for location {location.Identifier}");
            }

            replacementGameObject.transform.position = originalGameObject.transform.position;
            replacementGameObject.name = "Randomized " + item.Identifier + " at " + location.Identifier;
            
            randomizerPickup.SetLocation(location);
            _logger.LogDebug($"GameObjectCreationVisitor has replaced {location.Selector} with {item.Identifier}");
            ReplacePickupAction replaceAction = new ReplacePickupAction(_pickupManager, originalGameObject, randomizerPickup);

            return new LoggableAction(replaceAction, _logger);
        }

        public IPatchAction VisitLinearShop(LinearShopLocation location, Item item)
        {
            _logger.LogDebug($"GameObjectCreationVisitor is visiting LinearShopLocation {location.Identifier}");

            ShopReplacementData shopReplacement = _shopReplacementData.Find(data => data.LocationNames.Contains(location.Identifier));
            if (shopReplacement == null)
            {
                throw new GameObjectCreationException($"No shop replacement data found for {location.Identifier}");
            }

            RandomizerNpcTradingInteraction interaction = UnityEngine.Object
                .FindObjectsOfType<RandomizerNpcTradingInteraction>(true)
                .FirstOrDefault(currentInteraction => currentInteraction.gameObject.name == shopReplacement.NpcName);
            if (interaction == null)
            {
                throw new GameObjectCreationException($"No RandomizerNpcTradingInteraction found for {shopReplacement.NpcName}");
            }

            RandomizedShopSlot shopSlot = new RandomizedShopSlot(location, item, location.DefaultPrice, location.DefaultCurrency);
            interaction.AddShopSlot(shopSlot);

            return new NullAction();
        }

        public IPatchAction VisitParallelShop(ChoiceShopLocation location, Item item)
        {
            _logger.LogDebug($"GameObjectCreationVisitor is visiting ChoiceShopLocation {location.Identifier}");

            GameObject originalGameObject;
            try {
                originalGameObject = _objectFinder.FindObject(location.Selector);
            } catch (ObjectNotFoundException) {
                throw new GameObjectCreationException($"Could not find GameObject {location.Selector} for location {location.Identifier}");
            }

            // First, we need to find the RandomizerNpcTradingInteraction that corresponds to this location
            ShopReplacementData shopReplacement = _shopReplacementData.Find(data => data.LocationNames.Contains(location.Identifier));
            if (shopReplacement == null)
            {
                throw new GameObjectCreationException($"No shop replacement data found for {location.Identifier}");
            }

            GameObject npcObject;
            try
            {
                npcObject = _objectFinder.FindObject(new ByName(shopReplacement.NpcName, typeof(RandomizerNpcTradingInteraction)));
            }
            catch (ObjectNotFoundException)
            {
                throw new GameObjectCreationException($"No RandomizerNpcTradingInteraction found for {shopReplacement.NpcName}");
            }
            RandomizerNpcTradingInteraction interaction = npcObject.GetComponent<RandomizerNpcTradingInteraction>();

            // Next, we need to create a RandomizedShopSlot for this location and item
            RandomizedShopSlot shopSlot = new RandomizedShopSlot(location, item, location.DefaultPrice, location.DefaultCurrency);

            // Then, we need to create the item pickup GameObject and assign the RandomizedShopSlot to it
            GameObject replacementGameObject = _factory.CreateGameObjectForItem(item);
            RandomizerPickup randomizerPickup = replacementGameObject.GetComponent<RandomizerPickup>();
            if (randomizerPickup == null) {
                throw new GameObjectCreationException($"GameObject does not have RandomizerPickup component for location {location.Identifier}");
            }
            
            replacementGameObject.transform.position = originalGameObject.transform.position;
            replacementGameObject.name = "Randomized " + item.Identifier + " at " + location.Identifier;

            randomizerPickup.SetLocation(location);
            randomizerPickup.ShopSlot = shopSlot;
            randomizerPickup.Owner = interaction.GetComponent<Npc>();
            _logger.LogDebug($"GameObjectCreationVisitor has replaced {location.Selector} with {item.Identifier}");

            ReplacePickupAction replaceAction = new ReplacePickupAction(_pickupManager, originalGameObject, randomizerPickup);

            return new LoggableAction(replaceAction, _logger);
        }

        public IPatchAction VisitDestroyable(DestroyableLocation location, Item item)
        {
            _logger.LogDebug($"GameObjectCreationVisitor is visiting DestroyableLocation {location.Identifier}");
            GameObject pickupGameObject = _factory.CreateGameObjectForItem(item);

            GameObject destroyableObject = _objectFinder.FindObject(location.DestroyableSelector);
            if (destroyableObject == null) {
                throw new GameObjectCreationException($"Could not find GameObject {location.DestroyableSelector} for location {location.Identifier}");
            }

            AddComponentAction<EventDestroyableComponent> addComponentAction = new AddComponentAction<EventDestroyableComponent>(destroyableObject);

            RandomizerPickup randomizerPickup = pickupGameObject.GetComponent<RandomizerPickup>();
            if (randomizerPickup == null) {
                throw new GameObjectCreationException($"GameObject does not have RandomizerPickup component for location {location.Identifier}");
            }

            pickupGameObject.transform.position = destroyableObject.transform.position;
            pickupGameObject.transform.position += location.Offset;
            pickupGameObject.name = "Randomized " + item.Identifier + " at " + location.Identifier;
            
            randomizerPickup.SetLocation(location);
            addComponentAction.OnComponentAdded += randomizerPickup.AssignDestroyable;
            _logger.LogDebug($"GameObjectCreationVisitor has replaced {location.DestroyableSelector} with {item.Identifier}");

            CreatePickupAction createAction = new CreatePickupAction(_pickupManager, randomizerPickup);

            IPatchAction removeOriginalItemAction = new NullAction();
            if (location.ItemSelector != null) {
                GameObject itemGameObject = _objectFinder.FindObject(location.ItemSelector);
                if (itemGameObject == null) {
                    throw new GameObjectCreationException($"Could not find GameObject {location.ItemSelector} for location {location.Identifier}");
                }
                pickupGameObject.transform.position = itemGameObject.transform.position;
                removeOriginalItemAction = new RemoveGameObjectAction(itemGameObject);
            }

            CompositeAction compositeAction = new CompositeAction("Patch Destroyable Location at " + location.Identifier);
            compositeAction.Add(addComponentAction);
            compositeAction.Add(createAction);
            compositeAction.Add(removeOriginalItemAction);

            return new LoggableAction(compositeAction, _logger);
        }

        public IPatchAction VisitDungeonReward(DungeonRewardLocation location, Item item)
        {
            // do nothing
            return new NullAction();
        }
    }

    public class GameObjectCreationException : System.Exception
    {
        public GameObjectCreationException(string message) : base(message) { }
    }
}
