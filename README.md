# `FileStorage`

## For what

An application for downloading files, sorted by day. Uniqueness is compared sha256. Files are provided only to the user who downloaded them (the domain name is taken from the server).

## Running the application

1. Download zip or git clone
2. Restore database `./FileStorDB.bak`
3. Install dependencies in `./FileStorageAngular`

```sh
npm install
```

4. .NET Core build then restore in `./WebAPI`

```sh
dotnet build
dotnet restore
```

5. Start the server in `./WebAPI`

```sh
ctrl + f5
```

6. Start the Angular application in `./FileStorageAngular`

```sh
npm start
```

The angular app will be served at `http://localhost:4200`
While the c# server will be served at `http://localhost:63659/`
