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

4. Build the Angular application in `./FileStorageAngular`

```sh
ng build
```

5. .NET Core build then restore in `./WebAPI`

```sh
dotnet build
dotnet restore
```

6. Finally, run the server in `./WebAPI`

```sh
dotnet run
```

The angular app will be served at `http://localhost:4200`
While the c# server will be served at `http://localhost:63659/`
