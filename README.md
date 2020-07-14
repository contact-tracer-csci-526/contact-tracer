# Contact Tracers
Ball bouncing survival game built on top of Unity written in C#

This project is initiated as a class assignment of **CSCI 526 - Advanced Mobile Devices and Game Consoles** at the University of Southern California in the summer of 2020.

[**WebGL Release**](https://contact-tracer-csci-526.github.io/contact-tracer)

[**Game Design Document**](https://docs.google.com/document/d/16KfgnSeWCqYLeWXISbLL8AeyP_CU4sXrAhU_9TFUKLw/edit?usp=sharing)

## Elevator pitch
Contact Tracers is a survival game. It involves different kinds of healthy balls that bounce in random directions within the screen. A deadly virus is capable of infecting healthy balls upon collision. Players should make sure that the healthy ball doesn’t get in contact with the deadly virus by either moving away from it or protect it by freezing a healthy ball to a safe ball. An infected ball can be cured using a cure ball, which moves in random directions and appears for every 10 seconds throughout the game. Your goal is to score enough to move to the next level.

## Game genre
Arcade / Strategy

## Goal
Survive each level by keeping the number of infected balls below the threshold until the time runs out

## Inspiration
Year 2020

## Mechanics
Draw a line between the virus and a healthy ball to move away from the virus
Draw a circle around a healthy ball to freeze it and protect from viruses.

### Cure mechanism
A Cure ball moves in random directions and appears for every 10 seconds during the game. Upon collision with an infected ball, it transforms the infected ball to a healthy one.

### Safe ball mechanism
Players can draw a circle around any healthy ball to freeze it and keep it safe from viruses. Note that only, only two balls can be frozen at a time.

### Score mechanism
At the end of each level,

- The player gets 10 points for every healthy ball and every safe ball.
- The player loses 10 points for every infected ball.

### End Condition
  - Win condition: Player scores the minimum number of points required to pass the level within the given time limit
  - Lose condition: Player fails to score the expected score within the given time limit

### Similar games
- Bloody Virus
- Fruit Ninja
- Harbor King

## Contributors
- Patrick Assaf https://www.linkedin.com/in/patrick-assaf-964029161
- Joseph Beavans https://www.linkedin.com/in/joseph-beavans
- Venkata Sai Himabindu Kandukuri https://www.linkedin.com/in/himabindu-kandukuri-a2690356/
- Gustavo Moncada https://www.linkedin.com/in/gustavo-moncada-12591463
- Elden Park https://www.linkedin.com/in/eldenpark/
- Priya Patel https://www.linkedin.com/in/priya-patel-usc/
- Bhagyashree Rawal https://www.linkedin.com/in/bhagyashreerawal/
- Rucha Tambe https://www.linkedin.com/in/rucha-tambe/
- Jungwon Yoon https://www.linkedin.com/in/foggyoon/
