from builtins import len

import csv
import HerbSearch
import HerbData
import FetchData
import Helper
import sys
import ListFunction

popularHerbs = Helper.GetListFromCSV("AnimalByPopular.csv")
animalHerbs = Helper.GetListFromCSV("animalHerbs.csv")

singlePopularHerbs = ListFunction.ExtractColumn(popularHerbs, 1)
singlePopularHerbs = ListFunction.CapList(singlePopularHerbs)

singleAnimalHerbs = ListFunction.ExtractColumn(animalHerbs, 1)
singleAnimalHerbs = ListFunction.CapList(singleAnimalHerbs)

myFinalList = ListFunction.ListSubtract(singleAnimalHerbs, singlePopularHerbs)

print(len(myFinalList))
print(myFinalList)

Helper.WriteToCSVSingle("AnimalNotPopular.csv", myFinalList)

sys.exit(0)

# Get formula list clean format
cleanFormulaList = FetchData.GetData()

print("Number of formulas " + str(len(cleanFormulaList)))

# with open("cleanData.csv", "w+") as cleanBook:
#    writer = csv.writer(cleanBook, delimiter=",")
#    writer.writerows(cleanData)


# 1 get most common dui yao list.
# uniqueList = HerbData.GetUniqueHerbList(cleanFormulaList)
# comboList = HerbData.CombineUniqueHerbs(uniqueList)
# valueCount = HerbData.GetDoublesWithValues(cleanFormulaList, comboList)
# doubleResults = valueCount[0]
# doubleResults.sort(reverse=True)
# totalComps = valueCount[1]
# for result in doubleResults:
#     print(result)
# print(totalComps)
#
# with open("doublesCompleteSort.csv", "w+", newline='') as cleanBook:
#     writer = csv.writer(cleanBook)
#     for res in doubleResults:
#         if res[0] > 0:
#             writer.writerow(res)
#
# sys.exit(0)

# 2 get unique list of herbs and count how many times in formula.
uniqueList = HerbData.GetUniqueHerbList(cleanFormulaList)
valueCount = HerbData.GetUniqueWithValues(cleanFormulaList, uniqueList)
valueCount.sort(reverse=True)
for v in valueCount:
    print(v)
Helper.WriteToCSV("PopularHerbs.csv", valueCount)
sys.exit(0)

# 3 Search list of single herbs in formula list.
searchVal = "gan cao, bai shao"
myResults = HerbSearch.Search(cleanFormulaList, searchVal)
print(myResults)

sys.exit(0)
