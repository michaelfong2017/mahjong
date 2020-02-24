from tkinter import *
from ttk import Entry, Button, OptionMenu
from PIL import Image, ImageTk
import random
import tkinter.filedialog
import os


from enum import Enum
import random


from featureExtractors import *
from qLearningAgents import *



class DiscardBy(Enum):
    NOBODY = "nobody"
    PLAYER_0 = 0
    PLAYER_1 = 1
    PLAYER_2 = 2
    PLAYER_3 = 3

class Tile:
    def __init__(self, tile_code):
        self.tile_code = tile_code
        self.tile_integer = int(tile_code)
        self.tile_pattern = int(int(self.tile_code) / 10)
        self.tile_number = int(self.tile_code) % 10
        self.discardedBy = DiscardBy.NOBODY

    def __str__(self):
        return f"{self.tile_code}"
    
    def __lt__(self, tile):
        return self.tile_integer < tile.tile_integer
    

class TileType(Enum):
    _00 = "00"
    _01 = "01"
    _02 = "02"
    _03 = "03"
    _04 = "04"
    _05 = "05"
    _06 = "06"
    _07 = "07"
    _08 = "08"
    _10 = "10"
    _11 = "11"
    _12 = "12"
    _13 = "13"
    _14 = "14"
    _15 = "15"
    _16 = "16"
    _17 = "17"
    _18 = "18"
    _20 = "20"
    _21 = "21"
    _22 = "22"
    _23 = "23"
    _24 = "24"
    _25 = "25"
    _26 = "26"
    _27 = "27"
    _28 = "28"
    _30 = "30"
    _31 = "31"
    _32 = "32"
    _33 = "33"
    _34 = "34"
    _35 = "35"
    _36 = "36"
    _40 = "40"
    _41 = "41"
    _42 = "42"
    _43 = "43"
    _44 = "44"
    _45 = "45"
    _46 = "46"
    _47 = "47"

class Player:
    def __init__(self, player_number):
        self.player_number = player_number
        self.tiles_hand = []
        self.tiles_displayed = [] #2D
        self.tiles_flower = []
        
    def GetTiles_13Or14(self): #The 4th from gong, is not counted
        tiles_13Or14 = []
        for tiles in self.tiles_displayed:
            i = 0
            for tile in tiles:
                if not i == 3:
                    tiles_13Or14.append(tile)
                i = i + 1
        for tile in self.tiles_hand:
            tiles_13Or14.append(tile)
                
        return tiles_13Or14


class OneGame:
    def __init__(self, wind, dealer):
        self.wind = wind
        self.dealer = dealer
        self.remaining_pool = []
        self.discarded_pool = []
        self.players = [Player(0), Player(1), Player(2), Player(3)]
        self.state = State()
        self.state.ownPlayer = self.players[0]
        self.state.remaining_pool = self.remaining_pool

        for i in range(4):
            j = 0
            for tileType in TileType:
                if (i > 0 and j >= 34):
                    break
                self.remaining_pool.append(Tile(tileType.value))
                j = j + 1

        random.shuffle(self.remaining_pool)
        
        for i in range(4):
            for j in range(13):
                self.players[i].tiles_hand.append(self.remaining_pool.pop())
        self.players[self.dealer].tiles_hand.append(self.remaining_pool.pop())
        
        
    
        
    def getWind(self):
        return self.wind


class State:
    def __init__(self):
        self.ownPlayer = Player(0)
        self.remaining_pool = []
    
    def __str__(self):
        self.state = {}
        tiles_displayed = []
        i = 0
        for tiles in self.ownPlayer.tiles_displayed:
            j = 0
            for tile in tiles:
                if j == 0:
                    tiles_displayed.append([tile.tile_code])
                else:
                    tiles_displayed[i].append(tile.tile_code)
                j = j + 1
            i = i + 1
            
        tiles_hand = []
        for tile in self.ownPlayer.tiles_hand:
            tiles_hand.append(tile.tile_code)
            
        
        self.state["tiles_displayed"] = tiles_displayed
        self.state["tiles_hand"] = tiles_hand
        
        
        return f"{self.state}"
        

