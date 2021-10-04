# SonicTheHedgehogProject
## Introduction
After completing The Tech Academy's C# and Unity course, I spent two weeks on a live project in which I built a level from a classic game of my chosing in Unity. I picked Sonic the Hedgehog, because I it was the first video game I played as a kid and I thought it would be a fun challenge. Over the course of the project, I worked under the supervision of my live project manager to get a working version of the first level of the game built into their classic arcade game project. I finished four user stories over the course of the week including creating my basic scenes, building the level and designing Sonic's behaviors, adding enemies and their behaviors, and creating a working gameplay model with working win and loss scenarios available to the player.

I quickly came to appreciate the complexity of Sonic the Hedgehog's character movement and the different states in which he is able to interact with enemies and the environment. I still have plenty of work to do to make this a truly faithful remake of the original game, but I am very proud of what I was able to accomplish over the course of the two week project. I am particularly happy with the finite state machine I used to create Sonic's controller as well as the scripts for several of the classic Sonic enemies. Certain aspects of the project were more difficult than others, but I enjoyed the challenge and had a lot of fun figuring out solutions to the various problems that came up. I learned a lot about Unity and its features, including its Animation capabilities, many of the available components and how to chose the right one for the situation, the cinemachine package, and it's 2D physics system. 

The project was built using Unity 2020.3.2f1. I want to thank Joseph Judge since I used many of the sprites from his "I Can't Believe it's Not Sonic 1!" project. I also want to not that this project features a scene loader built by The Tech Academy that I will eventually replace with my own to better suit the project. Other than the one script related to the scene loader, all the script in this project was written solely by me.
## The Basic Scenes
I created 3 scenes so far for this project. The Title Menu, the Green Hill Zone Act 1 scene, and an End Menu. Rather than implementing buttons to chose what to do between the scenes, I chose to use the get GetKeyDown() function for both the beginning and ending scenes with onscreen instruction for the player. 

![image](https://user-images.githubusercontent.com/87107050/135880820-388d01c1-8ab9-4f80-82ba-2bdc4ae5067b.png)
![image](https://user-images.githubusercontent.com/87107050/135881294-b2a985b4-7bc0-4f8f-a6ab-bf671bdc5dde.png)

The transition from the game scene to the End Menu occures when Sonic has lost his last life or makes it to the end of the level.

![SonicGameOver](https://user-images.githubusercontent.com/87107050/135884539-2919abf4-08f0-4e70-bbcf-3a06354e8c31.gif)
![SonicLevelEnd](https://user-images.githubusercontent.com/87107050/135885684-5e1a7afa-d9fc-4677-909f-bdb05403fddf.gif)

I will be adding more images and animations to the Title Menu and End Menu as I continue working on this project.
