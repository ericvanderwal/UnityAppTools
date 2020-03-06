# make list from string using commas and new lines
def _CreateListFromString(myString):
    lineList = []
    cleanList = []
    commaList = myString.split(",")
    for s in commaList:
        if "\n" in s:
            mySplit = s.split("\n")
            lineList.extend(mySplit)
        else:
            lineList.append(s)
    for s in lineList:
        cleanList.append(s.strip())
    return cleanList


# clean string list to be cap for first character
def _CapList(myStringList):
    myNewStringList = []
    for s in myStringList:
        s = s.title()
        myNewStringList.append(s)
    return myNewStringList


# add 0 value at the start of each list
def _AddZeroValue(myStringList):
    myNewStringList = []
    for myList in myStringList:
        myList.insert(0, int(0))
        myNewStringList.append(myList)
    return myNewStringList


# check if list contains herb and adds points
def _TallyValues(searchList, fullList):
    myNewStringList = []
    for myList in fullList:
        for s in searchList:
            if s in myList:
                myList[0] = myList[0] + 1
                myNewStringList.append(myList)
    return myNewStringList


def Search(listOfForumals, searchInput):
    """Give a list of formula and a string of herbs to search. Will return a search value."""
    inputString = searchInput  # set var
    searchHerbs = _CreateListFromString(inputString)  # make list of herbs from string
    searchHerbs = _CapList(searchHerbs)  # make list with caps
    cleanHerbList = _AddZeroValue(listOfForumals)  # add dummy value
    results = _TallyValues(searchHerbs, cleanHerbList)  # get results
    return results
