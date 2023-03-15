import socket
import threading
import pickle
import numpy as np

def process_data(data):
    # Process the data received from Unity
    model = pickle.load(open('finalized_model.sav', 'rb'))
    input_data = data.split(",")
    print(input_data)
    input_data_float = [float(s.strip()) for s in input_data]
    #print(input_data_float)
    data_array = np.array([input_data_float])
 


    y_pred = model.predict(data_array)
    y_pred_label = np.argmax(y_pred, axis=1)

    # Send the result back to Unity
    res_str = str(int(y_pred_label[0]))
    #conn.sendall(res_str.encode())
    response_data = res_str.encode('ascii')
    total_sent = 0
    while total_sent < len(response_data):
        sent = conn.send(response_data[total_sent:])
        if sent == 0:
            # handle the case where the socket's send buffer is full
            break
        total_sent += sent

def handle_connection(conn, addr):
    print(f"New connection from {addr}")
    while True:
        data = conn.recv(1024)
        if not data:
            break
        process_data(data.decode())
    conn.close()

HOST = '127.0.0.1'  # Localhost IP address
PORT = 5000  # Port number for the socket server
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    print(f"Socket server listening on {HOST}:{PORT}")
    while True:
        conn, addr = s.accept()
        threading.Thread(target=handle_connection, args=(conn, addr)).start()
