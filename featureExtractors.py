import util

class FeatureExtractor:
    def getFeatures(self, state):
        """
          Returns a dict from features to counts
          Usually, the count will just be 1.0 for
          indicator functions.
        """
        util.raiseNotDefined()


class SimpleExtractor(FeatureExtractor):
    """
    Returns simple features for a basic reflex Pacman:
    - whether food will be eaten
    - how far away the next food is
    - whether a ghost collision is imminent
    - whether a ghost is one step away
    """
    def getFeatures(self, state):
        features = util.Counter()
        
        tiles_13Or14 = state.ownPlayer.GetTiles_13Or14()
        
        
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

        return features

