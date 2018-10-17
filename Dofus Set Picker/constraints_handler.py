import dofus_set_picker as dsp
def check(constraints, charact):
    if constraints is None:
        return True
    assert type(constraints) is dict, f"Constraints isn't a dictionary is {type(constraints)}"
    assert type(charact) is dsp.character, "Charact isn't a character is {type(stats)}"
    for stat, constr in constraints.items():
        if stat.endswith("<="):
            if charact.getrealstat(stat[:-2]) > constr:
                return False
        elif stat.endswith(">="):
            if charact.getrealstat(stat[:-2]) < constr:
                return False
        elif stat.endswith("!="):
            if charact.getrealstat(stat[:-2]) == constr:
                return False
        elif stat.endswith("="):
            if charact.getrealstat(stat[:-1]) != constr:
                return False
        elif stat.endswith("<"):
            if charact.getrealstat(stat[:-1]) >= constr:
                return False
        elif stat.endswith(">"):
            if charact.getrealstat(stat[:-1]) <= constr:
                return False
    return True


