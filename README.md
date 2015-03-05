# EFETL
C# Entity Framework 6 ETL

Benchmark tests so far:

**First Test: Inserting 10,000 records.**

Num  | Description  | Milliseconds | Seconds
------------- | ------------- | -------------: | -------------:
1  | Using Entity Framework right out of the box  | 35,898  | 35.9
2  | Using Entity Framework optimized  | 4,612  | 4.5
3  | Using Entity Framework with BulkInsert nugget package  | 1,833  | 1.8

*2) Optimization included removing security checks for EF (AutoDetectChangesEnabled, ValidateOnSaveEnabled)

**Second Test: Inserting 1,000,000 records.**

Num  | Description  | Milliseconds | Seconds
------------- | ------------- | -------------: | -------------:
1  | Using Entity Framework with BulkInsert nugget package  | 10,208  | 10



###References
EntityFramework.BulkInsert https://efbulkinsert.codeplex.com/ (code is very simple)

