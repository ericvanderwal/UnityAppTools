def GetUniqueHerbList(listOfFormulas):
    """Get a list of unique herbs from the list of formulas. Only every herb once"""
    # make singe list of all of the herbs
    singleHerbList = []
    for formula in listOfFormulas:
        for herb in formula[1:]:
            singleHerbList.append(herb)
    # get a list of just unique herbs
    uniqueHerbList = []
    for herb in singleHerbList:
        if herb not in uniqueHerbList:
            uniqueHerbList.append(herb)
    # remove dirty words from unique list
    if "" in uniqueHerbList:
        uniqueHerbList.remove("")
    if "?" in uniqueHerbList:
        uniqueHerbList.remove("?")
    # sort list
    uniqueHerbList.sort()
    return uniqueHerbList


def GetUniqueWithValues(listOfFormulas, uniqueHerbList):
    """Compare list of unique single herbs against list of formulas. Return herbs with values."""
    # set up return value
    herbListWithValues = []

    for herb in uniqueHerbList:
        valueCount = 0
        for formula in listOfFormulas:
            valueCount += formula.count(herb)
        newHerbValue = [valueCount, herb]
        herbListWithValues.append(newHerbValue)
    return herbListWithValues


def GetDoublesWithValues(listOfFormulas, doublesList):
    """Compare list of unique doubles herbs against list of formulas. Return herbs doubles with values."""
    herbListWithValues = []  # set up return value
    comps = 1
    for double in doublesList:
        # print(comps)
        herb1 = double[0]
        herb2 = double[1]
        valueCount = 0
        for formula in listOfFormulas:  # loop through each formula and check for
            comps = comps + 1
            if herb1 in formula:
                if herb2 in formula:
                    valueCount += 1
        newDouble = [herb1, herb2]
        newDouble.sort()
        newDouble.insert(0, valueCount)  # add value at start of double
        herbListWithValues.append(newDouble)  # add double to return list
    finalValues = (herbListWithValues, comps)
    return finalValues


def CombineUniqueHerbs(uniqueHerbList):
    """Create doubles list from single unique herbs combined together."""
    combo_list = []
    uniqueHerbList2 = uniqueHerbList.copy()
    for herb1 in uniqueHerbList:
        for herb2 in uniqueHerbList2:
            if herb1 != herb2:
                doubles = [herb1, herb2]
                combo_list.append(doubles)
    return combo_list
