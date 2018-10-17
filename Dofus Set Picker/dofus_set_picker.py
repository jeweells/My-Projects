import constraints_handler as ch
class set:
    def __init__(self, name="Undefined", bonuses=[dict()]):
        self.name = name
        self.bonuses = [dict(), dict()] + bonuses

class dofus_object:
    def __init__(self, objname, stats, set=None, constraints = dict()):
        assert type(stats) is dict and type(constraints) is dict, "Error by defining the parameters of the object"
        self.name = objname
        self.stats = stats
        self.constraints = constraints
        self.set = set
class hat(dofus_object):
    pass
class ring(dofus_object):
    pass
class boots(dofus_object):
    pass
class dofus(dofus_object):
    pass
class trophy(dofus):
    pass
class minor_trophy(trophy):
    pass
class medium_trophy(trophy):
    pass
class major_trophy(trophy):
    pass
class belt(dofus_object):
    pass
class weapon(dofus_object):
    pass
class amulet(dofus_object):
    pass
class shield(dofus_object):
    pass
class cape(dofus_object):
    pass
class super_mount(dofus_object):
    pass
class axe(weapon):
    pass
class bow(weapon):
    pass
class dagger(weapon):
    pass
class hammer(weapon):
    pass
class scythe(weapon):
    pass
class shovel(weapon):
    pass
class staff(weapon):
    pass
class sword(weapon):
    pass
class wand(weapon):
    pass
class pet(super_mount):
    pass
class petsmount(super_mount):
    pass
class mount(super_mount):
    pass

def getfathertype(object):
    if issubclass(type(object), hat):
        return hat
    if issubclass(type(object), cape):
        return cape
    if issubclass(type(object), weapon):
        return weapon
    if issubclass(type(object), super_mount):
        return super_mount
    if issubclass(type(object), boots):
        return boots
    if issubclass(type(object), belt):
        return belt
    if issubclass(type(object), amulet):
        return amulet
    if issubclass(type(object), shield):
        return shield
    if issubclass(type(object), dofus):
        return dofus
    if issubclass(type(object), ring):
        return ring
