# Web API with C-Sharp Dotnet v8.0

To create the project, run the following command in a terminal.

```bash
dotnet new webapi --name Nameproject -f net8.0
```

The following instructions are for creating the services in containers, namely the Backend, Frontend, and SQL Server 2022.

First, you must have a folder with the following structure.

```bash
project
    desaweb090
    desawebback
    docker-compose.yml
    demoapps.conf
```

desaweb090 is the frontend
desawebbacke is the backend

```bash
docker compose up --build
```

Connect via sqlcmd and run this script, change for your custom password

Get container name sql with

```bash
docker ps

```

In this exampe is: diario-sqlserver-1. Next step, get network name with:

```bash
docker inspect diario-sqlserver-1
```

And finaly connect with sqlcmd, change container name and network name

```bash
docker run -it --rm --network diario_appnet mcr.microsoft.com/mssql-tools /opt/mssql-tools/bin/sqlcmd -S diario-sqlserver-1 -U sa -P 'D3saweb.2025$'
```

```sql

CREATE DATABASE desaweb;
GO

USE desaweb;
GO

CREATE LOGIN desaweb WITH PASSWORD = 'D3saweb.2025';
CREATE USER desaweb FOR LOGIN desaweb;
EXEC sp_addrolemember 'db_owner', 'desaweb';
GO
```

Now start the backend

```bash
docker compose up -d
```

Connect to container via shell

```bash
docker exec -it diario-backend /bin/bash
```
