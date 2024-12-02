from components.ai import HostileEnemy
from components import consumable, equippable
from components.equipment import Equipment
from components.fighter import Fighter
from components.inventory import Inventory
from components.level import Level
from entity import Actor, Item

from constants import ENTITIES

player = Actor(
    img=ENTITIES.PLAYER,
    name="Player",
    ai_cls=HostileEnemy,
    equipment=Equipment(),
    fighter=Fighter(hp=30, mana=10, base_defense=1, base_power=2),
    inventory=Inventory(capacity=26),
    level=Level(level_up_base=200),
)

orc = Actor(
    img=ENTITIES.ORC,
    name="random",
    ai_cls=HostileEnemy,
    equipment=Equipment(),
    fighter=Fighter(hp=10, mana=0, base_defense=0, base_power=0),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=35),
)
troll = Actor(
    img=ENTITIES.TROLL,
    name="Troll",
    ai_cls=HostileEnemy,
    equipment=Equipment(),
    fighter=Fighter(hp=16, mana=0, base_defense=1, base_power=0),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=100),
)

confusion_scroll = Item(
    img=ENTITIES.SACK,
    name="Confusion Scroll",
    consumable=consumable.ConfusionConsumable(number_of_turns=10),
)
fireball_scroll = Item(
    img=ENTITIES.SACK,
    name="Fireball Scroll",
    consumable=consumable.FireballDamageConsumable(damage=12, radius=3),
)
health_potion = Item(
    img=ENTITIES.SACK,
    name="Health Potion",
    consumable=consumable.HealingConsumable(amount=4),
)
lightning_scroll = Item(
    img=ENTITIES.SACK,
    name="Lightning Scroll",
    consumable=consumable.LighningDamageConsumable(damage=20, maximum_range=5),
)

dagger = Item(
    img=ENTITIES.SACK,
    name="Dagger",
    equippable=equippable.Dagger(),
)

sword = Item(img=ENTITIES.SWORD, name="Sword", equippable=equippable.Sword())

leather_armor = Item(
    img=ENTITIES.SACK,
    name="Leather Armor",
    equippable=equippable.LeatherArmor(),
)
chain_mail = Item(
    img=ENTITIES.SACK, name="Chain mail", equippable=equippable.ChainMail()
)
