
import socket, sys, _thread, time

from datetime import datetime


list_clients = []


def createNewServer(server_host, server_port, max_connections = 2, max_connections_wait = 4, buffer_size = 1024):

    server_socket = None

    try:

        server_socket = socket.socket(
            socket.AF_INET, socket.SOCK_STREAM
        )

        print( "[{0}] Socket creado".format(datetime.fromtimestamp(int(time.time())).time()) )

    except:

        print( "[{0}] Error al crear socket".format(datetime.fromtimestamp(int(time.time())).time()) )
        
        sys.exit(0)
    
    try:
        server_socket.bind(
            (server_host, server_port)
        )
        
        address_ip_modif = server_host
        if not server_host:
            address_ip_modif = 'all'
        
        print( "[{0}] Socket conectado a {1}:{2}.".format(datetime.fromtimestamp(int(time.time())).time(), address_ip_modif, server_port) )
        
        print( "[{0}] Maximo de clientes: {1}".format(datetime.fromtimestamp(int(time.time())).time(), str(max_connections)) )
    
    except:
        
        print( "[{0}] Ocorrio um error {1}:{2}.".format(datetime.fromtimestamp(int(time.time())).time(), server_host, server_port) )
        
        print( "[{0}] Error {1}".format(datetime.fromtimestamp(int(time.time())).time(), server_socket) )
        
        sys.exit(0)
    
    
    server_socket.listen(max_connections_wait)
    
    print( "[{0}] Esperando clientes...".format(datetime.fromtimestamp(int(time.time())).time()) )
     
    while True:
       

        connection, address = server_socket.accept()
        
        print( "[{0}] Cliente se conecto - {1}:{2}".format(datetime.fromtimestamp(int(time.time())).time(), address[0], address[1]) )
        
        _thread.start_new_thread(client_thread, (connection, address, buffer_size, max_connections))
   
    server_socket.close()



def client_thread(connection, address, buffer_size, max_connections):
   
    if len(list_clients) == max_connections:
        connection.close()
        return
    
    list_clients.append( ( address[0], address[1], len(list_clients), '0' ) )

    connection.send(b'Servidor')

    while True:

        try:
            time_start_receive = time.time()

            string_ip_address = address[0] + ":" + str(address[1])

            client_mensage = connection.recv(buffer_size)

            if not client_mensage:
                break

            time_end_receive = time.time()

            string_data = ""

            for index_client in range(0, len(list_clients)):

                if list_clients[index_client][0] == address[0]:

                    if list_clients[index_client][1] == address[1]:

                        client_list = list(list_clients[index_client])

                        client_list[3] = client_mensage.decode("utf-8") 

                        client_tuple = tuple(client_list)

                        list_clients[index_client] = client_tuple

                string_data += "{0}|{1}|{2}|{3}|{4}|".format(list_clients[index_client][0], str(list_clients[index_client][1]), str(len(list_clients)), "{0:0.1f}ms".format( (time_end_receive - time_start_receive)), str(list_clients[index_client][3]))

            string_data = string_data[:- 1]
            print(string_data)

            server_mensage = bytes(string_data, 'utf-8')

            connection.sendall(server_mensage)

     

        except:
           

            print( "[{0}] Cliente desconectado - {1}".format(datetime.fromtimestamp(int(time.time())).time(), string_ip_address) )
            for client in list_clients:
                if client[0] == address[0]:
                    if client[1] == address[1]:
                        list_clients.remove(client)
            
            break
