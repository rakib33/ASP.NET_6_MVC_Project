# ASP.NET_6_MVC_Project

## Task A
1. Create an app that generates a JSON file from Excel
2. Parse a Excel file
3. Convert the Excel data into C# object
4. Serialize the data into JSON format.
5. Save the JSON data to a file.
6. Database Integration with PostgreSQL.

## Task B
7. Implement a controller action that retrieves data from the JSON file or PostgreSQL database.
8. Create a page to load content using JSON or PostgreSQL.
9. Provide an option to choose between loading data from the JSON file or the PostgreSQL database.

### Filtering and Searching:
10. Add functionality to filter content based on a date range.
11.Implement a textbox search to filter content based on user input.

## Task C

12. HTML Table Customization:
13 Create an HTML table with columns corresponding to the data fields.
14 Implement sorting functionality for each column (A-Z, Z-A).
15 Allow users to customize which columns are displayed in the table.

# Front-end Tasks

1. create a new MVC C# .NET 6 project with a basic structure.
2. Pass the created json file and display data in a page.
3. Page design looks like
   
   ![image](https://github.com/user-attachments/assets/c880bdd6-68b4-4bcc-90ab-27d6f1de658a)

   ![image](https://github.com/user-attachments/assets/afd7e3b5-cd39-4a29-b369-d71aa01ce7a7)
5. Use jQuery to fetch and parse the JSON data from data.json. Data in the design that are not part of the JSON can be static and hardcoded.
6. Generate a mobile-responsive card grid layout based on the data for all screen.
7. No functionality is required to be build. All buttons and forms can be static.

## Create an app that generates a JSON file from Excel

1. Create a solution in visual studio 2022 named AssesmentV4 or you can provide any Name.
2. Create a ASP.NET Core MVC project under this solution with same name or different name.

   ![image](https://github.com/user-attachments/assets/19c840b5-f99e-4fed-839b-c88d4bbd7191)

3. Select .Net 6 framework and create this project.
   
   ![image](https://github.com/user-attachments/assets/71a2b625-4d4f-4903-aa9c-75654cbdd7f1)

4. For database integration install postreSQL from [here](https://www.postgresql.org/download/windows/)
5. Install pgAdmin also.
6. Check connection string through pgAdmin.
   
   ![image](https://github.com/user-attachments/assets/30c7a4bc-6532-4799-966e-cecc05658b57)

7. Configure appsettings.json. Give Excel file path and json file path and PostreSQL connection string.
   
   ```
   
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=ProductDB;User Id=postgres;Password=1234"
    },
   "ExcelDataSource": "E:\\TechnicalAssesment\\vodus-test-excel.xlsx",
   "OuputJsonPath": "E:\\TechnicalAssesment\\Order.json",
    ```
 

