# Site Response Checker

Windows app that sends notifications when a site content changes (whole page or just specific contents with css selector like jQuery). 

## Author

* **Ivan Monteiro Cantalice**

# Motivation

I wanted a reliable way to check certain websites for changes, and keep track of only a small portion of that page since many things change periodically, and send an email only when something changes.

### Prerequisites

.Net Framework 4.0 (https://www.microsoft.com/pt-br/download/details.aspx?id=17851)

## Deployment

Build and add the Desktop GUI's .exe to Windows's start-up folder (for windows 8/10 type WindowsKey+R and then type shell:startup) or choose to install the Windows Service project (experimental).

## Built With

-WPF (Windows Presentation Foundation) for the GUI
-NHibernate for ORM and FluentNhibernate for nice configuration (http://nhibernate.info/)
-FluentMigrator for database migrations (https://github.com/fluentmigrator/fluentmigrator)
-SharpArch for architecture and code reusability (http://sharparchitecture.github.io/)
-HtmlAgilityPack to manipulate html (https://github.com/zzzprojects/html-agility-pack)
-DiffPlex to get detailed diff information about what has been added and removed (https://github.com/mmanela/diffplex)
