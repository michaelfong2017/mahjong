import util
import copy

class FeatureExtractor:
    def getFeatures(self, state):
        """
          Returns a dict from features to counts
          Usually, the count will just be 1.0 for
          indicator functions.
        """
        util.raiseNotDefined()


class SimpleExtractor(FeatureExtractor):
    def getFeatures(self, state):
        features = util.Counter()
        
        state.ownPlayer.tiles_hand.sort()

        tiles_13Or14 = state.ownPlayer.GetTiles_13Or14()
        
        """
        Count the length of different suits
        - Exluding quadruplets
        """
        numberOfDot = 0
        for tile in tiles_13Or14:
            if tile.tile_pattern == 0:
                numberOfDot = numberOfDot + 1
        numberOfBamboo = 0
        for tile in tiles_13Or14:
            if tile.tile_pattern == 1:
                numberOfBamboo = numberOfBamboo + 1
        numberOfCharacter = 0
        for tile in tiles_13Or14:
            if tile.tile_pattern == 2:
                numberOfCharacter = numberOfCharacter + 1
        numberOfHonor = 0
        for tile in tiles_13Or14:
            if tile.tile_pattern == 3:
                numberOfHonor = numberOfHonor + 1
                
        numbersOfDishonorSuits = []
        numbersOfDishonorSuits.append(numberOfDot)
        numbersOfDishonorSuits.append(numberOfBamboo)
        numbersOfDishonorSuits.append(numberOfCharacter)
        numbersOfDishonorSuits = sorted(numbersOfDishonorSuits, reverse = True)
        features["length of longest dishonor suit"] = numbersOfDishonorSuits[0]
        features["length of second and third longest dishonor suit"] = numbersOfDishonorSuits[1] + numbersOfDishonorSuits[2]
        features["length of honor suit"] = numberOfHonor
        if numberOfHonor >= 11:
            features["is at least 11 honors"] = 1
        else:
            features["is at least 11 honors"] = 0



        """
        Count triplets
        - Including quadruplets
        """
            
        features["number of triplets"] = SimpleExtractor.GetNumberOfTriplets(self, state.ownPlayer)


        """
        Count duplets
        """
            
        features["number of duplets"] = SimpleExtractor.GetNumberOfDuplets(self, state.ownPlayer)


        """
        Count sequence
        ##########
        """
        features["number of sequences"] = SimpleExtractor.GetNumberOfSequences(self, state.ownPlayer)
        
        
        """
        Count good half sequence
        - Two tiles should be isolated
        ##########
        """
        features["number of good half sequences"] = SimpleExtractor.GetNumberOfGoodHalfSequences(self, state.ownPlayer)
        
        
        """
        Count bad half sequence
        - The two tiles should be isolated
        ##########
        """
        #features["number of bad half sequences"] = #SimpleExtractor.GetNumberOfBadHalfSequences(self, state.ownPlayer)
        
        
        """
        Count maximum number of melds
        """
        features["number of melds"] = SimpleExtractor.GetNumberOfMelds(self, state.ownPlayer)
        
        
        """
        Count maximum number of melds with eyes
        """
        features["number of melds with eyes"] = SimpleExtractor.GetNumberOfMeldsWithEyes(self, state.ownPlayer)
        
        
        """
        Count number of orphans
        """
        features["number of orphans"] = SimpleExtractor.GetNumberOfOrphans(self, state.ownPlayer)
        
        
        """
        Is 13 orphans
        """
        features["is 13 orphans"] = SimpleExtractor.Is_13_Orphans(self, state.ownPlayer)
        
        """
        IsWin
        """
        features["is win"] = SimpleExtractor.IsWin(self, state.ownPlayer)
        
        
        """
        Is10Score, Is9Score, etc
        """
        features["is 10 score"] = SimpleExtractor.Is10Score(self, state.ownPlayer)
        features["is 8 score"] = SimpleExtractor.Is8Score(self, state.ownPlayer)
        features["is 7 score"] = SimpleExtractor.Is7Score(self, state.ownPlayer)
        features["is 6 score"] = SimpleExtractor.Is6Score(self, state.ownPlayer)
        features["is 3 score"] = SimpleExtractor.Is3Score(self, state.ownPlayer)
        


        """
        Remaining pool count
        ##########
        """
        features["remaining pool count"] = len(state.remaining_pool)


        #features.divideAll(10.0)
        return features
        
    def IsQuadruplet(self, player, tile):
        count = 0
        for eachTile in player.tiles_hand:
            if eachTile.tile_integer == tile.tile_integer:
                count = count + 1
        if count == 4:
            return True
        else:
            return False
            
    def IsTriplet(self, player, tile):
        count = 0
        for eachTile in player.tiles_hand:
            if eachTile.tile_integer == tile.tile_integer:
                count = count + 1
        if count == 3:
            return True
        else:
            return False

    def IsDuplet(self, player, tile):
        count = 0
        for eachTile in player.tiles_hand:
            if eachTile.tile_integer == tile.tile_integer:
                count = count + 1
        if count == 2:
            return True
        else:
            return False
            
    def IsSingle(self, player, tile):
        count = 0
        for eachTile in player.tiles_hand:
            if eachTile.tile_integer == tile.tile_integer:
                count = count + 1
        if count == 1:
            return True
        else:
            return False

    def GetNumberOfTriplets(self, player): #Include quadruplets
        numberOfTriplets = 0
                
        for tile in player.tiles_hand:
            if SimpleExtractor.IsQuadruplet(self, player, tile):
                numberOfTriplets = numberOfTriplets + 1
        numberOfTriplets = int(numberOfTriplets / 4)
        temp = numberOfTriplets
        numberOfTriplets = 0
                
        for tile in player.tiles_hand:
            if SimpleExtractor.IsTriplet(self, player, tile):
                numberOfTriplets = numberOfTriplets + 1
        numberOfTriplets = int(numberOfTriplets / 3)
        numberOfTriplets = numberOfTriplets + temp
        
        for tiles in player.tiles_displayed:
            if tiles[0].tile_integer == tiles[1].tile_integer:
                numberOfTriplets = numberOfTriplets + 1
        
        return numberOfTriplets
        
    def GetNumberOfDuplets(self, player):
        numberOfDuplets = 0
            
        for tile in player.tiles_hand:
            if SimpleExtractor.IsDuplet(self, player, tile):
                numberOfDuplets = numberOfDuplets + 1
        numberOfDuplets = int(numberOfDuplets / 2)
    
        return numberOfDuplets

    def GetNumberOfSequences(self, player):
        count = 0
        tiles_hand_copy = copy.deepcopy(player.tiles_hand)
        indexes = []
        
        def RemoveSequence(self, count):
            for i in range(len(tiles_hand_copy)):
                if i == 0:
                    continue
                if len(indexes) == 0:
                    if tiles_hand_copy[i].tile_pattern <= 2 and tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
                        indexes.append(i - 1)
                        indexes.append(i)
                else:
                    if tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer:
                        continue
                    elif tiles_hand_copy[i].tile_pattern <= 2 and tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
                        indexes.append(i)
                        count = count + 1
                        tiles_hand_copy.pop(indexes[2])
                        tiles_hand_copy.pop(indexes[1])
                        tiles_hand_copy.pop(indexes[0])
                        indexes.clear()
                        count = RemoveSequence(self, count)
                        return count
                    
                    else:
                        indexes.clear()
                        
            return count
            
        count = RemoveSequence(self, count)
        
        for tiles in player.tiles_displayed:
            if tiles_hand_copy[i].tile_pattern <= 2 and tiles[1].tile_integer == tiles[0].tile_integer + 1:
                count = count + 1
            
        return count
    
    
    def GetNumberOfSequencesExceptMelds(self, player):
        count = 0
        tiles_hand_copy = copy.deepcopy(player.tiles_hand)
        indexes = []
        
        def RemoveSequence(self, count):
            for i in range(len(tiles_hand_copy)):
                if i == 0:
                    continue
                if len(indexes) == 0:
                    if tiles_hand_copy[i].tile_pattern <= 2 and tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
                        indexes.append(i - 1)
                        indexes.append(i)
                else:
                    if tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer:
                        continue
                    elif tiles_hand_copy[i].tile_pattern <= 2 and tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
                        indexes.append(i)
                        count = count + 1
                        tiles_hand_copy.pop(indexes[2])
                        tiles_hand_copy.pop(indexes[1])
                        tiles_hand_copy.pop(indexes[0])
                        indexes.clear()
                        count = RemoveSequence(self, count)
                        return count
                    
                    else:
                        indexes.clear()
                        
            return count
            
        count = RemoveSequence(self, count)
        
        return count
        
        
    def GetNumberOfGoodHalfSequences(self, player): #The two tiles should be isolated
        tiles_hand = player.tiles_hand
        numberOfGoodHalfSequences = 0
        
        i = 0
        for tile in tiles_hand:
            if i == 0:
                i = i + 1
                continue
            elif len(tiles_hand) == 2:
                if tiles_hand[0].tile_pattern <= 2 and tiles_hand[1].tile_integer == tiles_hand[0].tile_integer + 1:
                    return 1
            elif i == len(tiles_hand) - 1:
                if tiles_hand[i - 1].tile_pattern <= 2 and tiles_hand[i].tile_integer == tiles_hand[i - 1].tile_integer + 1:
                    numberOfGoodHalfSequences = numberOfGoodHalfSequences + 1
            else:
                hasAnotherHalf = False
                for j in range(len(tiles_hand) - 1 - i):
                    if (tiles_hand[i + j + 1].tile_integer == tiles_hand[i].tile_integer + 1):
                        hasAnotherHalf = True
                if tiles_hand[i - 1].tile_pattern <= 2 and tiles_hand[i].tile_integer == tiles_hand[i - 1].tile_integer + 1 and not hasAnotherHalf:
                    numberOfGoodHalfSequences = numberOfGoodHalfSequences + 1
            
            i = i + 1
            
        return numberOfGoodHalfSequences
        
    #def GetNumberOfBadHalfSequences(self, player): #The two tiles should be isolated
        #return 0
        
    def GetNumberOfMelds(self, player): #without eyes
        tiles_hand = player.tiles_hand
        
        indexes = []
        resume = 0
        for i in range(len(tiles_hand)):
            if i < resume:
                continue
            if SimpleExtractor.IsQuadruplet(self, player, tiles_hand[i]):
                indexes.append(i)
                resume = i + 4
            elif SimpleExtractor.IsTriplet(self, player, tiles_hand[i]):
                indexes.append(i)
                resume = i + 3
                
        numberOfTriplets = len(indexes)
        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player)
        if numberOfTriplets == 4:
            return 4
        for i in range(numberOfTriplets):
            if i == 0:
                for j in range(numberOfTriplets):
                    tile = tiles_hand.pop(indexes[j])
                    tiles_hand.pop(indexes[j])
                    tiles_hand.pop(indexes[j])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 1:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 1
                    tiles_hand.insert(indexes[j], tile)
                    tiles_hand.insert(indexes[j], tile)
                    tiles_hand.insert(indexes[j], tile)
            if i == 1:
                if numberOfTriplets >= 2:
                    tile1 = tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tile0 = tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[1], tile1)
                if numberOfTriplets >= 3:
                    tile1 = tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tile0 = tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    # # # # # # # # # #
                    tile1 = tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tile0 = tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                if numberOfTriplets == 4:
                    tile1 = tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tile0 = tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[3], tile1)
                    tiles_hand.insert(indexes[3], tile1)
                    tiles_hand.insert(indexes[3], tile1)
                    # # # # # # # # # #
                    tile1 = tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tile0 = tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[3], tile1)
                    tiles_hand.insert(indexes[3], tile1)
                    tiles_hand.insert(indexes[3], tile1)
                    # # # # # # # # # #
                    tile1 = tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tile0 = tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[2], tile0)
                    tiles_hand.insert(indexes[2], tile0)
                    tiles_hand.insert(indexes[2], tile0)
                    tiles_hand.insert(indexes[3], tile1)
                    tiles_hand.insert(indexes[3], tile1)
                    tiles_hand.insert(indexes[3], tile1)
            if i == 2:
                if numberOfTriplets >= 3:
                    tile2 = tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tile1 = tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tile0 = tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[2], tile2)
                    tiles_hand.insert(indexes[2], tile2)
                    tiles_hand.insert(indexes[2], tile2)
                if numberOfTriplets == 4:
                    tile2 = tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tile1 = tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tile0 = tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[1], tile1)
                    tiles_hand.insert(indexes[3], tile2)
                    tiles_hand.insert(indexes[3], tile2)
                    tiles_hand.insert(indexes[3], tile2)
                    # # # # # # # # # #
                    tile2 = tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tile1 = tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tile0 = tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[0], tile0)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[3], tile2)
                    tiles_hand.insert(indexes[3], tile2)
                    tiles_hand.insert(indexes[3], tile2)
                    # # # # # # # # # #
                    tile2 = tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tiles_hand.pop(indexes[3])
                    tile1 = tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tiles_hand.pop(indexes[2])
                    tile0 = tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    tiles_hand.pop(indexes[1])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, player) + 2
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[1], tile0)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[2], tile1)
                    tiles_hand.insert(indexes[3], tile2)
                    tiles_hand.insert(indexes[3], tile2)
                    tiles_hand.insert(indexes[3], tile2)
                    
                    
        maxNumberOfMelds = maxNumberOfMelds + len(player.tiles_displayed)
        
        return maxNumberOfMelds
            
    def IsWin(self, player): #Input must include the 14th tile
        if SimpleExtractor.Is_13_Orphans(self, player):
            return True
    
        tiles_hand = player.tiles_hand
        if not len(tiles_hand) % 3 == 2:
            return False
        for i in range(len(tiles_hand) - 1):
            if i < len(tiles_hand) - 2:
                if tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer and not tiles_hand[i].tile_integer == tiles_hand[i + 2].tile_integer:
                    tile = tiles_hand.pop(i)
                    tiles_hand.pop(i)
                    
                    sum = 0
                    for eachTile in tiles_hand:
                        sum = sum + eachTile.tile_integer
                    if not sum % 3 == 0:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        continue
                        
                    if SimpleExtractor.GetNumberOfMelds(self, player) == 4:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        return True
                    else:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
            else:
                if tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer:
                    tile = tiles_hand.pop(i)
                    tiles_hand.pop(i)
                    
                    sum = 0
                    for eachTile in tiles_hand:
                        sum = sum + eachTile.tile_integer
                    if not sum % 3 == 0:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        continue
                        
                    if SimpleExtractor.GetNumberOfMelds(self, player) == 4:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        return True
                    else:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
        
        return False
        
        
    def GetNumberOfMeldsWithEyes(self, player):
        tiles_hand = player.tiles_hand
        
        allSingle = True
        for tile in tiles_hand:
            if not SimpleExtractor.IsSingle(self, player, tile):
                allSingle = False
                break
        if allSingle:
            return 0
        
        max = 0
        for i in range(len(tiles_hand) - 1):
            if i < len(tiles_hand) - 2:
                if tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer and not tiles_hand[i].tile_integer == tiles_hand[i + 2].tile_integer:
                    tile = tiles_hand.pop(i)
                    tiles_hand.pop(i)
                    
                    sum = 0
                    for eachTile in tiles_hand:
                        sum = sum + eachTile.tile_integer
                        
                    if SimpleExtractor.GetNumberOfMelds(self, player) == 4:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 4:
                            max = 4
                    elif SimpleExtractor.GetNumberOfMelds(self, player) == 3:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 3:
                            max = 3
                    elif SimpleExtractor.GetNumberOfMelds(self, player) == 2:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 2:
                            max = 2
                    elif SimpleExtractor.GetNumberOfMelds(self, player) == 1:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 1:
                            max = 1
                    else:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
            else:
                if tiles_hand[i].tile_integer == tiles_hand[i + 1].tile_integer:
                    tile = tiles_hand.pop(i)
                    tiles_hand.pop(i)
                    
                    sum = 0
                    for eachTile in tiles_hand:
                        sum = sum + eachTile.tile_integer
                    if not sum % 3 == 0:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        continue
                        
                    if SimpleExtractor.GetNumberOfMelds(self, player) == 4:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 4:
                            max = 4
                    elif SimpleExtractor.GetNumberOfMelds(self, player) == 3:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 3:
                            max = 3
                    elif SimpleExtractor.GetNumberOfMelds(self, player) == 2:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 2:
                            max = 2
                    elif SimpleExtractor.GetNumberOfMelds(self, player) == 1:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
                        if max < 1:
                            max = 1
                    else:
                        tiles_hand.insert(i, tile)
                        tiles_hand.insert(i, tile)
        
        return max
        
        
    def GetWinScore(self, player):
        tiles_displayed = player.tiles_displayed
        tiles_hand = player.tiles_hand
        
        score = 0
        maxscore = 20
        
        if SimpleExtractor.IsWin(self, player):
            tiles_13Or14 = player.GetTiles_13Or14()
            hasDot = False
            hasBamboo = False
            hasCharacter = False
            hasHonor = False
            for tile in tiles_13Or14:
                if tile.tile_pattern == 0:
                    hasDot = True
                if tile.tile_pattern == 1:
                    hasBamboo = True
                if tile.tile_pattern == 2:
                    hasCharacter = True
                if tile.tile_pattern == 3:
                    hasHonor = True
                    
            if hasDot and not hasBamboo and not hasCharacter and not hasHonor:
                score = score + 7
            if not hasDot and hasBamboo and not hasCharacter and not hasHonor:
                score = score + 7
            if not hasDot and not hasBamboo and hasCharacter and not hasHonor:
                score = score + 7
            if not hasDot and not hasBamboo and not hasCharacter and hasHonor:
                score = maxscore
            if hasDot and not hasBamboo and not hasCharacter and hasHonor:
                score = score + 3
            if not hasDot and hasBamboo and not hasCharacter and hasHonor:
                score = score + 3
            if not hasDot and not hasBamboo and hasCharacter and hasHonor:
                score = score + 3
                
            if SimpleExtractor.GetNumberOfTriplets(self, player) == 4:
                score = score + 3
        
        return score
        
    def Is10Score(self, player):
        if SimpleExtractor.GetWinScore(self, player) == 10:
            return 1
        else:
            return 0
    def Is8Score(self, player):
        if SimpleExtractor.GetWinScore(self, player) == 8:
            return 1
        else:
            return 0
            
    def Is7Score(self, player):
        if SimpleExtractor.GetWinScore(self, player) == 7:
            return 1
        else:
            return 0
    def Is6Score(self, player):
        if SimpleExtractor.GetWinScore(self, player) == 6:
            return 1
        else:
            return 0
    def Is3Score(self, player):
        if SimpleExtractor.GetWinScore(self, player) == 3:
            return 1
        else:
            return 0
        
    def GetNumberOfOrphans(self, player): #only in tiles_hand
        tiles_unique = list(set(player.tiles_hand))
        count = 0
        
        for orphan in ["00", "08", "10", "18", "20", "28", "30",
        "31", "32", "33", "34", "35", "36"]:
            for tile in tiles_unique:
                if tile.tile_code == orphan:
                    count = count + 1
                    break
        
        return count
    
    def Is_13_Orphans(self, player):
        if not len(player.tiles_hand) == 14:
            return False
        for tile in player.tiles_hand:
            if tile.tile_code not in ["00", "08", "10", "18", "20", "28", "30",
            "31", "32", "33", "34", "35", "36"]:
                return False
                
        if SimpleExtractor.GetNumberOfOrphans(self, player) == 13:
            return True
        else:
            return False
