# Chickenshock

![Chickenshock](https://raw.githubusercontent.com/Gamify-IT/docs/main/images/chickenshock.webp)
This repository contains the frontend for the [Chickenshock minigame](https://gamifyit-docs.readthedocs.io/en/latest/user-manuals/minigames/chickenshock.html).

## Disclaimer

This project is developed as part of a student project at Universität Stuttgart.
It may contain bugs, and is not licensed for external use.

## Table of contents

<!-- TOC -->
* [Links](#links)
* [Development](#development)
  * [Getting started](#getting-started)
  * [Run the project](#run-the-project)
  * [Build](#build)
<!-- TOC -->

## Links

- User documentation for the minigame can be found [here](https://gamifyit-docs.readthedocs.io/en/latest/user-manuals/minigames/chickenshock.html).
- For the backend, see the [Gamify-IT/chickenshock-backend repository](https://github.com/Gamify-IT/chickenshock-backend).
- The installation manual and setup instructions can be found [here](https://gamifyit-docs.readthedocs.io/en/latest/install-manuals/index.html).

## Development

Unity Version: 2021.3.2f1 (LTS)

### Getting started

Install the [Unity Version 2021.3.2f1 (LTS)](https://gamifyit-docs.readthedocs.io/en/latest/dev-manuals/languages/unity/version.html)

Clone the repository  
```sh
git clone https://github.com/Gamify-IT/chickenshock.git
```

Game specific properties that are likely to be changed are stored in a `.properties` file located at `Assets/Scripts/Properties/Chickenshock.properties`

#### Run with Docker-compose

Start all dependencies with our docker-compose files.
Check the [manual for docker-compose](https://github.com/Gamify-IT/docs/blob/main/dev-manuals/languages/docker/docker-compose.md).

To run the main branch with minimal dependencies use the `docker-compose.yaml` file.\
To run the latest changes on any other branch than `main` use the `docker-compose-dev.yaml` file.


### Build

Build the project like described in [this manual](https://gamifyit-docs.readthedocs.io/en/latest/dev-manuals/languages/unity/build-unity-project.html).

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

## Audio sources

1.	Chicken sound
https://pixabay.com/de/sound-effects/047876-chicken-clucking-68610/

2.	Click
https://pixabay.com/de/sound-effects/interface-button-154180/

3.	Wrong answer
https://pixabay.com/de/sound-effects/error-126627/

4.	Shot sound
https://pixabay.com/de/sound-effects/shotgun-firing-4-6746/

5.	Correct answer
https://pixabay.com/de/sound-effects/short-success-sound-glockenspiel-treasure-video-game-6346/

6.	Trumpets end sound
https://pixabay.com/sound-effects/success-fanfare-trumpets-6185/
