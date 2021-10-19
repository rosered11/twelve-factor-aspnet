# twelve-factor-aspnet

## How to build image 

1. Access to root directory
2. Run this command 
```
docker build -f container/dockerfile/{location-dockerfile} -t {image-name} .
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