from http.server import HTTPServer, SimpleHTTPRequestHandler

class CustomHandler(SimpleHTTPRequestHandler):
    def do_GET(self):
        if self.path == '/':  # Handle the default route
            self.path = '/login.html'  # Redirect to a specific file or path
        return super().do_GET()

if __name__ == '__main__':
    port = 2137
    server_address = ('', port)
    httpd = HTTPServer(server_address, CustomHandler)
    print(f"Serving on port {port}")
    httpd.serve_forever()
