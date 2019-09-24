import _pyserver
# Crear servidor
# Argumentos default (max_connections = 2, max_connections_wait = 4, buffer_size = 1024)
_pyserver.createNewServer('127.0.0.1', 5000, 5)
