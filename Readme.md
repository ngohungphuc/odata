> Odata Learning

http://odata.github.io/WebApi/#01-01-preface

https://damienbod.com/2014/06/10/getting-started-with-web-api-and-odata-v4/

https://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/OData/v4/

http://aspnetwebstack.codeplex.com/

http://www.asp.net/web-api/overview/releases/whats-new-in-aspnet-web-api-22

http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api/odata-routing-conventions

http://stackoverflow.com/questions/18233059/apicontroller-vs-odatacontroller-when-exposing-dtos


Testing Url

http://localhost:53613/odata/People
http://localhost:53613/odata/VinylRecords
http://localhost:53613/odata/People$select=Email
http://localhost:53613/odata/People?$select=Gender
http://localhost:53613/odata/People(1)/Email
http://localhost:53613/odata/People(1)?$select=Email,FirstName
http://localhost:53613/odata/People(1)/VinylRecords?$select=Title
http://localhost:53613/odata/People?$expand=VinylRecords,Friends
http://localhost:53613/odata/People?$orderby=Gender  desc, Email