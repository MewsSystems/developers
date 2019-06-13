## Console application

Set **Mews.Backend.Task.Console** as startup project and hit F5. You can see result in console window.

## Docker compose

At the solution level, run `docker-compose up --build` or you can use Visual Studio Tools for Docker to run example.

### How to use

**Mews.Backend.Task.Api** will be available on http://localhost/api/exchange-rate. Use following HTTP request to retrieve exchange rates:

```
GET http://localhost/api/exchange-rate?currencies=USD&currencies=GBP
```

## Unit tests
You can see all unit tests in **Mews.Backend.Task.UnitTests** project.