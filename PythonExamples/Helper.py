import csv

def WriteToCSV(fileName, listToWrite):
    """Write to a csv file using list of lists"""
    with open(fileName, "w+", newline='') as cleanBook:
        writer = csv.writer(cleanBook, delimiter=",")
        writer.writerows(listToWrite)


def WriteToCSVSingle(fileName, listToWrite):
    """Write to a csv file for single list"""
    with open(fileName, "w+", newline='') as cleanBook:
        writer = csv.writer(cleanBook, delimiter=",")
        for line in listToWrite:
            cleanBook.write(line)
            cleanBook.write("\n")


def GetListFromCSV(fileName):
    """Get list from CSV file"""
    with open(fileName, "r", encoding='utf-8-sig') as book:
        reader = csv.reader(book)
        myList = list(reader)
        # remove any None values, include empty list values
        listOfLists = list(filter(None, myList))
        return listOfLists
