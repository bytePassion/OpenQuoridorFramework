# OpenQuoridorFramework (2.2)

[![Join the chat at https://gitter.im/bytePassion/OpenQuoridorFramework](https://badges.gitter.im/bytePassion/OpenQuoridorFramework.svg)](https://gitter.im/bytePassion/OpenQuoridorFramework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![view and contribute to current tasks on trello](https://img.shields.io/badge/tasks-on%20trello-blue.svg)](https://trello.com/b/X9gnlWEl/openquoridorframework)
[![get the latest code from github](https://img.shields.io/badge/code-on%20github-lightgrey.svg)](https://github.com/bytePassion/OpenQuoridorFramework.git)
[![view licence from original source](https://img.shields.io/badge/licence-Apache%202.0-orange.svg)](http://www.apache.org/licenses/LICENSE-2.0)

OpenQuoridorFramework is a set of libraries and applications to create bots for Quoridor, play against them and do a lot more cool stuff around Quoridor.

In this Version the OQF comes with two applications:
- PlayerVsBot:  An application to play (as human) against an OQF-Bot
- ReplayViewer: An application to analize (review) a past game

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
- QuoridorNetworkPlay: An application for two humans to play against over a LAN-connection
- QuoridorWebPlay: A web-solution to play Quoridor

---

Latest Release ChangeLog Entry:

[November 25th, 2016]
 - PlayerVsBot (1.1)
   - Several minor bug fixes
   - Several Ui-Improvements
   - Progress can be viewed and copied as compressed-progress-string
   - Game can be started from a comressed-progress-string as well as from a progress-file
   - added new commandLineParamter to start a game from a compressed-progress-string
- ReplayViewer (1.1)
   - Several minor bug fixes
   - Several Ui-Improvements, including localization (de & en)
   - replay can be loaded from compressed string

---
The boardgame Quoridor was created by Mirko Marchesi and is published by [Gigamic](http://en.gigamic.com/).