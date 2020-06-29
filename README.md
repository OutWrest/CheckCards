# Burps R Suite

CheckCards is a test application made to explore vulnerabilities on [ASP.NET](https://dotnet.microsoft.com/apps/aspnet) web applications. The application is made to use [ASP.NET](https://dotnet.microsoft.com/apps/aspnet) Web API and [JWT](https://jwt.io/) for authorization. Because many web applications today use these types of setups, this application's main purpose is to analyze common problems that may show up. 

## Installation 

Use [.NET](https://dotnet.microsoft.com/download) to compile and run the project:

*Make sure to add an stmp email*

```bash
git clone https://github.com/OutWrest/CheckCards.git
cd CheckCards
dotnet user-secrets init
dotnet user-secrets set "Email:Username" "<smtp username here>"
dotnet user-secrets set "Email:Password" "<smtp password here"
dotnet user-secrets set "Email:Host" "<your smtp host here, ex: smtp.gmail.com"
dotnet user-secrets set "Email:Port" "587"
dotnet user-secrets set "Email:From" "<your email address here, ex: myusername@gmail.com"
dotnet user-secrets set "JwtKey" "7cc9bb76-1e7d-41d7-a657-c5ec7e293f52"
dotnet run
```

The app will run on http://localhost:5000/.

## Testing

There are pre-made test accounts to try out:

*Please note that the password rules are really lax to allow for faster testing.*

| Username | Password | Name    | Email             | Answer 1 (Security Question) | Answer 2 (Security Question) |
|----------|----------|---------|-------------------|------------------------------|------------------------------|
| user     | asd      | name    | user@6mails.com   | -                            | -                            |
| user##   | *        | name##  | user2@6mails.com  | *                            | *                            |
| admin    | asd      | admin   | admin@6mails.com  | asd                          | asd                          |
| admin##  | *        | admin## | admin2@6mails.com | *                            | *                            |

**random lowercase letters (length = 5)*

*All emails can be access thru the [temp-mail.org](https://temp-mail.org/en) service, to access a specific email for testing, click on [change](https://temp-mail.org/en/change) and make sure you write the exact the email and select the correct domain*

## Known Issues

#### Failure to load users

There is a small chance that only some of the users will be created in a new session. It has something to do with the threads and I have no idea how to fix it.

#### Fix (also, not really)

Restart session and pray.