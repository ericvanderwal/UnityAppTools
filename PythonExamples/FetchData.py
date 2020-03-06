import sys
import csv


def GetData():
    """Get Herb Data from CSV file"""
    cleanHerbList = []

    with open("FormulaData.csv", "r") as book:
        reader = csv.reader(book)
        bookList = list(reader)

    # remove any None values, include empty list values
    listOfLists = list(filter(None, bookList))

    # remove any \n , extra spaces and ()
    for myList in listOfLists:
        newList = [" ".join(item.split()) for item in myList]
        newList = [item.replace("\n", "") for item in newList]
        newList = [item.replace("(", "") for item in newList]
        newList = [item.replace(")", "") for item in newList]
        cleanHerbList.append(newList)

    return cleanHerbList
