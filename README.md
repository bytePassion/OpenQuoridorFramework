## Hinweis für den dotnetpro-Contest 01/2017

Auch wenn der Code im [offiziellen dotnetpro-GitHub-Repository](https://github.com/dotnetpro/contest012017), bzw. auf der Heft-CD die Referenz in dem Contest ist, so ist dieser Code trotzdem noch kompatibel zur aktuellen Version des OpenQuoridorFramework.
Wer also gerne mit der aktuellen Version von PlayerVsBot herumspielen mag, ist gerne dazu eingeladen. 

Trotzdem empfehlen wir vor der Abgabe den Bot auf jeden Fall nochmal mit dem offizellen Contest-Code zu testen.

Fragen zum Contest beantworten wir gerne in der Kommentar-Sektion auf der Homepage der dotnetpro, per E-Mail, oder im [Gitter-Chat](https://gitter.im/bytePassion/OpenQuoridorFramework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![OQF_Logo](/OpenQuoridorFramework/quoridor_logo.ico)

# OpenQuoridorFramework (4.2)

[![Join the chat at https://gitter.im/bytePassion/OpenQuoridorFramework](https://badges.gitter.im/bytePassion/OpenQuoridorFramework.svg)](https://gitter.im/bytePassion/OpenQuoridorFramework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![view and contribute to current tasks on trello](https://img.shields.io/badge/tasks-on%20trello-blue.svg)](https://trello.com/b/X9gnlWEl/openquoridorframework)
[![get the latest code from github](https://img.shields.io/badge/code-on%20github-lightgrey.svg)](https://github.com/bytePassion/OpenQuoridorFramework.git)
[![view licence from original source](https://img.shields.io/badge/licence-Apache%202.0-orange.svg)](http://www.apache.org/licenses/LICENSE-2.0)

OpenQuoridorFramework is a set of libraries and applications to create bots for Quoridor, play against them and do a lot more cool stuff around Quoridor.

In this Version the OQF comes with four applications:
- PlayerVsBot:  An application to play (as human) against an OQF-Bot
- ReplayViewer: An application to analize (review) a past game
- Network.DesktopClient: An application to play againt each other over LAN
- Network.LanServer: This application is the junction for the Network.DesktopClient

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
- Tournament: An application to let multiple bots  fight each other
- QuoridorWebPlay: A web-solution to play Quoridor

---

Usages:
- PlayerVsBot: load a OQF-bot from a Dll-file and play against the bot
- ReplayViewer: load a progress from a past game via progress-file or compressed progress string and review the past game 
- Network:
   - first: start and activate the LanServer
   - second: start as many DesktopClients as you want in the local network, connect to the server and play against each other

---

Latest ChangeLog Entry:

[December 8th, 2016]
 - PlayerVsBot (1.1)
 - ReplayViewer (1.1)
 - Network.DesktopClient (1.0)
   - Initial Release
 - Network.LanServer (1.0)
   - Initial Release

---

The boardgame Quoridor was created by Mirko Marchesi and is published by [Gigamic](http://en.gigamic.com/).

---

Developed by [bytePassion](http://www.bytePassion.de)

![bytePassion_Logo](/OpenQuoridorFramework/OQF.Resources/Images/bytePassionLogo.png)