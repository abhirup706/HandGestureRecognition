# first neural network with keras tutorial
from numpy import loadtxt
import pickle
import numpy as np
import pandas as pd
from tensorflow import keras
from keras.models import Sequential
from keras.layers import Dense
from keras.utils import to_categorical
from sklearn.model_selection import train_test_split


# Load the data from the Excel file
dataset = loadtxt('training_data.csv', delimiter=',')

# Extract the input features and output label
X = dataset[:, 0:75]
y = dataset[:, 75]
y = to_categorical(y, num_classes=5)

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=0)

#y_train = to_categorical(y_train, num_classes=5)
#y_test = to_categorical(y_test, num_classes=5)

# create a sequential model
model = Sequential()

# add input layer with 75 neurons
model.add(Dense(units=75, input_dim=75, activation='relu'))

# add hidden layer with 50 neurons
model.add(Dense(units=50, activation='relu'))

# add output layer with 5 neurons
model.add(Dense(units=5, activation='softmax'))

# compile the model
model.compile(loss='categorical_crossentropy',
              optimizer='adam',
              metrics=['accuracy'])

# train the model
model.fit(X_train, y_train, epochs=50, batch_size=32, validation_data=(X_test, y_test))

# make predictions on test data
y_pred = model.predict(X_test)

# convert predictions from probabilities to class labels
y_pred_label = np.argmax(y_pred, axis=1)

# evaluate the model
_, accuracy = model.evaluate(X, y)
print('Accuracy: %.2f' % (accuracy*100))

print(y_pred_label)

filename = 'finalized_model.sav'
pickle.dump(model,open(filename,'wb'))