class character:
    def __init__(self, init_stats=dict(), scrolled_stats=dict(), level=1):
        assert type(init_stats) is dict and type(scrolled_stats) is dict, "Stats error, dict type required"
        self.equipment = {
           hat: None,
           weapon: None,
           cape: None,
           boots: None,
           belt: None,
           amulet: None,
           shield: None,
           super_mount: None,
           ring: [None, None],
           dofus: [None, None, None, None, None, None]
        }

        self.current_sets = dict()
        self._ring_index = self._dofus_index = 0
        self.base_stats = init_stats
        for ekey,_ in self.base_stats.items():
            if ekey in scrolled_stats:
                assert 0 <= scrolled_stats[ekey] <= 100 and type(scrolled_stats[ekey]) is int, "Scrolled stats error, must be in [0,100] and integer type"
                self.base_stats[ekey] += scrolled_stats[ekey]
        for ekey,estat in scrolled_stats.items():
            if ekey not in self.base_stats:
                self.base_stats[ekey] = estat
        assert 0 <= level <= 200 and type(level) is int, "Level error, must be in [0,200] and integer type"
        self.level = level
        self.base_stats[ap] = self.base_stats[ap] if ap in self.base_stats else 0
        self.base_stats[mp] = self.base_stats[mp] if mp in self.base_stats else 0
        self.base_stats[ap] += 6 if level < 100 else 7
        self.base_stats[mp] += 3
        assert self.base_stats[ap] <= 12, "Ap must be lower or equal to 12"
        assert self.base_stats[mp] <= 6, "Mp must be lower or equal to 6"
        if vitality in self.base_stats:
            self.base_stats[vitality] += 50 + 5*level
        else:
            self.base_stats[vitality] = 50 + 5*level
        self.stats = dict(self.base_stats)
        self.stats[set_bonus] = 0
    def _reduce_stats(self, object):
        if object is None:
            return
        for stat, value in object.items():
            if stat not in self.stats:
                self.stats[stat] = -value
            else:
                self.stats[stat] -= value
    def _increment_stats(self, object): # Adds the character's stats with the object
        if object is None:
            return
        for stat, value in object.items():
            if stat in self.stats:
                self.stats[stat] += value
            else:
                self.stats[stat] = value
    def unwear(self, object):
        if object is None: # Nothing to unwear here
            return
        objtype = type(object) # Type of the object to unwear
        objfathertype = getfathertype(object) # What is this object ? e.g: if objtype is an axe then the father is a weapon
        takenoff = False
        if objfathertype is dofus or objfathertype is ring:
            for i in range(len(self.equipment[objfathertype])):
                if self.equipment[objfathertype][i] is object:
                    self.equipment[objfathertype][i] = None
                    takenoff = True
                    break
        else:
            if self.equipment[objfathertype] is object:
                self.equipment[objfathertype] = None
                takenoff = True
        if not takenoff:
            return

        # Reduce this object stats
        self._reduce_stats(object.stats)
        if object.set is not None: # It has a set, so we need to check the bonus as well
                self._reduce_stats(object.set.bonuses[self.current_sets[object.set]]) # Take the bonus away
                self.current_sets[object.set] -= 1 # Decrement the number of worn parts of this set
                if self.current_sets[object.set] == 0: # The bonus of this set is null and should disappear
                    del self.current_sets[object.set]
                else:
                    self._increment_stats(object.set.bonuses[self.current_sets[object.set]]) # Set the new bonus

    
    def wear_no_restrictions(self, object:dofus_object):
        if object is None:
            return None
        objtype = type(object)
        fathertype = getfathertype(object)
        # Get the old object and put the new one
        if fathertype is dofus:
            oldobject = self.equipment[dofus][self._dofus_index]
            self.equipment[dofus][self._dofus_index] = object
        elif fathertype is ring:
            oldobject = self.equipment[ring][self._ring_index]
            self.equipment[ring][self._ring_index] = object
        else:
            oldobject = self.equipment[fathertype]
            self.equipment[fathertype] = object

        # Increment and decrement characteristics
        if oldobject is not None:
            self._reduce_stats(oldobject.stats)
        self._increment_stats(object.stats)

        # Increment and decrement set bonus characteristics
        if oldobject is not None and oldobject.set is not None:
            self._reduce_stats(oldobject.set.bonuses[self.current_sets[oldobject.set]])
            self.current_sets[oldobject.set] -= 1
            if self.current_sets[oldobject.set] == 0:
                del self.current_sets[oldobject.set]
            else:
                self._increment_stats(oldobject.set.bonuses[self.current_sets[oldobject.set]])
        if object.set is not None:
            if object.set in self.current_sets:
                self._reduce_stats(object.set.bonuses[self.current_sets[object.set]])
                self.current_sets[object.set] += 1
            else:
                self.current_sets[object.set] = 1
            self._increment_stats(object.set.bonuses[self.current_sets[object.set]])

        # Set the new set bonus
        count_bonus = 0
        for _,s in self.current_sets.items():
            if s > 1:# There are more than one parts of that X set
                count_bonus += 1
        self.stats[set_bonus] = count_bonus # Set the new value
        return oldobject
    def check_all_constraints(self):
        for tmpobjtype, tmpobj in self.equipment.items():
            if tmpobj is not None:
                if tmpobjtype is dofus or tmpobjtype is ring:
                    for tmp in tmpobj:
                        if tmp is not None:
                            if not ch.check(tmp.constraints, self):
                                return False
                else:
                    if not ch.check(tmpobj.constraints, self):
                        return False
        return True
    def wear(self, object):
        oldobject = self.wear_no_restrictions(object)
        wearable = self.check_all_constraints()
        if not wearable:
            self.wear_no_restrictions(oldobject)
        return wearable
        
    def wear_old(self, object):
        objtype = type(object) # Type of this object
        # Check if it really is an object
        assert issubclass(objtype, dofus_object), f"It's not posible to wear '{objtype}', object type error"
        # Rings and Dofus, checking repetitions of the items
        if (issubclass(objtype, ring) or issubclass(objtype, dofus)): 
            for e in self.equipment[dofus if issubclass(objtype, dofus) else ring]:

                if e is object and (
                    issubclass(objtype, dofus) or # Another dofus already worn
                    ( issubclass(objtype, ring) and object.set is not None ) # Another ring that belongs to a set already worn
                    ): # The same item can't be equipped twice unless they belong to noset
                    return False
        
        self._increment_stats(object.stats) # Add the stats of the object that's going to be worn
        objtobereplaced = None # Store the objecto to be replaced
        objtobereplacedtype = None # Store the father type of the object to be worn
        def modify_simple(t, objtobereplaced): # Avoiding repeating myself
            if issubclass(objtype, t): # Hat/Cape/etc
                if self.equipment[t] is not None: # Check if there's an object equipped
                    # Saves the old object and sets the new object
                    objtobereplaced[0], self.equipment[t] = self.equipment[t], object # Choose the object to be replaced
                else:
                    # There's no object equipped
                    self.equipment[t] = object
                # Save the type of the object just replaced
                objtobereplaced[1] = t
                return True
            return False
        objtobereplacedlst = [None,None] # List to get the reference of what the function modify_simple does
        tmp = (modify_simple(hat, objtobereplacedlst) or 
        modify_simple(cape, objtobereplacedlst) or 
        modify_simple(weapon, objtobereplacedlst) or 
        modify_simple(super_mount, objtobereplacedlst) or 
        modify_simple(boots, objtobereplacedlst) or 
        modify_simple(belt, objtobereplacedlst) or 
        modify_simple(amulet, objtobereplacedlst) or 
        modify_simple(shield, objtobereplacedlst))
        # If tmp is false then the object is nothing like above
        if tmp is False:
            if issubclass(objtype, ring):# Ring
                if self.equipment[ring][self._ring_index] is not None: # Check if there's an object equipped
                    # Saves the old object and sets the new object
                    objtobereplaced, self.equipment[ring][self._ring_index] = self.equipment[ring][self._ring_index], object # Choose the object to be replaced
                else:
                    # There's nothing to be replaced
                    self.equipment[ring][self._ring_index] = object
                objtobereplacedtype = ring
            elif issubclass(objtype, dofus):# Dofus / Trophy
                if self.equipment[dofus][self._dofus_index] is not None: # Check if there's an object equipped
                    objtobereplaced, self.equipment[dofus][self._dofus_index] = self.equipment[dofus][self._dofus_index], object # Choose the object to be replaced
                else:
                    self.equipment[dofus][self._dofus_index] = object
                objtobereplacedtype = dofus
        else:
            # An object was found on modify_simple
            objtobereplaced = objtobereplacedlst[0] # Sets the object that's going to be worn
            objtobereplacedtype = objtobereplacedlst[1] # Sets its type
        

        # Decrement the set bonus of the object to be replaced
        if objtobereplaced is not None and objtobereplaced.set is not None:
            self._reduce_stats(objtobereplaced.set.bonuses[self.current_sets[objtobereplaced.set]]) # Take the bonus away
            self.current_sets[objtobereplaced.set] -= 1 # Reduce the number of worn parts of this set
            self._increment_stats(objtobereplaced.set.bonuses[self.current_sets[objtobereplaced.set]]) # Set the new bonus
        # Increment the set bonus of the object that is going to be worn
        if object.set is not None:
            if object.set in self.current_sets:
                self._reduce_stats(object.set.bonuses[self.current_sets[object.set]]) # Take the bonus away
                self.current_sets[object.set] += 1 # Increment the number of worn parts of this set
            else:
                self.current_sets[object.set] = 1
            self._increment_stats(object.set.bonuses[self.current_sets[object.set]]) # Set the new bonus
        if objtobereplaced is not None:
            self._reduce_stats(objtobereplaced.stats) # Temporarily reduce its stats

        canwear = True
        count_bonus = 0
        for _,s in self.current_sets.items():
            if s > 1:# There are more than one parts of that X set
                count_bonus += 1
        prevsetbonus = self.stats[set_bonus] # Save the previous value
        self.stats[set_bonus] = count_bonus # Set the new value
        if canwear:
            for key, part in self.equipment.items():
                if canwear is False:
                    break
                if part is not None:# Check that it's an object
                    if key is ring or key is dofus: # They have many slots
                        for slot in part:
                            if slot is not None:
                                # Make sure this item accomplish the constraints
                                 if not ch.check(slot.constraints, self):
                                     canwear = False
                                     break
                    else:
                        # Make sure this item accomplish the constraints
                        if not ch.check(part.constraints, self):
                                 canwear = False
                                 break
        if canwear: # This object can be worn
            return True
        else:# This object CANNOT be worn
            # Restore the object
            if objtobereplacedtype is ring:
                self.equipment[ring][self._ring_index] = objtobereplaced
            elif objtobereplacedtype is dofus:
                self.equipment[dofus][self._dofus_index] = objtobereplaced
            else:
                self.equipment[objtobereplacedtype] = objtobereplaced
            # Modify the set bonus
            if objtobereplaced is not None and objtobereplaced.set is not None:
                self._reduce_stats(objtobereplaced.set.bonuses[self.current_sets[objtobereplaced.set]]) # Take the bonus away
                self.current_sets[objtobereplaced.set] += 1 # Increase the number of worn parts of this set
                self._increment_stats(objtobereplaced.set.bonuses[self.current_sets[objtobereplaced.set]]) # Set the new bonus
            # Reduce the set bonus already created for the new item that failed
            if object.set is not None:
                self._reduce_stats(object.set.bonuses[self.current_sets[object.set]]) # Take the bonus away
                self.current_sets[object.set] -= 1 # Decrement the number of worn parts of this set
                if self.current_sets[object.set] == 0: # Not even an item of this set
                    del self.current_sets[object.set] # Get rid of this set
                else:
                    self._increment_stats(object.set.bonuses[self.current_sets[object.set]]) # Set the new bonus
            if objtobereplaced is not None:
                self._increment_stats(objtobereplaced.stats)
            self._reduce_stats(object.stats)
            self.stats[set_bonus] = prevsetbonus
            return False
    def getrealstat(self, statkey):
        if statkey == prospecting:
            tmpchance = self.stats.get(chance,0)
            tmpprospecting = self.stats.get(prospecting, 0)
            return tmpprospecting + 100 + int(tmpchance/10)
        elif (statkey == ap_resistance or
              statkey == mp_resistance or
              statkey == ap_reduction or
              statkey == mp_reduction):
            tmpwisdom = self.stats.get(wisdom,0)
            tmpkey = self.stats.get(statkey,0)
            return tmpkey + int(tmpwisdom/10)
        elif (statkey == initiative):
            tmpchance = self.stats.get(chance,0)
            tmpstrength = self.stats.get(strength,0)
            tmpintelligence = self.stats.get(intelligence,0)
            tmpagility = self.stats.get(agility,0)
            return self.stats.get(statkey,0) + tmpchance + tmpstrength + tmpintelligence + tmpagility
        elif (statkey == summons):
            return 1 + self.stats.get(summons,0)
        elif (statkey == lock or
              statkey == dodge):
            return self.stats.get(statkey,0) + int(self.stats.get(agility, 0)/10)
        elif (statkey == pods):
            return 1000 + self.stats.get(statkey, 0) + self.stats.get(strength,0) * 5
        else: # Any other case return the same
            return self.stats[statkey]

