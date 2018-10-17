import dofus_set_picker as dsp




i = 0



# print(mychar.wear(amulets[0]))
# print(mychar.stats)
# print(mychar.current_sets)
# 
# print(mychar.wear(hats[0]))
# print(mychar.stats)
# print(mychar.current_sets)
# 
# print(mychar.wear(belts[0]))
# print(mychar.stats)
# print(mychar.current_sets)



import json_extractor as je


print("Decrypting...")
files= {
"files/input/200/hats.txt": "files/json/hats.json",
"files/input/200/capes.txt": "files/json/capes.json",
"files/input/200/rings.txt": "files/json/rings.json",
"files/input/pets.txt": "files/json/pets.json",
"files/input/mounts.txt": "files/json/mounts.json",
"files/input/petsmounts.txt": "files/json/petsmounts.json",
"files/input/200/axes.txt": "files/json/axes.json",
"files/input/200/bowes.txt": "files/json/bowes.json",
"files/input/200/daggers.txt": "files/json/daggers.json",
"files/input/200/hammers.txt": "files/json/hammers.json",
"files/input/200/scythes.txt": "files/json/scythes.json",
"files/input/200/shovels.txt": "files/json/shovels.json",
"files/input/200/staffs.txt": "files/json/staffs.json",
"files/input/200/swords.txt": "files/json/swords.json",
"files/input/200/wands.txt": "files/json/wands.json",
"files/input/200/boots.txt": "files/json/boots.json",
"files/input/200/belts.txt": "files/json/belts.json",
"files/input/200/shields.txt": "files/json/shields.json",
"files/input/200/amulets.txt": "files/json/amulets.json",
"files/input/dofuses.txt": "files/json/dofuses.json",
"files/input/majortrophies.txt": "files/json/majortrophies.json",
"files/input/mediumtrophies.txt": "files/json/mediumtrophies.json",
"files/input/minortrophies.txt": "files/json/minortrophies.json"
}
for fr, to in files.items():
    je.save(fr,to)
print("Done")

import json
data = {
    }
print("Loading data")
for fname,f in files.items():
    print("\tLoading", fname)
    tmpdata, tmpkey = je.load(f)
    if tmpkey in data:
        data[tmpkey] += tmpdata
    else:
        data[tmpkey] = tmpdata
    print("\tDone")
print("Done")


print("Wait...")
import threading as thd
wanttostop = [False, None, False]



def findingsets():
    recoverinfo = None
    try:
        with open("out/sets_info_recover.json", "r") as infof:
            recoverinfo = json.load(infof)
            print("Recovery session loaded")
    except:
        print("No info to load")
    i = 0 if recoverinfo is None else int(recoverinfo["next_i"])
    if recoverinfo is None:
        print("Setting set number:", i)
    fnumber = 0 if recoverinfo is None else int(recoverinfo["last_file_number_used"])
    if recoverinfo is None:
        print("Setting file nomber:", fnumber)
    fileopen= False
    tmpfname = None
    lastiterhelper = None if recoverinfo is None else list(recoverinfo["last_iteration"])
    if recoverinfo is None:
        print("Setting iteration code", lastiterhelper)
    if recoverinfo is None:
        current_json_data = {}
    else:
        tmpfname = "out/sets_found"+str(fnumber)+".json"
        print("Loading current data...")
        try:
            with open(tmpfname, "r") as f:
                current_json_data = json.load(f)
        except:
            current_json_data = {}
            print("No current data found")
        finally:
            print("Done")
        

    for finalset, finalstats in dsp.find_set(mychar, userconstraints, data, wanttostop, lastiterhelper):
        if not fileopen:# Opens a new file to store when it's required
            tmpfname = "out/sets_found"+str(fnumber)+".json"
            f = open(tmpfname, "w")
            fileopen = True
        current_json_data["Set"+str(i+1)] = {
            "Parts": [n.name for n in finalset],
            "Stats": finalstats
            }
        if (i+1) % 10000 == 0: # File reached its limit
            fileopen = False
            fnumber += 1
            json.dump(current_json_data, f)
            current_json_data = {}
            f.close()
        elif wanttostop[0]: # Stop required
            json.dump(current_json_data, f)
            current_json_data = {}
            fileopen = False
            f.close()
        i+= 1
        if wanttostop[0]:
            break
    if fileopen:
        json.dump(current_json_data, f)
        current_json_data = {}
        f.close()
    print(i, "different sets found")

    with open("out/sets_info_recover.json", "w") as infof:
        if wanttostop[0]:
            print("Saving state...")
            json.dump({"last_iteration": wanttostop[1], "last_file_number_used": fnumber, "next_i": i, "last_constraints_used": userconstraints},infof)
            print("Done")
# Here 

import re
import os
from collections import deque
mychar = dsp.character(dict(), level=200)
userconstraints = None
t = None
sessionstarted = False
autocommands = deque()
sessionpath = "out/sets_info_recover.json"
if os.path.exists(sessionpath):
    inp = input("There's a previous session, do you want to continue?\n").lower()
    if inp == "yes" or inp == "y" or inp == "1":
       autocommands.append("recover")
       autocommands.append("find")
    elif inp == "no" or inp == "n" or inp == "0":
       autocommands.append("delete")

