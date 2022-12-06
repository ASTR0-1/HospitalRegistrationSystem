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

➡️ <i>Angular</i><br>
Angular v14.2.5 client for API.<br>

<h2>Swagger screenshots :</h2>
<b>Controllers :</b><br>

![image](https://user-images.githubusercontent.com/71894616/198849408-c5b400cf-968d-466f-b15b-60362765f768.png)

<br><b>Used DTO's :</b><br>
![image](https://user-images.githubusercontent.com/71894616/198849303-d4df1c8b-6666-496b-9ce1-9580b1c599e9.png)

<h2>Angular client screenshots :</h2>
<b>Home :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205894585-18533821-036f-4e8c-9117-3d2f54082d20.png)

<br><b>Doctors :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205895040-5b4646b3-1143-42aa-bfd4-294d642516f7.png)

<br><b>Clients :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205895163-0c27eebe-8074-4358-aaa9-42a212b0de0e.png)

<br><b>Searched doctors :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205895304-45ef8570-5450-43d5-9940-6eb3d73d0370.png)
![image](https://user-images.githubusercontent.com/71894616/205895369-49897b07-7bd4-45d1-ad9d-5ca0e7813f9a.png)

<br><b>Searched clients :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205896024-bb0c410c-e348-48f9-b373-7147fc369f84.png)
![image](https://user-images.githubusercontent.com/71894616/205896073-580bd3ba-de4a-487f-9e89-a0544df8fb47.png)

<br><b>Client appointments :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205896183-a58b763a-302d-4ccf-a727-887160a2278b.png)

<br><b>Doctor schedule :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205896442-a22a5415-1479-4ff3-9af6-b40bec9ecbd6.png)

<br><b>Client's visited appointments :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205896681-4c8bd7d1-0067-498f-87fb-6e2521ffe54b.png)

<br><b>Create new client :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205896771-e6f2957c-0658-42b3-aa5b-543d2c0f2c8a.png)

<br><b>Create new doctor :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205896829-258b30d1-ef10-4781-942c-df33b183508b.png)

<br><b>Add new appointment for doctor :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205897605-56056b1a-183f-4679-84e4-1c4f9db6fdce.png)

<br><b>Mark appointment as visited :</b><br>

![image](https://user-images.githubusercontent.com/71894616/205897036-6ed3564a-f871-4f9e-bb44-2e9dde504604.png)
