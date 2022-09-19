# Chickenshock

In this game, you have to shoot the chicken displaying the correct answer.  
![Chickenshock](https://github.com/Gamify-IT/docs/tree/main/images/chickenshock.webp)

Unity Version: 2021.3.2f1 (LTS)

## Disclaimer

This project is developed as part of a student project at Universität Stuttgart.
It may contain bugs, and is not licensed for external use.

## Development

### Getting started

Install the [Unity Version 2021.3.2f1 (LTS)](https://gamifyit-docs.readthedocs.io/en/latest/dev-manuals/languages/unity/version.html)

Clone the repository  
```sh
git clone https://github.com/Gamify-IT/chickenshock.git
```

Game specific properties that are likely to be changed are stored in a `.properties` file located at `Assets/Scripts/Properties/Chickenshock.properties`

### Run the project

Check the [manual for docker-compose](https://github.com/Gamify-IT/docs/blob/main/dev-manuals/languages/docker/docker-compose.md).

### Build

Build the project like discribed in [this manual](https://gamifyit-docs.readthedocs.io/en/latest/dev-manuals/languages/unity/build-unity-project.html).

Build the Docker-Container
```sh
docker build -t chickenshock-dev
```
And run it at port 8000 with
```sh
docker run -d -p 8000:80 --name chickenshock-dev chickenshock-dev
```

To monitor, stop and remove the container you can use the following commands:
```sh
docker ps -a -f name=chickenshock-dev
```
```sh
docker stop chickenshock-dev
```
```sh
docker rm chickenshock-dev
```

## User manual

Run the docker container with the following command at port 8000:
```sh
docker run -d -p 8000:80 --name chickenshock ghcr.io/gamify-it/chickenshock:latest
```
Now you can access it at [http://localhost:8000](http://localhost:8000).  
To access it externally replace localhost with your IP.  

To monitor the container you can use the following command:
```sh
docker ps -a -f name=chickenshock
```
To stop the container you can use the following command:
```sh
docker stop chickenshock
```
To remove the container you can use the following command:
```sh
docker rm chickenshock
```
### Features

- Questions are displayed at the top of the screen
- Chickens run around with signs
- Signs contain possible answers
- Chickens can be shot
- Mouse can be unlocked by pressing `p`
- Points are credited for shooting the chicken with the correct answer
- Points are deducted when shooting a chicken with the wrong answer
- Score in one corner of the screen
- After fixed time or fixed number of answered questions the game ends

### Screenshot

![screenshot](https://user-images.githubusercontent.com/44726248/171508596-16837ea1-f6ce-4b69-b12b-30b0452f917c.png)
