def CompareList(firstList, secondList):
    """"Compare to two lists for cross matches."""

    returnList = []
    for item in firstList:
        for item2 in secondList:
            if item == item2:
                returnList.append(item)
    return returnList


def CompareListOfLists(firstList, columnCompare, secondList):
    """"Compare to two lists for cross matches, specify which column to use."""

    returnList = []
    for item in firstList:
        compareItem = item[columnCompare]
        for item2 in secondList:
            if compareItem == item2:
                returnList.append(item)
    return returnList


def ListSubtract(firstList, secondList):
    """"Remove one list from a second list"""

    returnList = []
    for item in firstList:
        count = 0
        for item2 in secondList:
            if item == item2:
                count += 1
        if count == 0:
            returnList.append(item)
    return returnList


# clean string list to be cap for first character
def CapList(myStringList, stripEndStartSpaces=True):
    myNewStringList = []
    for s in myStringList:
        s = s.title()
        if stripEndStartSpaces:
            s = s.strip()
        myNewStringList.append(s)
    return myNewStringList


def ExtractColumn(myList, myColumn):
    """Extract a specific column to a new list"""
    returnList = []
    for item in myList:
        returnList.append(item[myColumn])
    return returnList
