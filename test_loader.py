import socket
import threading

def process_data(data):
    # Process the data received from Unity
    result = data.upper()  # For example, convert to uppercase

    # Send the result back to Unity
    conn.sendall(result.encode())

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
