

class ConvertSide(object):
    # only convert table to client side
    client_only = 1 << 0
    # only convert table to server side
    server_only = 1 << 1
    # convert table to both client and server side
    client_server = client_only | server_only
