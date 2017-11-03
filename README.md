# Site Response Checker

A site response checker that sends notifications when a site content changes (whole page or just specific contents with css selector query like jQuey). Periodically sends requests to the sites and checks if the response has changed.

# Architecture
Multi-layered, separation between domain, business and client-side logic. Job execution is multithreaded to avoid blocking while processing site checks.
This architecture allows to add multiple clients implementations (Web, Windows GUI, etc...)

### Prerequisites
.Net Framework 4.0 (https://www.microsoft.com/pt-br/download/details.aspx?id=17851)

## Deployment
You can add the GUI .exe to Windows's start-up folder (for windows 8/10 type WindowsKey+R and then type shell:startup) or choose to install the Windows Service project (experimental).

## Built With
Built with C# 4.0, NHibernate, WPF for the GUI, SQLite for the embedded database (but that can be changed to any database that NHibernate supports)

## Authors

* **Ivan Monteiro Cantalice**