# Testing RESTful Web Services 1/2 Day Workshop Code & Resources
## Prerequisites
For this workshop, you need the following knowledge:
* 101 level knowledge of C#
* 101 level Git knowledge (to manage with the repo and branches)

This workshop requires the following:
* Laptop with Windows (May work with Visual Studio for Mac, Postman for Mac etc but hasn't been tested)
* Visual Studio Community (or better) 2017 version 15.7.3 (https://www.visualstudio.com/downloads/)
* .NET Core SDK 2.1.300 (https://www.microsoft.com/net/download/visual-studio-sdks)
* Postman (https://www.getpostman.com/apps)
* Fork this repo & clone locally

## Resources
Some resources mentioned or useful during the workshop:
* Random string generator (https://goo.gl/sQ9Zej)
* http://unicodesnowmanforyou.com/
* Which Tests Should We Automate - Angie Jones (https://www.youtube.com/watch?v=VL-_pnICmGY)
* Chrome Dev Tools Network Reference
 (https://developers.google.com/web/tools/chrome-devtools/network-performance/reference)
* Danny Dainton's excellent Postman resources (https://github.com/DannyDainton/All-Things-Postman)


## To Use This Application In Docker
The application can also be used in Docker, for instance for the Workshop "Have Some Cake With Your Frosting"
* Install and setup Docker (https://www.docker.com/community-edition)
* If using Docker on Windows, set Docker to use Linux Containers - either during installation or by right-clicking on the Docker icon in the system tray and choosing Switch to Linux Containers (you can switch back after the workshop)
* Verify your installation (https://docs.docker.com/get-started/#test-docker-installation)
* Run the following command from a command line anywhere on your computer
`docker run -p 8080:80 --name myapp g33klady/todoapi:latest`
This will get the latest docker image with this code running on your machine

To see what is running in Docker
`Docker ps`

To stop a Docker image
`Docker stop myapp`

To start a Docker image back up
`Docker start myapp`

Once Docker is running, application can be accessed:
http://localhost:8080

### Note
You'll need to provide a header with key "CanAccess" and value "true" to use the API via Postman. When using the Swagger specification, select the Authorization button and enter the value "true".
