## WARNING: the current state of the master-branch is unstable - please use the latest release!!

![OQF_Logo](/OpenQuoridorFramework/quoridor_logo.ico)

# OpenQuoridorFramework (4.7.1)

[![Join the chat at https://gitter.im/bytePassion/OpenQuoridorFramework](https://badges.gitter.im/bytePassion/OpenQuoridorFramework.svg)](https://gitter.im/bytePassion/OpenQuoridorFramework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![view and contribute to current tasks on trello](https://img.shields.io/badge/tasks-on%20trello-blue.svg)](https://trello.com/b/X9gnlWEl/openquoridorframework)
[![get the latest code from github](https://img.shields.io/badge/code-on%20github-lightgrey.svg)](https://github.com/bytePassion/OpenQuoridorFramework.git)
[![view licence from original source](https://img.shields.io/badge/licence-Apache%202.0-orange.svg)](http://www.apache.org/licenses/LICENSE-2.0)

OpenQuoridorFramework is a set of libraries and applications to create bots for Quoridor, play against them and do a lot more cool stuff around Quoridor.

In this Version the OQF comes with five applications:
- PlayerVsBot:  An application to play (as human) against an OQF-Bot
- ReplayViewer: An application to analize (review) a past game
- Network.DesktopClient: An application to play againt each other over LAN
- Network.LanServer: This application is the junction for the Network.DesktopClient
- Tournament: This application performs a tournament with any number of OQF-Bots

---

To write a OQF-Bot you have to implement the IQuoridorBot-Inteface which can be found at OQF.Bot.Contracts. You can get more detailed information from the help-pages of PlayerVsBot.

An exemplary implementation is the SimpleWalkingBot which is also part of this version of OQF.

---

Lead-Developers:
- Matthias Drescher [[matthias.drescher@bytePassion.de](matthias.drescher@bytePassion.de)]
- Alexander Horn [[alexander.horn@bytePassion.de](alexander.horn@bytePassion.de)]

If you want to contribute to the OQF, contact a developer via E-Mail or Gitter-Chat

---

Planned applications:
- QuoridorWebPlay: A web-solution to play Quoridor

---

Usages:
- PlayerVsBot: load a OQF-Bot from a Dll-file and play against the bot
- ReplayViewer: load a progress from a past game via progress-file or compressed progress string and review the past game 
- Network:
   - first: start and activate the LanServer
   - second: start as many DesktopClients as you want in the local network, connect to the server and play against each other
- Tournament: load as many OQF-Bots as you want (with the "+"-Button) and start your tournament

---

Latest ChangeLog Entry:

[April 5th, 2017]
- PlayerVsBot (1.2)   
- ReplayViewer (1.1)
- Network.DesktopClient (1.0.1)    
- Network.LanServer (1.0)  
- Tournament (0.4 Alpha)
    - Initial Release (still alpha)

---

The boardgame Quoridor was created by Mirko Marchesi and is published by [Gigamic](http://en.gigamic.com/).

---

Developed by [bytePassion](http://www.bytePassion.de)

![bytePassion_Logo](/OpenQuoridorFramework/OQF.Resources/Images/bytePassionLogo.png)