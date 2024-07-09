import socket

def start_tcp_server(host='0.0.0.0', port=12345):
    # Create a socket object
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # Bind the socket to the host and port
    server_socket.bind((host, port))

    # Listen for incoming connections
    server_socket.listen(1)
    print(f"Server listening on {host}:{port}")

    # Accept a connection
    conn, addr = server_socket.accept()
    print(f"Connected by {addr}")

    try:
        while True:
            # Receive data from the client
            data = conn.recv(1024)
            if not data:
                break

            # Decode the received bytes to a string
            data_str = data.decode('utf-8')

            # Process the point cloud data
            point_cloud = process_point_cloud_data(data_str)

            # Print or store the point cloud data
            print(point_cloud)

    finally:
        # Close the connection
        conn.close()

def process_point_cloud_data(data_str):
    # Split the data string into individual points
    points = data_str.strip().split('|')

    # Convert the points to a list of tuples (x, y, z)
    point_cloud = []
    for point in points:
        if point:
            try:
                x, y, z = map(float, point.split(','))
                point_cloud.append((x, y, z))
            except ValueError as e:
                print(f"Skipping invalid point '{point}': {e}")
    
    return point_cloud

if __name__ == "__main__":
    start_tcp_server()
