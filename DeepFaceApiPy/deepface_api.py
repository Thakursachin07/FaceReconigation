from flask import Flask, request, jsonify
from deepface import DeepFace
import cv2
import numpy as np
import os

app = Flask(__name__)
REFERENCE_IMAGE = "reference.jpg"

@app.route('/upload', methods=['POST'])
def upload():
    file = request.files.get('image')
    if not file:
        return jsonify({"error": "No image provided"}), 400
    file.save(REFERENCE_IMAGE)
    return jsonify({"message": "Reference image uploaded"}), 200

@app.route('/verify', methods=['POST'])
def verify():
    if not os.path.exists(REFERENCE_IMAGE):
        return jsonify({"error": "Reference image missing"}), 400
    file = request.files.get('liveImage')
    if not file:
        return jsonify({"error": "No live image provided"}), 400

    ref_img = cv2.imread(REFERENCE_IMAGE)
    live_img = cv2.imdecode(np.frombuffer(file.read(), np.uint8), cv2.IMREAD_COLOR)

    # Resize to 224x224 to avoid memory issues
    ref_img = cv2.resize(ref_img, (224, 224))
    live_img = cv2.resize(live_img, (224, 224))

    try:
        # Only DeepFace.verify, no need to import Facenet or loadModel manually
        result = DeepFace.verify(ref_img, live_img, model_name='Facenet', enforce_detection=False)
        match_percent = round((1 - result['distance']) * 100, 2)
        
        print(match_percent)

        return jsonify({
            "name": "Candidate",
            "matchPercentage":  match_percent if match_percent > 0 else 0,
            "verified": result['verified']
            
        })
    except Exception as e:
        print(f"Error in verification: {e}")
        return jsonify({"error": "Verification failed", "details": str(e)}), 500

if __name__ == '__main__':
    app.run(host='localhost', port=5500)
