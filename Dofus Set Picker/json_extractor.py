import dofus_set_picker as dsp
import re
from set_creator import sets
from json import loads as ld
def load(jsonfilename):
    data = []
    type = jsonfilename.replace(".json", "").replace("files/json/", "")
    print(type, jsonfilename)
    fathertype = None
    def adddata(data, jsonfilename, objtype):
        tmp = None
        with open(jsonfilename) as f:
            tmp = f.read()
        for name, stats in ld(tmp).items():
            constr = dict()
            setname = None
            if "constraints" in stats:
                constr = stats["constraints"]
                del stats["constraints"]
            if "set" in stats:
                try:
                    setname = sets[stats["set"]]
                except:
                    print(stats["set"], ": Set no encontrado, se ignorará")
                del stats["set"]
            data.append(objtype(name, dict(stats), setname, constr))
    if type == "hats":
        adddata(data, jsonfilename, dsp.hat)
        fathertype = dsp.hat
    elif type == "capes":
        adddata(data, jsonfilename, dsp.cape)
        fathertype = dsp.cape
    elif type == "rings":
        adddata(data, jsonfilename, dsp.ring)
        fathertype = dsp.ring
    elif type == "pets":
        adddata(data, jsonfilename, dsp.pet)
        fathertype = dsp.super_mount
    elif type == "mounts":
        fathertype = dsp.super_mount
        adddata(data, jsonfilename, dsp.mount)
    elif type == "petsmounts":
        fathertype = dsp.super_mount
        adddata(data, jsonfilename, dsp.petsmount)
    elif type == "axes":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.axe)
    elif type == "bowes":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.bow)
    elif type == "daggers":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.dagger)
    elif type == "hammers":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.hammer)
    elif type == "scythes":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.scythe)
    elif type == "shovels":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.shovel)
    elif type == "staffs":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.staff)
    elif type == "swords":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.sword)
    elif type == "wands":
        fathertype = dsp.weapon
        adddata(data, jsonfilename, dsp.wand)
    elif type == "boots":
        fathertype = dsp.boots
        adddata(data, jsonfilename, dsp.boots)
    elif type == "belts":
        fathertype = dsp.belt
        adddata(data, jsonfilename, dsp.belt)
    elif type == "shields":
        fathertype = dsp.shield
        adddata(data, jsonfilename, dsp.shield)
    elif type == "amulets":
        fathertype = dsp.amulet
        adddata(data, jsonfilename, dsp.amulet)
    elif type == "dofuses":
        fathertype = dsp.dofus
        adddata(data, jsonfilename, dsp.dofus)
    elif type == "majortrophies":
        fathertype = dsp.dofus
        adddata(data, jsonfilename, dsp.major_trophy)
    elif type == "mediumtrophies":
        fathertype = dsp.dofus
        adddata(data, jsonfilename, dsp.medium_trophy)
    elif type == "minortrophies":
        fathertype = dsp.dofus
        adddata(data, jsonfilename, dsp.minor_trophy)
    return data, fathertype
