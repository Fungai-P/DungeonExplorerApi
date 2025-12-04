# DungeonExplorerApi

## Solution Structure
The backend solution is in the server folder and the frontend is in the client folder.

## The backend
### Running
Run the backend from Visual Studio. The backend requires .NET 9.0. The database used is SQLLite so no need for docker images to run dependancies. The database and migrations are all in the project.

### Architecture
The service has 3 layers, the API layer (requests and validations), the Business logic layer (handlers) and the data layer (the repository).

### Tests
There is a tests project. All endpoints have been tested and thoroughly.

## The Frontend
### Running
This is a basic angular app. Run it via the command 'ng serve --proxy-config proxy.conf.json'. This will allow you to create a map and generate maps path. The results of the generate map path is currently hardcoded. There is no computation so you will always see thesame result. The olution does however, demonstrate the frontend and backend integration and the presentation of the generated map path.

### Running via Docker
```
docker build -t dungeon-frontend .
docker run --rm -p 80:80 dungeon-frontend
```

The app will run on http://localhost
