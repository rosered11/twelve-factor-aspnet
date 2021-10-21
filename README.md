# twelve-factor-aspnet

## How to build image 

1. Access to root directory
2. Run this command 
```
docker build -f container/dockerfile/{location-dockerfile} -t {image-name} .
```

## How to build image arch AMD64 on Arm64

1. pull image arch amd64 and run container on Amd64 *(like Rosseta on M1)*

*Example*
```
docker run -it -d --platform linux/amd64 --name my-amd mcr.microsoft.com/dotnet/sdk:5.0 bash
```
2. copy source into container amd64 

*Example*
```
docker cp {host-location} my-amd:/source/
```

3. exec into container for build or publish
```
docker exec -it my-amd bash
```
***Remark***
*In case .net, please remove folder ***bin*** or ***obj***, because it maybe has a problems when run build cross architecture.*

4. copy publish file out to host
*Example*
```
dockere cp my-amd:/out {host-location}
```

5. build image docker arch **AMD64** from publish file 

*Example*
```
docker build -f container/dockerfile/account/Dockerfile -t rosered/twelve-factor-account:amd64 --platform linux/amd64 --no-cache .
```

## Health Check Config Server

This url for check detail on config server
```http://localhost:8888/<file-name>/<profile>```

# Reference

- [Config Server with Asp.Net](https://www.youtube.com/watch?v=UXSieGmbOhg)

- [Steeltoe Config Server](https://github.com/SteeltoeOSS/Dockerfiles)

- [Eureka Server on Heroku](https://github.com/kissaten/heroku-eureka-server-demo)

- [Eureka for dotnet](https://altkomsoftware.pl/en/blog/service-discovery-eureka/)

- [Build images multi arch](https://jitsu.com/blog/multi-platform-docker-builds)

- [Setup build image m1](https://betterprogramming.pub/how-to-actually-deploy-docker-images-built-on-a-m1-macs-with-apple-silicon-a35e39318e97)