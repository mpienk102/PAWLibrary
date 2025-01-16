#!/bin/bash

# Function to stop processes
stop_processes() {
  echo "Stopping API..."
  kill $API_PID

  echo "Stopping Python server..."
  kill $PYTHON_PID

  echo "Stopping Docker container..."
  docker stop library_db
}

# Trap Ctrl+C (SIGINT) and call stop_processes function
trap stop_processes SIGINT

# Run the API
echo "Running API"
cd ./LibraryApi
dotnet run & 
API_PID=$!
cd ..

# Start the Docker container
echo "Running docker container"
docker start library_db

# Run the Python HTTP server
echo "Running python http server"
cd ./Page
python3 custom_server.py &
PYTHON_PID=$!
cd ..


wait $API_PID
wait $PYTHON_PID
