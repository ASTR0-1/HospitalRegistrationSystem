<h1>🏥HospitalRegistrationSystem</h1>

Main idea of this application is to control Hospital appointments 

<h3>Application made with: <ins>Clean architecture</ins></h3>

<h4>Using these techonlogies :</h4>
➡️ <i>ASP.NET Core 6.0</i><br>
UI(to be added soon) interacts with application using API Controllers.<br>
To test controllers we use <i>Swagger</i>.

<br>➡️ <i>Entity Framework Core</i><br>
Used for creating database tables with "code-first" approach.<br>
To interact with database we use repositories and for business logic we use services.<br>
To provide data from services to the end user we use data transfer objects.<br>

➡️ <i>AutoMapper</i><br>
To translate data from Domain entities to DTO we use AutoMapper and MappingConfigrutaion for each entity.<br>

➡️ <i>Fluent Validation</i><br>
To validate DTO we use FluentValidation configuration for each DTO we use for creation.<br>

➡️ <i>NLog</i><br>
To log exceptional situations we use NLog to log info/warnings/errors/debug into files.<br>

<h2>Swagger screenshots :</h2>
<b>Controllers :</b><br>

![image](https://user-images.githubusercontent.com/71894616/198849408-c5b400cf-968d-466f-b15b-60362765f768.png)

<br><b>Used DTO's :</b><br>
![image](https://user-images.githubusercontent.com/71894616/198849303-d4df1c8b-6666-496b-9ce1-9580b1c599e9.png)

