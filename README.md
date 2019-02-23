# Simple Forum
It's a simple ASP.NET web application for forum, which uses EF, AspNet.Identity and OWIN.
It is separated into three main projects:
1. DataContract - data access layer, this class library contains classes, that interact directly with the database using Entity Framework. 
Hierarchical ata is loaded using __lazy loading__ - the simplest way to load data (navigation properties are virtual like [here](https://github.com/StruninIhor/SimpleForum/blob/test/DataContract/Models/Forum.cs#L13))
2. DataAccessServices - services, which provides data access. Very simple.
3. Web - ASP.NET MVC project, the web part of the whole solution. It handles the HTTP requests and it is responsible for the presentation part of the web application
