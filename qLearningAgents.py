from featureExtractors import *
import util

class QLearningAgent():
    """
      Q-Learning Agent

      Functions you should fill in:
        - computeValueFromQValues
        - computeActionFromQValues
        - getQValue
        - getAction
        - update

      Instance variables you have access to
        - self.epsilon (exploration prob)
        - self.alpha (learning rate)
        - self.discount (discount rate)

      Functions you should use
        - self.getLegalActions(state)
          which returns legal actions for a state
    """
    def __init__(self, **args):
        self.qvalues = {}

    def getQValue(self, state):
        """
          Returns Q(state,action)
          Should return 0.0 if we have never seen a state
          or the Q node value otherwise
        """
        "*** YOUR CODE HERE ***"
        if state in self.qvalues:
            return self.qvalues[state]
        else:
            return 0.0


    def computeValueFromQValues(self, state):
        """
          Returns max_action Q(state,action)
          where the max is over legal actions.  Note that if
          there are no legal actions, which is the case at the
          terminal state, you should return a value of 0.0.
        """
        "*** YOUR CODE HERE ***"
        if len(self.getLegalActions(state)) == 0:
            return 0
            
        maxValue = None
        for action in self.getLegalActions(state):
            qValue = self.getQValue(state)
            if maxValue == None or qValue > maxValue:
                maxValue = qValue
                
        return maxValue
            

    def computeActionFromQValues(self, state):
        """
          Compute the best action to take in a state.  Note that if there
          are no legal actions, which is the case at the terminal state,
          you should return None.
        """
        "*** YOUR CODE HERE ***"
        maxValue = None
        policy = None
        for action in self.getLegalActions(state):
            qValue = self.getQValue(state, action)
            if maxValue == None or qValue > maxValue:
                maxValue = qValue
                policy = action
                
        return policy

    def getAction(self, state):
        """
          Compute the action to take in the current state.  With
          probability self.epsilon, we should take a random action and
          take the best policy action otherwise.  Note that if there are
          no legal actions, which is the case at the terminal state, you
          should choose None as the action.

          HINT: You might want to use util.flipCoin(prob)
          HINT: To pick randomly from a list, use random.choice(list)
        """
        # Pick Action
        legalActions = self.getLegalActions(state)
        action = None
        "*** YOUR CODE HERE ***"
        if util.flipCoin(self.epsilon):
            action = random.choice(legalActions)
        else:
            action = self.computeActionFromQValues(state)

        return action


    def update(self, state, action, nextState, reward):
        """
          The parent class calls this to observe a
          state = action => nextState and reward transition.
          You should do your Q-Value update here

          NOTE: You should never call this function,
          it will be called on your behalf
        """
        "*** YOUR CODE HERE ***"
        if (state, action) not in self.qvalues:
            self.qvalues[state] = 0.0
        
        nextStateValue = self.computeValueFromQValues(nextState)
        currentStateValue = self.qvalues[state]

        self.qvalues[state] = (1 - self.alpha) * currentStateValue + self.alpha * (reward + self.discount * nextStateValue)

    def getPolicy(self, state):
        return self.computeActionFromQValues(state)

    def getValue(self, state):
        return self.computeValueFromQValues(state)


class MahjongQAgent(QLearningAgent):
    "Exactly the same as QLearningAgent, but with different default parameters"

    def __init__(self, epsilon=0.05,gamma=0.8,alpha=0.2, numTraining=0, **args):
        """
        These default parameters can be changed from the pacman.py command line.
        For example, to change the exploration rate, try:
            python pacman.py -p PacmanQLearningAgent -a epsilon=0.1

        alpha    - learning rate
        epsilon  - exploration rate
        gamma    - discount factor
        numTraining - number of training episodes, i.e. no learning after these many episodes
        """
        args['epsilon'] = epsilon
        args['gamma'] = gamma
        args['alpha'] = alpha
        args['numTraining'] = numTraining
        self.index = 0  # This is always Player 1
        QLearningAgent.__init__(self, **args)

    def getAction(self, state):
        """
        Simply calls the getAction method of QLearningAgent and then
        informs parent of action for Pacman.  Do not change or remove this
        method.
        """
        action = QLearningAgent.getAction(self,state)
        self.doAction(state,action)
        return action



class ApproximateQAgent(MahjongQAgent):
    """
       ApproximateQLearningAgent

       You should only have to overwrite getQValue
       and update.  All other QLearningAgent functions
       should work as is.
    """
    def __init__(self, **args):
        self.features = util.Counter()
        self.weights = util.Counter()
        
        self.weights["length of longest dishonor suit"] = 20
        self.weights["length of second and third longest dishonor suit"] = -35
        self.weights["length of honor suit"] = -18
        self.weights["is at least 11 honors"] = 500
        self.weights["number of triplets"] = 25
        self.weights["number of duplets"] = 10
        self.weights["number of sequences"] = 25
        self.weights["number of good half sequences"] = 5
        self.weights["number of melds"] = 400
        self.weights["number of melds with eyes"] = 200
        self.weights["number of orphans"] = -1
        self.weights["is 13 orphans"] = 2500
        self.weights["is win"] = 1000
        self.weights["is 10 score"] = 6400
        self.weights["is 8 score"] = 3200
        self.weights["is 7 score"] = 2400
        self.weights["is 6 score"] = 1600
        self.weights["is 3 score"] = 400
        self.weights["remaining pool count"] = 1
        
        self.alpha = 0.05
        self.discount = 0.98

    def getWeights(self):
        return self.weights

    def getQValue(self, state): #getQValue(self, state, action)
        """
          Should return Q(state,action) = w * featureVector
          where * is the dotProduct operator
        """
        "*** YOUR CODE HERE ***"
        features = SimpleExtractor.getFeatures(SimpleExtractor, state)
        return sum([self.weights[feature] * features[feature] for feature in features])

    def update(self, state, nextState, reward): #update(self, state, action, nextState, reward):
        """
           Should update your weights based on transition
        """
        "*** YOUR CODE HERE ***"
        features = SimpleExtractor.getFeatures(SimpleExtractor, state)
        # difference = reward + self.discount * self.getValue(nextState) - self.getQValue(state)
        difference = reward + self.discount * self.getQValue(nextState) - self.getQValue(state)
        for feature in features:
            self.weights[feature] = self.weights[feature] + self.alpha * difference * features[feature]

'''
    def final(self, state):
        "Called at the end of each game."
        # call the super-class final method
        PacmanQAgent.final(self, state)

        # did we finish training?
        if self.episodesSoFar == self.numTraining:
            # you might want to print your weights here for debugging
            "*** YOUR CODE HERE ***"
            pass
'''
