# DOT_cardGames_2017

Client/server multiplayer Coinche game in C#, using the .NET Core framework. 

## Prerequisites

* dotnet installed

## Getting started

In a shell in the repository, enter the following command

### Building

```
dotnet build
```

### Run Server

The address will be 127.0.0.1 and the port will be 9000.

```
dotnet run --project Server 127.0.0.1 9000
```

### Run Client

The address will be 127.0.0.1 and the port will be 9000.

```
dotnet run --project Client 127.0.0.1 9000
```

### Run AI

The address will be 127.0.0.1 and the port will be 9000. 4 AI will be launched and will attempt to connect to the server.

```
dotnet run --project AI 127.0.0.1 9000 4
```

## Running the tests

```
dotnet test Test
```

## Project achieved with...

* [.NET](https://www.microsoft.com/net/learn/get-started/linuxredhat) - C# framework
* [Rider](https://www.jetbrains.com/rider/) - IDE
* [Git](https://git-scm.com/downloads) - Versioning software

## Authors

* **Erwan Simon**

## License

This project is licensed under the NALS licence (Not Any Law Skill)
