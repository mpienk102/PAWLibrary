from http.server import HTTPServer, SimpleHTTPRequestHandler

class CustomHandler(SimpleHTTPRequestHandler):
    def do_GET(self):
        if self.path == '/':  
            self.path = '/login.html'  
        return super().do_GET()

if __name__ == '__main__':
    port = 4000
    server_address = ('', port)
    httpd = HTTPServer(server_address, CustomHandler)
    print(f"Serving on port {port}")
    httpd.serve_forever()
