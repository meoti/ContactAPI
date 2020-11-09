## Contacts API Challenge (OpenWT)

### Running on Local

The default **LocalDB with SqlServer Express** in used as oposed to an inMemory storage

Please run migration provide in the migartion folder to setup LocalDB instance 

Eg, migration command from pakage manager console

> dotnet ef database update --project .\ContactAPI\ContactAPI.csproj

### Not Implemented in this Solution
1. Logger 
2. Global Exception Filter
3. Collection Resource Navigation properties eg, total size, prev, next, etc. Thus no collection response model

-------

Thank you !
