dotnet publish -c Debug -o bin/PublishOutput
docker rm -f $(docker ps -qa)
docker-compose up --force-recreate --build