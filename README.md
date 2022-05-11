# 1. Fhir-client-net
This is a simple fhir client in c# to practice with fhir resources and CRUD requests to a fhir server.<br>
Note that for the most part auto-completion is activated, that's the main reason to use fhir.resources.


- [1. Fhir-client-net](#1-fhir-client-net)
- [2. Prerequisites](#2-prerequisites)
- [3. Installation](#3-installation)
  - [3.1. Installation for development](#31-installation-for-development)
  - [3.2. Management Portal and VSCode](#32-management-portal-and-vscode)
  - [3.3. Having the folder open inside the container](#33-having-the-folder-open-inside-the-container)
- [4. FHIR server](#4-fhir-server)
- [5. Walkthrough](#5-walkthrough)
  - [5.1. Part 1](#51-part-1)
  - [5.2. Part 2](#52-part-2)
  - [5.3. Part 3](#53-part-3)
  - [5.4. Part 4](#54-part-4)
  - [5.5. Conclusion of the walkthrough](#55-conclusion-of-the-walkthrough)
- [7. How to start coding](#7-how-to-start-coding)
- [8. What's inside the repo](#8-whats-inside-the-repo)
  - [8.1. Dockerfile](#81-dockerfile)
  - [8.2. .vscode/settings.json](#82-vscodesettingsjson)
  - [8.3. .vscode/launch.json](#83-vscodelaunchjson)

# 2. Prerequisites
Make sure you have [git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git) and [Docker desktop](https://www.docker.com/products/docker-desktop) installed.

[Hl7.Fhir.Model](https://docs.fire.ly/projects/Firely-NET-SDK/model.html)

[Hl7.Fhir.Rest](https://docs.fire.ly/projects/Firely-NET-SDK/client.html)


# 3. Installation

## 3.1. Installation for development

Clone/git pull the repo into any local directory e.g. like it is shown below:
```
$ git clone https://github.com/LucasEnard/fhir-client.git
```

Go to the right branch, the dot net branch.

Open the terminal in this directory and run:

```
$ docker build .
```
## 3.2. Management Portal and VSCode

This repository is ready for [VS Code](https://code.visualstudio.com/).

Open the locally-cloned `fhir-client` folder in VS Code.

If prompted (bottom right corner), install the recommended extensions.

## 3.3. Having the folder open inside the container
You can be *inside* the container before coding.<br>
For this, docker must be on before opening VSCode.<br>
Then, inside VSCode, when prompted (in the right bottom corner), reopen the folder inside the container so you will be able to use the python components within it.<br>
The first time you do this it may take several minutes while the container is readied.

[More information here](https://code.visualstudio.com/docs/remote/containers)

![Architecture](https://code.visualstudio.com/assets/docs/remote/containers/architecture-containers.png)

<br><br><br>

By opening the folder remote you enable VS Code and any terminals you open within it to use the python components within the container. Configure these to use `/usr/irissys/bin/irispython`

<img width="1614" alt="PythonInterpreter" src="https://user-images.githubusercontent.com/47849411/145864423-2de24aaa-036c-4beb-bda0-3a73fe15ccbd.png">

# 4. FHIR server

To complete this walktrough you will need a FHIR server.<br>
You can either use your own or go to [InterSystems free FHIR trial](https://portal.live.isccloud.io) and follow the next few steps to set it up.

Using our free trial, just create an account and start a deployement, then in the `Overview` tab you will get acces to an endpoint like `https://fhir.000000000.static-test-account.isccloud.io` that we will use later.<br>
Then, by going to the `Credentials` tab, create an api key and save it somewhere.

Now you are all done, you have you own fhir server holding up to 20GB of data with a 8GB memory.

# 5. Walkthrough
Complete walkthrough of the client situated at `/Client.cs`.<br>

The code is separated in multiple parts, and we will cover each of them below.

## 5.1. Part 1
In this part we connect our client to our server using Fhir.Rest.

In order to connect to your server you need to change the line :
```c#
httpClient.DefaultRequestHeaders.Add("x-api-key", "api-key");
```
And this line :
```c#
var client = new FhirClient("url",httpClient,settings);
```

The `'url'` is an endpoint while the `"api-key"` is the api key to access your server.

Note that if **you are not using** an InterSystems server you may want to check how to authorize your acces if needed.<br>

Just like that, we have a FHIR client capable of direct exchange with our server.

## 5.2. Part 2
In this part we create a Patient using Fhir.Model and we fill it with a HumanName, following the FHIR convention, `use` and `family` are string and `given` is a list of string. The same way, a Patient can have multiple HumanNames so we have to put our HumanName in a list before puting it into our newly created Patient.

After that, we need to save our new Patient in our server using our client.

Note that if you start `Client.cs` multiple times, multiple Patients having the name we choosed will be created.<br> This is because, following the FHIR convention you can have multiple Patient with the same name, only the `id` is unique on the server.<br>
Check [the doc](https://docs.fire.ly/projects/Firely-NET-SDK/client/search.html#searching) for more information.

Therefore we advise to comment the line after the first launch.

## 5.3. Part 3
In this part we get a client searching for a Patient named after the one we created earlier.

Once we found him, we add a phone number to his profile and we change his given name to another.

Now we can use the update function of our client to update our patient in the server.

## 5.4. Part 4
In this part we want to create an observation for our Patient from earlier, to do this we need his id, which is his unique identifier.<br>
From here we fill our observation and add as the subject, the id of our Patient.

Then, we register using the create function our observation.

## 5.5. Conclusion of the walkthrough

If you have followed this walkthrough you now know exactly what Client.cs does, you can start it using any python interpreter and check in your server your newly created Patient and Observation.


# 7. How to start coding
This repository is ready to code in VSCode with InterSystems plugins.
Open `Client.cs` to start coding or using the autocompletion.

# 8. What's inside the repo

## 8.1. Dockerfile

The simplest dockerfile to start IRIS.
Use the related docker-compose.yml to easily setup additional parametes like port number and where you map keys and host folders.

## 8.2. .vscode/settings.json

Settings file to let you immedietly code in VSCode with [VSCode ObjectScript plugin](https://marketplace.visualstudio.com/items?itemName=daimor.vscode-objectscript))

## 8.3. .vscode/launch.json
Config file if you want to debug with VSCode ObjectScript
