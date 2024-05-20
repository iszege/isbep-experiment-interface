import cv2
import mediapipe as mp
import sys

# Initialize MediaPipe hands module
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(static_image_mode=False, max_num_hands=1, min_detection_confidence=0.7)
mp_drawing = mp.solutions.drawing_utils

# Function to detect thumbs-up or thumbs-down gesture
def detect_gesture(hand_landmarks):
    # Get coordinates of landmarks
    thumb_tip = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_TIP]
    thumb_ip = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_IP]
    thumb_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_MCP]
    thumb_cmc = hand_landmarks.landmark[mp_hands.HandLandmark.THUMB_CMC]
    
    index_tip = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_TIP]
    index_dip = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_DIP]
    index_pip = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_PIP]
    index_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.INDEX_FINGER_MCP]
    
    middle_tip = hand_landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_TIP]
    middle_dip = hand_landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_DIP]
    middle_pip = hand_landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_PIP]
    middle_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.MIDDLE_FINGER_MCP]
    
    ring_tip = hand_landmarks.landmark[mp_hands.HandLandmark.RING_FINGER_TIP]
    ring_dip = hand_landmarks.landmark[mp_hands.HandLandmark.RING_FINGER_DIP]
    ring_pip = hand_landmarks.landmark[mp_hands.HandLandmark.RING_FINGER_PIP]
    ring_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.RING_FINGER_MCP]
    
    pinky_tip = hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_TIP]
    pinky_dip = hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_DIP]
    pinky_pip = hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_PIP]
    pinky_mcp = hand_landmarks.landmark[mp_hands.HandLandmark.PINKY_MCP]
    
    wrist = hand_landmarks.landmark[mp_hands.HandLandmark.WRIST]
    
    # Check if thumb is extended upwards
    thumb_up = (thumb_tip.y < thumb_ip.y < thumb_mcp.y < thumb_cmc.y)
    
    # Check if other fingers are not extended (so curled)
    index_finger_folded = index_tip.y > index_pip.y
    middle_finger_folded = middle_tip.y > middle_pip.y
    ring_finger_folded = ring_tip.y > ring_pip.y
    pinky_finger_folded = pinky_tip.y > pinky_pip.y
    
    # Detect thumbs-up gesture
    if thumb_up and index_finger_folded and middle_finger_folded and ring_finger_folded and pinky_finger_folded:
        return "Thumbs Up"
    
    # Check if thumb is extended downwards
    thumb_down = (thumb_tip.y > thumb_ip.y > thumb_mcp.y > thumb_cmc.y)
    
    # Detect thumbs-down gesture
    if thumb_down and index_finger_folded and middle_finger_folded and ring_finger_folded and pinky_finger_folded:
        return "Thumbs Down"
    
    # If neither thumbs-up nor thumbs-down is registered
    return "Unknown Gesture"

# Initialize webcam
cap = cv2.VideoCapture(0)

while cap.isOpened():
    success, image = cap.read()
    if not success:
        break
    
    # Convert the BGR image to RGB.
    image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
    
    # Process the image and find hands
    results = hands.process(image)
    
    # Detect and report the gesture
    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            # Detect the gesture
            gesture = detect_gesture(hand_landmarks)
            
            # Report the gesture
            print(gesture)
            sys.stdout.flush()
            
    if cv2.waitKey(5) & 0xFF == 27:
        break

cap.release()
cv2.destroyAllWindows()
