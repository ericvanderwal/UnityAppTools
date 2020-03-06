import csv
from urllib import request
from urllib import error
from bs4 import BeautifulSoup as bs
import time
import sys
import Helper
import lxml

completeList = []
# get the all the links of the indexs
indexList = Helper.GetListFromCSV("HerbTypeIndex.csv")

# for each link do the following
for page in indexList:
    # open the page and get the title

    indexData = request.urlopen(page[0])
    indexSoup = bs(indexData, "lxml")
    indexTitle = indexSoup.find('h1', class_='page-title').text
    # get all the links within this class
    herbTable = indexSoup.find(class_='img-matrix')
    allHerbLinks = herbTable.findAll('a')

    # get the herb names from the link titles and format
    herbNames = []
    for rawLinks in allHerbLinks:
        linkString = rawLinks.text.rstrip("\n\r")
        herbNames.append(linkString)

    # In the title, get red of the word chinese
    indexTitle = indexTitle.replace("Chinese", "")
    indexTitle = indexTitle.strip()

    # remove any empty links. Group together with function.
    cleanHerbNames = list(filter(None, herbNames))
    for herb in cleanHerbNames:
        herbPlusCat = [herb, indexTitle]
        # write to complete list list
        completeList.append(herbPlusCat)

# End by saving to CSV
Helper.WriteToCSV("HerbByType.csv", completeList)
sys.exit(0)

# Get sacred lotus herb type index pages and save to CSV
indexPage = "https://www.sacredlotus.com/go/chinese-herbs"
indexData = request.urlopen(indexPage)
indexSoup = bs(indexData, "lxml")
indexTable = indexSoup.find(class_="list-category")
paths = indexTable.find_all('a')
links = []
for p in paths:
    links.append("https://www.sacredlotus.com" + p.get('href'))

savePage = open("herbTypePath.txt", "a")

for l in links:
    savePage.write(l + "\n")

sys.exit(0)

# # Get all Pages from Index
#
# indexPage = "https://www.americandragon.com/HerbFormulaIndexA-G.html"
# host = "https://www.americandragon.com"
# indexData = request.urlopen(indexPage)
# indexSoup = BeautifulSoup(indexData, "lxml")
# indexTable = indexSoup.find("table", class_="contentsm")
# paths = indexTable.find_all('a')
# correctedPaths = []
# links = []
# cleanLinks = []
#
# # Make sure all paths are in the correct format
# for p in paths:
#     clean = p.get('href')
#
#     if clean[0] is "/":
#         newPath = host + clean
#         correctedPaths.append(newPath)
#     elif clean[1] is ".":
#         newPath = host + clean[2:]
#         correctedPaths.append(newPath)
#         # print("Need to remove two dots")
#     elif clean[0] is ".":
#         newPath = host + clean[1:]
#         correctedPaths.append(newPath)
#         # print("Need to remove one dot")
#
# savePage = open("file.text", "a")
#
# for link in correctedPaths:
#     myCleanLink = link.replace(" ", "%20")
#     cleanLinks.append(myCleanLink)
#     print(myCleanLink)
#     savePage.write(myCleanLink + "\n")
#
# savePage.close()
# sys.exit()


## From link list open URL and get all data to CSV

fullLinkList = []

myLinkList = open("file.text", "r")
for link in myLinkList:
    link = link.strip()
    fullLinkList.append(link)

myLinkList.close()
# print(fullLinkList)

pageItems = []  # items for each entry


def GetData(pageLink):
    time.sleep(0.05)
    mySoup = Connect(pageLink)

    if mySoup is None:
        print("None data was found for " + pageLink)

    else:
        #  print(mySoup)
        GetTitle(mySoup)
        GetSingleHerbs(mySoup)
        SaveToCSV()


def Connect(pageLink):
    pageRequest = request.Request(pageLink)
    try:
        request.urlopen(pageRequest)
    except error.URLError as e:
        print(e.reason)
        return None
    pageData = request.urlopen(pageLink)
    soup = BeautifulSoup(pageData, 'lxml')
    return soup


def GetTitle(mySoup):
    h3Tags = mySoup.find('h3')
    ems = h3Tags.findAll("em")
    if ems:  # title is usually in em
        title = ems[0]
        titleText = title.text
        titleText = titleText.replace('-', '')
        pageItems.append(titleText.strip())
    else:  # somtimes there is no em, so check other place
        alink = h3Tags.a
        titleText = alink.text
        titleText = titleText.replace('-', '')
        pageItems.append(titleText.strip())


def GetSingleHerbs(mySoup):
    allTables = mySoup.findAll('table')
    singleTable = allTables[3]
    allTR = singleTable.findAll("tr")
    allTR.pop(0)
    for tr in allTR:
        td = tr.find_all('td')
        asText = td[1].get_text().strip()
        asText = asText.replace('\n', '')
        asText = asText.replace('-', '')
        pageItems.append(asText)


def SaveToCSV():
    with open("FormulaData.csv", 'a', encoding="utf-8") as myFile:
        mywriter = csv.writer(myFile)
        pageItems[1:] = sorted(pageItems[1:])
        mywriter.writerow(pageItems)
        # print(pageItems)
        pageItems.clear()


num = 0
for i in fullLinkList[789:]:
    GetData(i)
    num += 1
    print(num)
