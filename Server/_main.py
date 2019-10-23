import _pyserver, udp_server, threading, time

#servidor tcp
def t_thread():
    # Crear servidor
    # Argumentos default (max_connections = 2, max_connections_wait = 4, buffer_size = 1024)
    _pyserver.createNewServer('localhost', 13000, 5)

# servidor udp
def u_thread():
    udp_server.init_video()

def main():
    # time.sleep(10)
    hilo1 = threading.Thread(target=t_thread)
    hilo1.start()
    hilo2 = threading.Thread(target=u_thread)
    hilo2.start()
    # result = commands.getoutput('udp_server.py')
    # print(re)

main()