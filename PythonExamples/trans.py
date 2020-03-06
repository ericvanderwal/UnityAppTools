# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'C:\trans.ui'
#
# Created by: PyQt5 UI code generator 5.11.3
#
# WARNING! All changes made in this file will be lost!

import csv
import HerbSearch
import HerbData
import FetchData
import sys

from PyQt5 import QtCore, QtGui, QtWidgets


class Ui_MainWindow(object):
    def setupUi(self, MainWindow):
        MainWindow.setObjectName("MainWindow")
        MainWindow.resize(481, 634)
        self.centralwidget = QtWidgets.QWidget(MainWindow)
        self.centralwidget.setObjectName("centralwidget")
        self.headerImage = QtWidgets.QGraphicsView(self.centralwidget)
        self.headerImage.setGeometry(QtCore.QRect(0, 0, 481, 81))
        sizePolicy = QtWidgets.QSizePolicy(QtWidgets.QSizePolicy.Fixed, QtWidgets.QSizePolicy.Fixed)
        sizePolicy.setHorizontalStretch(0)
        sizePolicy.setVerticalStretch(0)
        sizePolicy.setHeightForWidth(self.headerImage.sizePolicy().hasHeightForWidth())
        self.headerImage.setSizePolicy(sizePolicy)
        self.headerImage.setAutoFillBackground(False)
        self.headerImage.setObjectName("headerImage")
        self.herbInput = QtWidgets.QTextEdit(self.centralwidget)
        self.herbInput.setGeometry(QtCore.QRect(220, 110, 251, 141))
        self.herbInput.setObjectName("herbInput")
        self.formulaOutput = QtWidgets.QTextEdit(self.centralwidget)
        self.formulaOutput.setGeometry(QtCore.QRect(10, 280, 461, 301))
        self.formulaOutput.setObjectName("formulaOutput")
      #  self.formulaOutput.lineWrapMode()
        self.searchButton = QtWidgets.QPushButton(self.centralwidget)
        self.searchButton.setGeometry(QtCore.QRect(10, 220, 201, 31))
        self.searchButton.setObjectName("searchButton")
        self.line = QtWidgets.QFrame(self.centralwidget)
        self.line.setGeometry(QtCore.QRect(0, 90, 471, 16))
        self.line.setFrameShape(QtWidgets.QFrame.HLine)
        self.line.setFrameShadow(QtWidgets.QFrame.Sunken)
        self.line.setObjectName("line")
        self.line_2 = QtWidgets.QFrame(self.centralwidget)
        self.line_2.setGeometry(QtCore.QRect(0, 260, 471, 16))
        self.line_2.setFrameShape(QtWidgets.QFrame.HLine)
        self.line_2.setFrameShadow(QtWidgets.QFrame.Sunken)
        self.line_2.setObjectName("line_2")
        self.instructionImage = QtWidgets.QGraphicsView(self.centralwidget)
        self.instructionImage.setGeometry(QtCore.QRect(10, 110, 201, 101))
        self.instructionImage.setObjectName("instructionImage")
        MainWindow.setCentralWidget(self.centralwidget)
        self.menubar = QtWidgets.QMenuBar(MainWindow)
        self.menubar.setGeometry(QtCore.QRect(0, 0, 481, 21))
        self.menubar.setObjectName("menubar")
        MainWindow.setMenuBar(self.menubar)
        self.statusbar = QtWidgets.QStatusBar(MainWindow)
        self.statusbar.setObjectName("statusbar")
        MainWindow.setStatusBar(self.statusbar)

        self.retranslateUi(MainWindow)
        QtCore.QMetaObject.connectSlotsByName(MainWindow)

        #   self.searchButton.clicked.connect(self.fetchData)
        self.searchButton.clicked.connect(self.fetchData)

    def retranslateUi(self, MainWindow):
        _translate = QtCore.QCoreApplication.translate
        MainWindow.setWindowTitle(_translate("MainWindow", "MainWindow"))
        self.herbInput.setText(_translate("MainWindow", "Input list of single herbs here"))
        self.formulaOutput.setText(_translate("MainWindow", "Formula Output ..."))
        self.searchButton.setText(_translate("MainWindow", "Search Formulas"))

    def fetchData(self):
        searchVal = self.herbInput.toPlainText()
        print(searchVal)

        cleanHerbList = FetchData.GetData()
        myResults = HerbSearch.Search(cleanHerbList, searchVal)
        myResults.sort(reverse=True)

        for res in myResults[:10]:
            print(res)
            self.formulaOutput.setPlainText(str(res))


if __name__ == "__main__":
    import sys

    app = QtWidgets.QApplication(sys.argv)
    MainWindow = QtWidgets.QMainWindow()
    ui = Ui_MainWindow()
    ui.setupUi(MainWindow)
    MainWindow.show()
    sys.exit(app.exec_())
