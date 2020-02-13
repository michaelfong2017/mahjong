from tkinter import *
from ttk import Entry, Button, OptionMenu
from PIL import Image, ImageTk
import random
import tkinter.filedialog
import os


from enum import Enum
import random


from featureExtractors import *



class DiscardBy(Enum):
    NOBODY = "nobody"
    PLAYER_0 = 0
    PLAYER_1 = 1
    PLAYER_2 = 2
    PLAYER_3 = 3

class Tile:
    def __init__(self, tile_code):
        self.tile_code = tile_code
        self.tile_pattern = int(int(self.tile_code) / 10)
        self.tile_number = int(self.tile_code) % 10
        self.discardedBy = DiscardBy.NOBODY

    def __str__(self):
        return f"{self.tile_code}"
    

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
        for tile in self.tiles_hand:
            tiles_13Or14.append(tile)
        for tiles in self.tiles_displayed:
            i = 0
            for tile in tiles:
                if not i == 3:
                    tiles_13Or14.append(tile)
                i = i + 1
                
        return tiles_13Or14


class OneGame:
    def __init__(self, wind, dealer):
        self.wind = wind
        self.dealer = dealer
        self.remaining_pool = []
        self.discarded_pool = []
        self.players = [Player(0), Player(1), Player(2), Player(3)]
        self.state = State
        self.state.ownPlayer = self.players[0]

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
    ownPlayer = Player
        

def main():
    print("main()")
    oneGame = OneGame(2, 3)
    print(len(oneGame.remaining_pool))
    print(oneGame.state.ownPlayer.tiles_hand[0])
    print(SimpleExtractor.getFeatures(SimpleExtractor, oneGame.state)["length of longest suit"])
    
    testState = State
    testState.ownPlayer = Player(0)
    testState.ownPlayer.tiles_hand.append(Tile("00"))
    testState.ownPlayer.tiles_hand.append(Tile("01"))
    testState.ownPlayer.tiles_hand.append(Tile("02"))
    testState.ownPlayer.tiles_hand.append(Tile("05"))
    testState.ownPlayer.tiles_hand.append(Tile("05"))
    testState.ownPlayer.tiles_hand.append(Tile("08"))
    testState.ownPlayer.tiles_hand.append(Tile("12"))
    testState.ownPlayer.tiles_hand.append(Tile("13"))
    testState.ownPlayer.tiles_hand.append(Tile("23"))
    testState.ownPlayer.tiles_hand.append(Tile("23"))
    testState.ownPlayer.tiles_hand.append(Tile("23"))
    testState.ownPlayer.tiles_hand.append(Tile("31"))
    testState.ownPlayer.tiles_hand.append(Tile("36"))
    testState.ownPlayer.tiles_hand.append(Tile("36"))
    
    print("features:")
    print(SimpleExtractor.getFeatures(SimpleExtractor, testState)["length of longest dishonor suit"])
    print(SimpleExtractor.getFeatures(SimpleExtractor, testState)["length of second and third longest dishonor suit"])
    print(SimpleExtractor.getFeatures(SimpleExtractor, testState)["length of honor suit"])
    print(SimpleExtractor.getFeatures(SimpleExtractor, testState)["is at least 11 honors"])
    
    
    

if __name__ == "__main__":
    main()
    #root = Tk()
    #Main(root)
    #root.mainloop()

    