def main():
    print("main()")
    '''
    oneGame = OneGame(2, 3)
    print(len(oneGame.remaining_pool))
    print(oneGame.state.ownPlayer.tiles_hand[0])
    
    testState = State()
    testState.ownPlayer = Player(0)
    testState.ownPlayer.tiles_displayed.append([Tile("05")])
    testState.ownPlayer.tiles_displayed[0].append(Tile("05"))
    testState.ownPlayer.tiles_displayed[0].append(Tile("05"))
    #testState.ownPlayer.tiles_hand.append(Tile("00"))
    #testState.ownPlayer.tiles_hand.append(Tile("08"))
    #testState.ownPlayer.tiles_hand.append(Tile("10"))
    testState.ownPlayer.tiles_hand.append(Tile("11"))
    testState.ownPlayer.tiles_hand.append(Tile("12"))
    testState.ownPlayer.tiles_hand.append(Tile("12"))
    testState.ownPlayer.tiles_hand.append(Tile("12"))
    testState.ownPlayer.tiles_hand.append(Tile("13"))
    testState.ownPlayer.tiles_hand.append(Tile("13"))
    testState.ownPlayer.tiles_hand.append(Tile("13"))
    testState.ownPlayer.tiles_hand.append(Tile("14"))
    testState.ownPlayer.tiles_hand.append(Tile("28"))
    testState.ownPlayer.tiles_hand.append(Tile("28"))
    testState.ownPlayer.tiles_hand.append(Tile("28"))
    
    testNextState = State()
    testNextState.ownPlayer = Player(0)
    testNextState.ownPlayer.tiles_displayed.append([Tile("05")])
    testNextState.ownPlayer.tiles_displayed[0].append(Tile("05"))
    testNextState.ownPlayer.tiles_displayed[0].append(Tile("05"))
    #testNextState.ownPlayer.tiles_hand.append(Tile("00"))
    #testNextState.ownPlayer.tiles_hand.append(Tile("08"))
    #testNextState.ownPlayer.tiles_hand.append(Tile("10"))
    testNextState.ownPlayer.tiles_hand.append(Tile("11"))
    testNextState.ownPlayer.tiles_hand.append(Tile("12"))
    testNextState.ownPlayer.tiles_hand.append(Tile("12"))
    testNextState.ownPlayer.tiles_hand.append(Tile("12"))
    testNextState.ownPlayer.tiles_hand.append(Tile("13"))
    testNextState.ownPlayer.tiles_hand.append(Tile("13"))
    testNextState.ownPlayer.tiles_hand.append(Tile("13"))
    testNextState.ownPlayer.tiles_hand.append(Tile("14"))
    testNextState.ownPlayer.tiles_hand.append(Tile("14"))
    testNextState.ownPlayer.tiles_hand.append(Tile("28"))
    testNextState.ownPlayer.tiles_hand.append(Tile("28"))
    
    
    features = SimpleExtractor.getFeatures(SimpleExtractor, testState)
    print("features:")
    #print("length of longest dishonor suit: " + str(features["length of longest dishonor suit"]))
    for key in features:
        print(key + ": " + str(features[key]))
    '''
    
    
    
    
    '''
    approximateQAgent = ApproximateQAgent()
    print(approximateQAgent.getWeights())
    print(approximateQAgent.getQValue(testNextState))
    approximateQAgent.update(testState, testNextState, 100)
    print(approximateQAgent.getWeights())
    
    testNextState = State()
    testNextState.ownPlayer = Player(0)
    testNextState.ownPlayer.tiles_displayed.append([Tile("05")])
    testNextState.ownPlayer.tiles_displayed[0].append(Tile("05"))
    testNextState.ownPlayer.tiles_displayed[0].append(Tile("05"))
    #testNextState.ownPlayer.tiles_hand.append(Tile("00"))
    #testNextState.ownPlayer.tiles_hand.append(Tile("08"))
    #testNextState.ownPlayer.tiles_hand.append(Tile("10"))
    testNextState.ownPlayer.tiles_hand.append(Tile("11"))
    testNextState.ownPlayer.tiles_hand.append(Tile("12"))
    testNextState.ownPlayer.tiles_hand.append(Tile("12"))
    testNextState.ownPlayer.tiles_hand.append(Tile("12"))
    testNextState.ownPlayer.tiles_hand.append(Tile("32"))
    testNextState.ownPlayer.tiles_hand.append(Tile("27"))
    testNextState.ownPlayer.tiles_hand.append(Tile("34"))
    testNextState.ownPlayer.tiles_hand.append(Tile("25"))
    testNextState.ownPlayer.tiles_hand.append(Tile("04"))
    testNextState.ownPlayer.tiles_hand.append(Tile("28"))
    testNextState.ownPlayer.tiles_hand.append(Tile("28"))
    
    print(approximateQAgent.getQValue(testNextState))
    approximateQAgent.update(testState, testNextState, -50)
    print(approximateQAgent.getWeights())
    
    
    #approximateQAgent.update(testState, testNextState, 1000)
    #print(approximateQAgent.getWeights())
    '''
    
    
    
    from optparse import OptionParser
    parser = OptionParser()
    parser.add_option("-n", "--number", dest="numberOfDisplayed",
                      help="write report to FILE", metavar="FILE")
    (options, args) = parser.parse_args()
    
    numberOfDisplayed = options.numberOfDisplayed
    
    CalculateValue(numberOfDisplayed, args)
    
    
def CalculateValue(numberOfDisplayed, *args):
    state = State()
    state.ownPlayer = Player(0)
    i = 0
    for arg in args[0]:
        if i <= 3*int(numberOfDisplayed) - 1:
            if i % 3 == 0:
                state.ownPlayer.tiles_displayed.append([Tile(arg)])
            else:
                state.ownPlayer.tiles_displayed[int(i/3)].append(Tile(arg))
        else:
            state.ownPlayer.tiles_hand.append(Tile(arg))
        i = i + 1
        
        
    print("\n")
    print("state: ")
    state.ownPlayer.tiles_hand.sort()
    print(state)
    features = SimpleExtractor.getFeatures(SimpleExtractor, state)
    
    print("\n")
    print("features: ")
    #print("length of longest dishonor suit: " + str(features["length of longest dishonor suit"]))
    for key in features:
        print(key + ": " + str(features[key]))
      
    approximateQAgent = ApproximateQAgent()
    
    print("\n")
    print("weights: ")
    print(approximateQAgent.getWeights())
    
    print("\n")
    print("score scale: ")
    print('''
    三番：400
    四番：800
    五番：1200
    六番：1600
    七番：2400
    八番：3200
    ''')
    
    print("\n")
    print("value for the state: ")
    print(approximateQAgent.getQValue(state))
    
        
    
    

if __name__ == "__main__":
    main()
    #root = Tk()
    #Main(root)
    #root.mainloop()

    