def show_stats(stats):
    for stat_name, value in stats.items():
        print(stat_name, ":\t", value)


def find_set(charact:character, constraints:dict, data:dict, stoprequired:list, lastiterhelper:list = None):
    __doc__ = """Data is a dictionary where the key is the type of the object that is equipped, be hat the key of the hats and so on"""

    useful_data = []
    dofus_range = ring_range = i = 0
    i = 0
    weaponlst = None
    for key, value in data.items():
        for _ in range(10):
            pass
        if key is weapon:
            weaponlst = value
            continue
        if key is dofus:
            dofus_range = (i, i+6)
            i+=5
            for _ in range(int(6)):
                useful_data.append(value)
        elif key is ring:
            ring_range = (i, i+2)
            i+= 1
            for _ in range(2):
                useful_data.append(value)
        else:
            useful_data.append(value)
        i+= 1
    useful_data.append(weaponlst) # Weapons come last since the conditions they require require to have set but other conditions require not to have it
    current_set = [None] * len(useful_data)
    per_id = [0] * len(useful_data)
    for i in range(dofus_range[0], dofus_range[1]):
        if i == dofus_range[0]: continue;
        per_id[i] = per_id[i-1] + 1
    per_id[ring_range[0]+1] = 1
    def next_id(maxindex):
        for i in range(maxindex-1, -1, -1):
            isring = ring_range[0] <= i < ring_range[1]
            isdofus = dofus_range[0] <= i < dofus_range[1]

            if ((isring or isdofus) and per_id[i] == len(useful_data[i]) + i - (ring_range[1] if isring else dofus_range[1])):
                if i == ring_range[0] or i == dofus_range[0]:
                    for j in range((ring_range[1]-ring_range[0]) if isring else (dofus_range[1]-dofus_range[0])):
                        per_id[j+i] = j
                continue
            
            per_id[i] = (per_id[i] + 1) % len(useful_data[i])
            if per_id[i] != 0:
                return i, True
        return len(per_id), False
    if lastiterhelper is not None:
        per_id = list(lastiterhelper)
    laststart = 0
    while True:
        wearable = True
        nextomodify = len(per_id)
        for i in range(laststart, len(per_id)):
            charact.unwear(current_set[i]) # Take this part off
        for i in range(laststart, len(per_id)):
            if ring_range[0] <= i < ring_range[1]: # Is ring
                charact._ring_index = i - ring_range[0]
            elif dofus_range[0] <= i < dofus_range[1]: # Is dofus
                charact._dofus_index = i-dofus_range[0]
            current_set[i] = useful_data[i][per_id[i]]
            if not charact.wear(current_set[i]):
                wearable = False
                nextomodify = i+1
                break
        if wearable:
            if ch.check(constraints, charact):
                newstats = {}
                for key, _ in charact.stats.items():
                    newstats[key] = charact.getrealstat(key)
                yield list(current_set), newstats

        laststart, isnext = next_id(nextomodify)
        if(stoprequired[0]):
            stoprequired[1] = list(per_id)
            break
        if not isnext:
            break


