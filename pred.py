from tensorflow.keras import backend as K
from tensorflow.keras.preprocessing import image
import tensorflow as tf
import numpy as np
import os
import pandas as pd
import argparse
import json
import random

os.environ['TF_FORCE_GPU_ALLOW_GROWTH'] = 'true'

parser = argparse.ArgumentParser()
parser.add_argument('--path', metavar='p', type=str, 
                     help='path to images')


args = parser.parse_args()
#print(args.path)


W, H, D = 224, 224, 3
def euclidean_distance(vects):
    x, y = vects
    sum_square = K.sum(K.square(x - y), axis=1, keepdims=True)
    return K.sqrt(K.maximum(sum_square, K.epsilon()))

model = tf.keras.models.load_model('resnet50.h5')

df = pd.read_csv('whale_encodings.csv')
encodings = [' '.join(enc.split()) for enc in list(df['encodings'])]
for i in range(len(encodings)):
    encodings[i] = encodings[i].replace('[', '')
    encodings[i] = encodings[i].replace(']', '')
    
for i in range(len(encodings)):
    encodings[i] = encodings[i].split()[:]
    for j in range(256):
        encodings[i][j] = float(encodings[i][j])


# path_to_images = r'F:\hac\new2'
path_to_images = args.path
names = os.listdir(path_to_images)

for name in names:
    img = image.load_img(os.path.join(path_to_images, name), target_size=(W,H,D))
    img_tensor = image.img_to_array(img)
    img_tensor = np.expand_dims(img_tensor, axis=0)
    img_tensor /= 255.
    pred = model.predict(img_tensor)
    dist = []
    for j in range(len(encodings)):
        dist.append(euclidean_distance([pred, encodings[j]]).numpy()[0][0])
    index = dist.index(min(dist))

    id_ = df['ids'].iloc[index]
    id_ = id_.split('\\')[0]

    print(id_)