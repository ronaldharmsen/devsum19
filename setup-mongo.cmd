REM Creating mongodata volume on Docker host and storing DB/state there
docker volume create --name=mongodata
docker run --name mongodb -v mongodata:/data/db -d -p 27017:27017 mongo

docker exec -it mongodb bash

echo "execute in container:"
echo "mongo"
echo "use webshop"
echo "db.createUser({user:"myuser", pwd:"secret", roles:[{role:"readWrite", db: "webshop"}]});"

echo "docker stop & docker rm mongodb & restart with --auth option"
echo "then the system is ready"