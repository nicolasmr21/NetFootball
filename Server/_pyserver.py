
import socket, sys, _thread, time, queue

from datetime import datetime

list_clients = []
queue_clients = queue.Queue(maxsize=20)
dic_clients = {}


#Player1
#Player2


def createNewServer(server_host, server_port, max_connections = 2, max_connections_wait = 4, buffer_size = 1024):

    server_socket = None

    try:

        server_socket = socket.socket(
            socket.AF_INET, socket.SOCK_STREAM
        )

        print( "[{0}] Socket creado".format(dataTime()) )

    except:

        print( "[{0}] Error al crear socket".format(dataTime()) )
        
        sys.exit(0)
    
    try:
        server_socket.bind(
            (server_host, server_port)
        )
        
        address_ip_modif = server_host
        if not server_host:
            address_ip_modif = 'all'
        
        print( "[{0}] Socket conectado a {1}:{2}.".format(dataTime(), address_ip_modif, server_port) )
        
        print( "[{0}] Maximo de clientes: {1}".format(dataTime(), str(max_connections)) )
    
    except:
        
        print( "[{0}] Ocorrio un error {1}:{2}.".format(dataTime(), server_host, server_port) )
        
        print( "[{0}] Error {1}".format(dataTime(), server_socket) )
        
        sys.exit(0)
    
    
    server_socket.listen(max_connections_wait)
    
    print( "[{0}] Esperando clientes...".format(dataTime()))

    _thread.start_new_thread(check_state, ())

    while True:
        
       #connection representa la conexion, address es una tupla que contiene la ip y el puerto por donde se comunica el cliente
       #el id es generado por el sistema para identificar al usuario
        if len(dic_clients) <= max_connections:
            #genero un id unico
            id = int(time.time())
            #obtengo el socket y la direccion ip y el puerto
            connection, address = server_socket.accept()

            #creo una tupla con los anteriores valores
            tupleC = connection, address, id
            #se agrega a la cola
            queue_clients.put(tupleC)
            #Se agrega al diccionario para buscarlo más rapidos
            dic_clients[id] = connection, address, '0'

            print( "[{0}] El Cliente se conecto - {1}:{2}, con el id {3}".format(dataTime(), address[0], address[1], id) )

            #solo crea una partida cuando hay dos jugadores
            if queue_clients.qsize() == 2:
                create_game(queue_clients.get()[2], queue_clients.get()[2], buffer_size, max_connections)

    server_socket.close()

#Metodo que retorna la fecha y hora actual del sistema
def dataTime():
    return datetime.now().strftime("%d/%m/%Y-%H:%M:%S")

#metodo que se encarga de enviar un mensaje a los jugadores en espero y verifica que jugador sale de la cola
def check_state():
    while True:
        while not queue_clients.empty():
            try:
                tupleC = queue_clients.get()
                queue_clients.put(tupleC)
                server_mensage = bytes("Wait", 'utf-8')
                tupleC[0].send(server_mensage)
            except:
                tupleC = queue_clients.get()
                string_ip_address = tupleC[1][0] + ":" + str(tupleC[1][1])
                print( "[{0}] Cliente desconectado - {1}".format(dataTime(), string_ip_address) )
                del dic_clients[tupleC[2]]
        # print(queue_clients.qsize())


#Metodo que se encarga se crear un juego entre dos conexiones
#El cual recibe los id de ambos jugadores
def create_game(client1, client2, buffer_size, max_connections):
    #identifico el jugador y envio quien es el primer jugador
    string_Msj = bytes("Player1", 'utf-8')
    dic_clients[client1][0].send(string_Msj)
    
    string_Msj = bytes("Player2", 'utf-8')
    dic_clients[client2][0].send(string_Msj)

    #inicio los hilos
    _thread.start_new_thread(client_thread, (client1, client2, buffer_size, max_connections))
    _thread.start_new_thread(client_thread, (client2, client1, buffer_size, max_connections))

#hilo por cada cliente, id1 es el afrition del hilo
def client_thread(id1, id2, buffer_size, max_connections):
    #socket del id1
    connection =  dic_clients[id1][0]
    #socket del id2
    connection2 =  dic_clients[id2][0]

    connection.send(b'Servidor')

    while True:

        try:
            time_start_receive = time.time()

            string_ip_address = dic_clients[id1][1][0] + ":" + str(dic_clients[id1][1][1])

            client_mensage = connection.recv(buffer_size)

            if not client_mensage:
                break

            time_end_receive = time.time()

            string_data = ""

            client_list = list(dic_clients[id1])

            client_list[2] = client_mensage.decode("utf-8") 

            client_tuple = tuple(client_list)

            dic_clients[id1] = client_tuple

            string_data += "{0}|{1}|{2}|{3}|{4}|".format(dic_clients[id1][1][0], str(dic_clients[id1][1][1]), str(len(dic_clients)), "{0:0.1f}ms".format( (time_end_receive - time_start_receive)), str(dic_clients[id1][2]))
            string_data += "{0}|{1}|{2}|{3}|{4}|".format(dic_clients[id2][1][0], str(dic_clients[id2][1][1]), str(len(dic_clients)), "{0:0.1f}ms".format( (time_end_receive - time_start_receive)), str(dic_clients[id2][2]))

            string_data = string_data[:- 1]
            # print(string_data)

            server_mensage = bytes(string_data, 'utf-8')

            connection2.sendall(server_mensage)

        except:
            server_mensage = bytes("NoPlayer2", 'utf-8')
            try:
                string_ip_address = dic_clients[id2][1][0] + ":" + str(dic_clients[id2][1][1])
                connection2.send(server_mensage)
                connection2.close()
                print( "[{0}] Cliente desconectado - {1}".format(dataTime(), string_ip_address) )
            except :
                try:
                    string_ip_address = dic_clients[id1][1][0] + ":" + str(dic_clients[id1][1][1])
                    connection.send(server_mensage)
                    connection.close()
                    print( "[{0}] Cliente desconectado - {1}".format(dataTime(), string_ip_address) )
                except:
                    break

            if id1 in dic_clients:
                del dic_clients[id1]
            elif id2 in dic_clients:
                del dic_clients[id2]

            break
            
