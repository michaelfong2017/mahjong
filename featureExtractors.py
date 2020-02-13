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
            
        features["number of triplets"] = SimpleExtractor.GetNumberOfTriplets(self, state)

        """
        Count sequence
        ##########
        """
        features["number of sequences"] = SimpleExtractor.GetNumberOfSequences(self, state)
        
        
        """
        Count maximum number of melds
        """
        features["number of melds"] = SimpleExtractor.GetNumberOfMelds(self, state)
        
        """
        Remaining pool count
        ##########
        """
        features["remaining pool count"] = len(state.remaining_pool)

        return features
        
    def IsQuadruplet(self, state, tile):
        count = 0
        for eachTile in state.ownPlayer.tiles_hand:
            if eachTile.tile_integer == tile.tile_integer:
                count = count + 1
        if count == 4:
            return True
        else:
            return False
            
    def IsTriplet(self, state, tile):
        count = 0
        for eachTile in state.ownPlayer.tiles_hand:
            if eachTile.tile_integer == tile.tile_integer:
                count = count + 1
        if count == 3:
            return True
        else:
            return False

    def GetNumberOfTriplets(self, state): #Include quadruplets
        numberOfTriplets = 0
                
        for tile in state.ownPlayer.tiles_hand:
            if SimpleExtractor.IsQuadruplet(self, state, tile):
                numberOfTriplets = numberOfTriplets + 1
        numberOfTriplets = int(numberOfTriplets / 4)
        temp = numberOfTriplets
        numberOfTriplets = 0
                
        for tile in state.ownPlayer.tiles_hand:
            if SimpleExtractor.IsTriplet(self, state, tile):
                numberOfTriplets = numberOfTriplets + 1
        numberOfTriplets = int(numberOfTriplets / 3)
        numberOfTriplets = numberOfTriplets + temp
        
        for tiles in state.ownPlayer.tiles_displayed:
            if tiles[0].tile_integer == tiles[1].tile_integer:
                numberOfTriplets = numberOfTriplets + 1
        
        return numberOfTriplets

    def GetNumberOfSequences(self, state):
        count = 0
        tiles_hand_copy = copy.deepcopy(state.ownPlayer.tiles_hand)
        indexes = []
        
        def RemoveSequence(self, count):
            for i in range(len(tiles_hand_copy)):
                if i == 0:
                    continue
                if len(indexes) == 0:
                    if tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
                        indexes.append(i - 1)
                        indexes.append(i)
                else:
                    if tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer:
                        continue
                    elif tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
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
        
        for tiles in state.ownPlayer.tiles_displayed:
            if tiles[1].tile_integer == tiles[0].tile_integer + 1:
                count = count + 1
            
        return count
    
    
    def GetNumberOfSequencesExceptMelds(self, state):
        count = 0
        tiles_hand_copy = copy.deepcopy(state.ownPlayer.tiles_hand)
        indexes = []
        
        def RemoveSequence(self, count):
            for i in range(len(tiles_hand_copy)):
                if i == 0:
                    continue
                if len(indexes) == 0:
                    if tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
                        indexes.append(i - 1)
                        indexes.append(i)
                else:
                    if tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer:
                        continue
                    elif tiles_hand_copy[i].tile_integer == tiles_hand_copy[i - 1].tile_integer + 1:
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
        
    def GetNumberOfMelds(self, state):
        indexes = []
        resume = 0
        for i in range(len(state.ownPlayer.tiles_hand)):
            if i < resume:
                continue
            if SimpleExtractor.IsQuadruplet(self, state, state.ownPlayer.tiles_hand[i]):
                indexes.append(i)
                resume = i + 4
            elif SimpleExtractor.IsTriplet(self, state, state.ownPlayer.tiles_hand[i]):
                indexes.append(i)
                resume = i + 3
                
        numberOfTriplets = len(indexes)
        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state)
        for i in range(numberOfTriplets):
            if i == 0:
                for j in range(numberOfTriplets):
                    tile = state.ownPlayer.tiles_hand.pop(indexes[j])
                    state.ownPlayer.tiles_hand.pop(indexes[j])
                    state.ownPlayer.tiles_hand.pop(indexes[j])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 1:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 1
                    state.ownPlayer.tiles_hand.insert(indexes[j], tile)
                    state.ownPlayer.tiles_hand.insert(indexes[j], tile)
                    state.ownPlayer.tiles_hand.insert(indexes[j], tile)
            if i == 1:
                if numberOfTriplets >= 2:
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                if numberOfTriplets >= 3:
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    # # # # # # # # # #
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                if numberOfTriplets == 4:
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    # # # # # # # # # #
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    # # # # # # # # # #
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile1)
            if i == 2:
                if numberOfTriplets >= 3:
                    tile2 = state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile2)
                if numberOfTriplets == 4:
                    tile2 = state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    # # # # # # # # # #
                    tile2 = state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    state.ownPlayer.tiles_hand.pop(indexes[0])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[0], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    # # # # # # # # # #
                    tile2 = state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    state.ownPlayer.tiles_hand.pop(indexes[3])
                    tile1 = state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    state.ownPlayer.tiles_hand.pop(indexes[2])
                    tile0 = state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    state.ownPlayer.tiles_hand.pop(indexes[1])
                    if maxNumberOfMelds < SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2:
                        maxNumberOfMelds = SimpleExtractor.GetNumberOfSequencesExceptMelds(self, state) + 2
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[1], tile0)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[2], tile1)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    state.ownPlayer.tiles_hand.insert(indexes[3], tile2)
                    
                    
        maxNumberOfMelds = maxNumberOfMelds + len(state.ownPlayer.tiles_displayed)
        
        return maxNumberOfMelds
            
