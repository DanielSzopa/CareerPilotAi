# Infrastructure Layer Guidelines

Use the Infrastructure layer to implement the technical details of the application.
e.g. Email service, OpenRouter service, Identity service, Database context, etc.

## Persistence layer

Use Persistence layer to implement the data access logic of the application.
Please keep the Code First approach using Entity Framework Core.

`./Persistence/ApplicationDbContext.cs` is the Database context, where the DbSet properties are defined.
Use `./Persistence/DataModels` to define the Data Models. In the CareerPilotAi project, only Data Models can fetch and save data to the database.
Use `./Persistence/Migrations` to define the EF Migrations.
Use `./Persistence/Configurations` to define the Entity Framework Core configurations for Data Models. They have definitions of relationships between Data Models, keys etc.
