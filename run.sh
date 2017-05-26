#!/bin/bash

dotnet publish -c Debug -o bin/PublishOutput
docker-compose up --force-recreate --build