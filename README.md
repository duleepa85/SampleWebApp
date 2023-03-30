                        Sample API (.NET Core with Azure functions)

Prerequisites
1. Visual Studio 2022
2. .NET Core 6.0
3. SQL Server Management Studio 2019

Steps
1. Checkout the code using https://github.com/duleepa85/SampleWebApp.git 
2. Open the solution using VS 2022
3. Make "Sample.EmployeeService.EmployeeFunctions" project as startup project
4. Build the solution
5. Open SQL Server Management Studio 2019 and execute the db scripts. Now you can see 2 databases will be created as SampleCatalogDB and SampleTenantDB
6. Open local.settings.json file in EmployeeFunctions project and update the "CatalogDBConnectionString" setting with the connection string of SampleCatalogDB database.
7. Run the project
8. Now you can see 2 functions are shown in new window func.exe like below
![image](https://user-images.githubusercontent.com/42170562/228753706-0d7e84c4-2feb-4602-bcc8-26a91b1a591b.png)

Calling functions
1. You can use postman or whatever the API testing tool
2. Add headers like below
![image](https://user-images.githubusercontent.com/42170562/228756682-b424e65e-d500-4a12-a01d-293383f33509.png)

For the token use AzureAD token. (You can get the token from graph explorer - https://developer.microsoft.com/en-us/graph/graph-explorer
3. For the post request(RegisterEmployee), you can use a json like below format.
   {
	"name": "John Smith",
	"age":"45"
   }
   For the get request, we can directly execute with headers.

