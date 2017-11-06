# Site Response Checker

Author: **Ivan Monteiro Cantalice**

App that sends notifications when a site content changes (whole page or just specific contents with css selector queries similar to jQuery's).
Has a GUI interface for Windows as well as possibility to add other clients (Web and mobile) in the future.

# Motivation

I wanted a reliable way to periodically check certain websites that I configure for changes. Also to keep track of only certain portion of the page's html, since things like comments change too often and may not be interesting to send a notification for things like that. And also send an email only when something changes stating WHAT has been added/removed.

# How It Works
Uses Fizzler for css selector engine, HtmlAgilityPack to manipulate html and DiffPlex to get detailed diff information about what has been added and removed. And a simple Windows GUI built with WPF.

### Prerequisites

.Net Framework 4.0 (https://www.microsoft.com/pt-br/download/details.aspx?id=17851)

## Deployment

Build and add the Desktop GUI's .exe to Windows's start-up folder (for windows 8/10 type WindowsKey+R and then type shell:startup).
If necessary run the database migration script (MigrateSqlite.bat for the default sqlite database and MigrateSqlServer.bat for sql server). That will automagically take care of database upgrades.

## TODO

Add a web api
Create Angular ui to replace the Windows GUI built with WPF

## Built With

-Fizzler for css selector engine (https://github.com/atifaziz/Fizzler)

-SharpArch for architecture and code reusability of some core components (http://sharparchitecture.github.io/)

-HtmlAgilityPack to manipulate html (https://github.com/zzzprojects/html-agility-pack)

-DiffPlex to get detailed diff information about what has been added and removed (https://github.com/mmanela/diffplex)

-NHibernate for ORM and FluentNhibernate for nice mapping configuration (http://nhibernate.info/)

-FluentMigrator for database migrations (https://github.com/fluentmigrator/fluentmigrator)

-WPF (Windows Presentation Foundation) for the GUI

-C# 4.0 backend