vitality = "vitality"
ap = "ap"
mp = "mp"
initiative = "initiative"
prospecting = "prospecting"
rang = "range"
summons = "summons"
wisdom = "wisdom"
strength = "strength"
intelligence = "intelligence"
chance = "chance"
agility = "agility"
ap_resistance = "ap_resistance"
ap_reduction = "ap_reduction"
mp_resistance = "mp_resistance"
mp_reduction = "mp_reduction"
critical_hits = "critical_hits"
heals = "heals"
lock = "lock"
dodge = "dodge"
per_final_damage = "%_final_damage"
power = "power"
critical_damage = "critical_damage"
neutral_damage = "neutral_damage"
earth_damage = "earth damage"
fire_damage = "fire_damage"
water_damage = "water_damage"
air_damage = "air_damage"
reflect = "reflect"
trap_damage = "trap_damage"
trap_power = "trap_power"
pushback_damage = "pushback_damage"
per_spell_damage = "%_spell_damage"
per_weapon_damage = "%_weapon_damage"
per_ranged_damage = "%_ranged_damage"
per_melee_damage = "%_melee_damage"
neutral_resistance = "neutral_resistance"
per_neutral_resistance = "%_neutral_resistance"
earth_resistance = "earth_resistance"
per_earth_resistance = "%_earth_resistance"
fire_resistance = "fire_resistance"
per_fire_resistance ="%_fire_resistance"
water_resistance = "water_resistance"
per_water_resistance = "%_water_resistance"
air_resistance = "air_resistance"
per_air_resistance = "%_air_resistance"
critical_resistance = "critical_resistance"
pushback_resistance = "pushback_resistance"
per_ranged_resistance = "%_ranged_resistance"
per_melee_resistance = "%_melee_resistance"
set_bonus = "set_bonus"
pods = "pods"

characteristics = {    
vitality,
ap,
mp,
initiative,
prospecting,
rang,
summons,
wisdom,
strength,
intelligence,
chance,
agility,
ap_resistance,
ap_reduction,
mp_resistance,
mp_reduction,
critical_hits,
heals,
lock,
dodge,
per_final_damage,
power,
critical_damage,
neutral_damage,
earth_damage,
fire_damage,
water_damage,
air_damage,
reflect,
trap_damage,
trap_power,
pushback_damage,
per_spell_damage,
per_weapon_damage,
per_ranged_damage,
per_melee_damage,
neutral_resistance,
per_neutral_resistance,
earth_resistance,
per_earth_resistance,
fire_resistance,
per_fire_resistance,
water_resistance,
per_water_resistance,
air_resistance,
per_air_resistance,
critical_resistance,
pushback_resistance,
per_ranged_resistance,
per_melee_resistance,
set_bonus,
pods
}


