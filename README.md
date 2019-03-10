# Simple Forum
It's a simple ASP.NET web application for forum, which uses EF, AspNet.Identity and OWIN.
It is separated into six main projects (or assemblies):
1. DataContract - class library, that represents data access layer, this class library contains interfacess, that interact directly with the database using Entity Framework. 
Hierarchical data is loaded using __lazy loading__ - the simplest way to load data (navigation properties are virtual like [here](https://github.com/StruninIhor/SimpleForum/blob/test/DataContract/Models/Forum.cs#L13))
2. DataAccessServices - library, that contains implementations of DataContract interfaces
3. BusinessContract - library with business interfaces, these interfaces use DataContract to interact with the Database. They provide availible interaction with the database and they also provide validation
4. BusinessServices - implementations of BusinessContract interfaces
5. DependencyInjection - library, which provides dependencies resolving between interfaces and implementations
6. Web - ASP.NET MVC project, the web part of the whole solution. It handles the HTTP requests and it is responsible for the presentation part of the web application

## Some other parts, which are planned to be developed in future:
1. MVC Web API project
2. Second client part of application, using Angular 7