while True:
    if not autocommands:
        inp = input("What do you want to do?\n").lower()
    else:
        inp = autocommands.popleft()
    if inp == "save":
        if sessionstarted:
            print("Saving... please wait")
            wanttostop[2] = True # Wants to save
            wanttostop[0] = True # Wants to stop
            t.join()
            wanttostop[0] = False # Restore
            sessionstarted = False
        else:
            print("There's nothing to save")
    elif inp == "new": # Creates a new session, no constraints, no stats
        if sessionstarted:
            print("Please stop the current session to clean the character")
            continue
        try:
            mychar = dsp.character(dict(), level=int(input("Character level: ")))
            userconstraints = None
            t = None
        except:
            print("Input error")
    elif inp == "delete": # Deletes the previous session
        try:
            os.remove(sessionpath)
            print("Removed successfully")
        except OSError:
            print("File is in use, can't be deleted")
    elif inp == "constraints": # Set the constraints
        if sessionstarted:
            print("Please stop the current session to set new constraints")
            continue
        backupconstraints = {}
        if len(userconstraints) > 0:
            more = input("There are already constraints set, do you want to add more or modify?\n")
            if inp == "yes" or inp == "y" or inp == "1":
               backupconstraints = userconstraints
        userconstraints =  re.finditer(r"[a-zA-Z_\s]+(>|<|<=|>=|==|=|!=)(\s)?(-)?[\s0-9]+", input("Insert constraints:\n"))
        userconstraints = [x[0].replace(" ", "") for x in userconstraints]
        newconstraints = {}
        for e in userconstraints:
            # Here we have something like agility >134
            tmp = re.search(r"(<=|>=|==|!=)", e)
            if not tmp:
                tmp = re.search(r"(<|>|=)", e)
                if not tmp:
                    print("Unknown constraints")
                    continue
            try:
                newconstraints[str(e[:tmp.span()[1]])] = int(e[tmp.span()[1]:])
            except:
                print("Unknown constraints")
                continue
                userconstraints = None
        for k,v in backupconstraints.items():
            newconstraints[k] = v
        userconstraints = newconstraints
    elif inp == "recover": # Recover the previous session
        if sessionstarted:
            print("Please stop the current session to recover a session")
            continue
        try:
            with open(sessionpath, "r") as infof:
                info = json.load(infof)
                userconstraints = dict(info["last_constraints_used"])
        except:
            print("No info to load")
            continue
    elif inp == "initialize": # Initializes the character stats
        if sessionstarted:
            print("Please stop the current session to initialize new characteristics")
            continue
        backupstats = {}
        if len(mychar.stats) > 0:
            more = input("There are already stats set, do you want to add more or modify?\n")
            if inp == "yes" or inp == "y" or inp == "1":
               backupstats = mychar.stats
        tmpstats =  re.finditer(r"[a-zA-Z_\s]+(-)?[\s0-9]+", input("Insert constraints:\n"))
        tmpstats = [x[0].replace(" ", "") for x in tmpstats]
        newstats = {}
        for e in tmpstats:
            # Here we have something like agility >134
            statname = re.search(r"[a-zA-Z_\s]+", e)
            statvalue = re.search(r"(-)?[\s0-9]+", e)
            try:
                newstats[str(statname)] = int(statvalue)
            except:
                print("Unknown stats")
                continue
                tmpstats = None
        for k,v in backupstats.items():
            newstats[k] = v
        mychar.stats = newstats
    elif inp == "find": # Starts the search
        if sessionstarted:
            print("Please stop the current session to start a new session")
            continue
        sessionstarted = True
        t = thd.Thread(target=findingsets)
        t.start()
        pass
    elif inp == "cancel": # Cancels the search
        if not sessionstarted:
            print("There's nothing to cancel")
            continue
        print("Cancelling... please wait")
        wanttostop[2] = False # Doesn't Want to save
        wanttostop[0] = True # Wants to stop
        t.join()
        wanttostop[0] = False # Restore
        sessionstarted = False
    elif inp == "help":
        print("Commands available are:")
        print("\tnew: Creates a new character, erases stats and constraints")
        print("\tinitialize: Creates the startup stats for the character")
        print("\tshow stats: Shows the current character stats")
        print("\tconstraints: Sets the constraints for the search")
        print("\tshow constraints: Shows the current constraints")
        print("\tfind: Stats the search")
        print("\tsave: Stop and saves the current search to continue later")
        print("\tcancel: Cancels the current search")
        print("\trecover: Recovers the previous session")
        print("\thelp: Shows the information about the commands")
        print("\texit: Finishes the program to exit normally")
    elif inp == "exit":
        if sessionstarted:
            autocommands.append("save")
            autocommands.append("exit")
            continue
        break
    elif inp == "show constraints":
        dsp.show_stats(userconstraints)
    elif inp == "show stats":
        dsp.show_stats(mychar.stats)
    else:
        print("Unknown command")