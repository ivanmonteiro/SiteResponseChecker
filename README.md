# Site Response Checker

Windows app that sends notifications when a site content changes (whole page or just specific contents with css selector query like jQuey). 

# Architecture
Uses the SharpArch project libraries to provide a multi-layered architectre focused on code reusability. Job execution is multithreaded to avoid blocking while processing site checks.

### Prerequisites
.Net Framework 4.0 (https://www.microsoft.com/pt-br/download/details.aspx?id=17851)

## Deployment
You can add the GUI .exe to Windows's start-up folder (for windows 8/10 type WindowsKey+R and then type shell:startup) or choose to install the Windows Service project (experimental).

## Built With
Built with C# 4.0, NHibernate, WPF for the GUI, SQLite for the embedded database (but that can be changed to any database that NHibernate supports).

## Authors

* **Ivan Monteiro Cantalice**