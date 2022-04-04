# Issue: JwtPaddingDemo
## About
Demonstrates two instances of an aspnetcore web api
- Post 50443
  - Web Service with JwtBearer Signing Key set without manual padding
- Post 51443
  - Web Service with JwtBearer Signing Key set with manual padding

## Console Output
~~~
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:50443
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: [REDACTED]\JwtBearerWithoutKeyPadding\bin\Debug\net6.0\
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:51443
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: [REDACTED]\JwtBearerWithoutKeyPadding\bin\Debug\net6.0\
The app instance without the padding threw the following:
Response status code does not indicate success: 401 (Unauthorized).
The app instance with the padding returned the following:
John Doe
~~~

Can also be evaluated via the /swagger endpoint on each port

## Payload / Bearer Token
- Tested with 
  - Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.ISl-kYtYCmmD4Er8oXOlyDz3PHsmZDsdd1AmMaKtLCA
- This was the default payload from JWT.IO, with HS256 and the secret "shortkey"

## Allowing port binding
Note, may need to add perms to host on these endpoints for this to run, or run as admin:
~~~
netsh http add urlacl url=http://+:50443/User user=%username%
netsh http add urlacl url=http://+:51443/User user=%username%
~~~
