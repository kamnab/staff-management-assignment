# staff-management-assignment

### StaffManagementApi Project structure:
- Data: Persist data storage such as DbContext
- Models: Store entity models such as Staff.cs
- DTOs: Request/Response entities between backend and frontend
- Services: Data service layer/middleware, mostly data query from db and return. This will utilize SOLID priciple
    + StaffService: handle staff related CRUD or related to staff
    + ExcelService: handle task related to excel such as export as a raw data or different excel format
- Controllers: Mostly consume XXServices to serve the request
- Program.cs: is where the service is register, configuration, disable or enable. 

### StaffManagementApi.Tests
```
cd StaffManagementApi.Tests
dotnet test
```

### CI Configuration:
```
.github/workflows/dotnet.yml
```

# Follow the below snippet to run the project
```
git clone https://github.com/kamnab/staff-management-assignment
cd staff-management-assignment
dotnet restore ./StaffManagementApi.sln
dotnet build ./StaffManagementApi.sln
dotnet run --project ./StaffManagementApi/StaffManagementApi.csproj

```