# Taking info from dofusplanner.com
def save(in_name, out_name):
    names_written = set() 
    with open(in_name, "r") as f_in:
        with open(out_name, "w") as f_out:
            cycle = 0
            n_current_stats = 0
            f_out.write("{\n")
            for l in f_in.readlines():
                # An object is structured as follows:

                # Object name
                # Level
                # Set name (Optional)
                # Amount_of_the_stat Name_of_the_stat
                # ... Repeat
                # Conditions
                # Amount_of_the_stat operator Name_of_the_stat
                # ... Repeat
                if cycle == 0:# Name
                    if l == "\n" or re.search(r"(Linked|For each| emote| turn|Pandawa)", l):
                        continue
                    if (re.match(r"^AP: [0-9]+\n$", l) or
                        re.match(r"^Range: [0-9]+(-[0-9]+)?\n$", l) or
                        re.match(r"^Crit: [0-9]+%\n$", l) or
                        re.match(r"^Bonus: (\+|-)[0-9]+\n$", l) or
                        re.match(r"^[0-9]+ / turn\n$", l)):
                        continue
                    line = l.strip('\n')
                    tmp = line
                    i = 1
                    while line in names_written:
                        line = tmp+" "+str(i)
                        i+=1
                    
                    names_written.add(line)

                    f_out.write("\t\"{0}\": {{\n".format(line))
                    cycle += 1
                elif cycle == 1:# Posible set name
                    if l == "\n":
                        cycle = 0
                        f_out.write("\t},\n") # Finish the object
                        continue
                    if (re.match(r"^Level [0-9]+$", l.strip('\n')) or 
                        re.match(r"^(-)?[0-9]+ to (-)?[0-9]+ \([a-zA-Z\s]+\)\n$",l)): # Level line
                        continue
                    if (re.match(r"^(-)?[0-9]+", l)  or
                        re.search(r"(Linked|For each| emote| turn|Pandawa)", l) or 
                        l == "Effects:\n"): # Set name overpassed
                        cycle += 1
                    else:
                        f_out.write("\t\t\"set\": \"{0}\",\n".format(l.strip('\n')))
                        cycle += 1
                        continue
                if cycle == 2:# Stats
                    if(re.match(r"^(-)?[0-9]+ to (-)?[0-9]+ \([a-zA-Z\s]+\)\n$",l)):
                        continue
                    if l == "Effects:\n":
                        continue
                    if re.search(r"(Linked|For each|emote|turn|Pandawa)", l):
                        if n_current_stats != 0:
                            f_out.seek(f_out.tell()-3)# Take the comma and \n from the previous stat added
                        f_out.write("\n")
                        cycle = 0
                        f_out.write("\t},\n") # Finish the object
                        n_current_stats = 0
                        continue
                    #if l == "\n":
                    #    if n_current_stats != 0:
                    #        f_out.seek(f_out.tell()-3)# Take the comma and \n from the previous stat added
                    #    f_out.write("\n")
                    #    cycle = 0
                    #    f_out.write("\t},\n") # Finish the object
                    #    n_current_stats = 0
                    #    continue
                    a = re.match(r"^(-)?[0-9]+", l)
                    if not a:
                        if l == "Conditions\n":
                            f_out.write("\t\t\"constraints\":{\n")
                            n_current_stats = 0
                            cycle += 1
                        else:
                            if n_current_stats != 0:
                                f_out.seek(f_out.tell()-3)# Take the comma and \n from the previous stat added
                            f_out.write("\n")
                            cycle = 0
                            f_out.write("\t},\n") # Finish the object
                            n_current_stats = 0
                    else:
                        n_current_stats += 1
                        # Build the stat name
                        if l[-1] != '\n':
                            line = l[len(a[0]):].strip().replace(" ", "_").replace("%", "per").lower()
                        else:
                            line = l[len(a[0]):-1].strip().replace(" ", "_").replace("%", "per").lower()
                        f_out.write("\t\t\"{0}\": {1},\n".format(line, a[0]))
                        if l[-1] != '\n':
                            f_out.seek(f_out.tell()-3)
                            cycle = 0
                            f_out.write("\n\t},\n") # Finish the object

                elif cycle == 3:# Conditions
                    a = re.match(r"^[a-zA-Z\s]+ (>|<|!=|==|=|<=|>=) [0-9]+(( and| or)( )?)?(\n)?$", l)
                    if not a:
                        f_out.seek(f_out.tell()-3)# Take the comma and \n from the previous stat added
                        f_out.write("\n\t\t}\n\t},\n") # Finish the object
                        cycle = 0
                    else:
                        # Build the stat name
                        a = re.search(r"(-)?[0-9]+",l)
                        
                        line = (l[:a.span()[0]]
                                .strip()
                                .replace(" ", "_")
                                .replace("%", "per")
                                .lower()
                                .replace("_<", "<")
                                .replace("_>", ">"))
                        f_out.write("\t\t\t\"{0}\": {1},\n".format(line, a[0]))
                        if l[-1] != '\n':
                            f_out.seek(f_out.tell()-3)# Take the comma and \n from the previous stat added
                            f_out.write("\n\t\t}\n\t},\n") # Finish the object
                            cycle = 0
            f_out.seek(f_out.tell()-3)
            f_out.write("\n}")
