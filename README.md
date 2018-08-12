# JamesMann.BotFramework

A collection of Bot Framework V4 extensions and middleware. I'll be migrating all the reusable stuff in existing demos to this library, and making it modular over time. In the meantime I'm using it for my video series on YouTube. Feel free to use, licensed with MIT - free to use / no liability or warranty.

## JamesMann.BotFramework.Middleware


### AzureAdAuthMiddleware

This middleware will allow your bot to authenticate with Azure AD. 

It was created to support integration with Microsoft Graph but it will work with any application that uses the OAuth 2.0 authorization code flow. https://docs.microsoft.com/en-gb/azure/active-directory/develop/v2-oauth2-auth-code-flow

It supports:
- Request an authorization code (Client side user consent)
- Request an access token using authorization code (Server side)
- Request an access token using refresh token (Server side)

**Note: This middleware requires you to store OAuth access/refresh tokens somewhere on the server side. I have purposefully not prescribed how to store these access tokens.  If you make use of this middleware you need to provide an implementation of `IAuthTokenStorage`. This should use secure storage like Azure Key Vault. Read up on that here. https://docs.microsoft.com/en-us/azure/key-vault/quick-create-net.**

#### Usage

##### Step 1 - Define an implementation of `IAuthTokenStorage` to store and retrieve tokens
This is an example of an in-memory `IAuthTokenStorage`. This is to demonstrate the principle only **AGAIN, DO NOT USE THIS FOR PRODUCTION APPLICATIONS** 

```
public class InMemoryAuthTokenStorage : IAuthTokenStorage
{
    private static readonly Dictionary<string, ConversationAuthToken> InMemoryDictionary = new Dictionary<string, ConversationAuthToken>();

    public ConversationAuthToken LoadConfiguration(string id)
    {
        if (InMemoryDictionary.ContainsKey(id))
        {
            return InMemoryDictionary[id];
        }

        return null;
    }

    public void SaveConfiguration(ConversationAuthToken token)
    {
        InMemoryDictionary[state.Id] = token;
    }
}
```
##### Step 2 - Register the middleware

To ensure that users are always authenticated, add this middleware to the start of the pipeline.

In your `Startup.cs` file, register an your `IAuthTokenStorage` implemnentation as a singleton into the asp dotnet core ioc. Then configure your bot type to use an instance of `AzureAdAuthMiddleware`.


```
var tokenStorage = new InMemoryAuthTokenStorage();
services.AddSingleton<IAuthTokenStorage>(tokenStorage);
          
services.AddBot<Bot>((options) => {
    options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);
                
    options.Middleware.Add(new AzureAdAuthMiddleware(tokenStorage, Configuration));
    // more middleware
});
```

Note this requires an instance of `IConfiguration` passing to it.  Use the instance injected into the `Startup.cs` class.  

The configuration can be read from your `appsettings.json` file which needs the following keys (I've included some sample permissions- you can change these to meet your needs).
```
{
  "AzureAdTenant": "<AZURE AD TENANT>",
  "AppClientId": "<AZURE AD APPLICATION ID>",
  "AppRedirectUri": "https://<HOSTNAME>:<PORT>/redirect",
  "PermissionsRequested": "Calendars.ReadWrite.Shared User.ReadBasic.All People.Read",
  "AppClientSecret": "<AZURE AD APPLICATION KEY>"
}
```


### TypingMiddleware

This middleware will show a 'typing' event whenever a long running operation is occurring in your bot or other middeware components in the pipeline.

This is a good visual cue to the user that your bot is doing something.

#### Usage

To ensure that users get appropriate feedback at all times, add this middleware to the start of the pipeline.

In your `Startup.cs` file, configure your bot type to use an instance of `TypingMiddleware`:

```
services.AddBot<Bot>((options) => {
    options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);
                
    options.Middleware.Add(new TypingMiddleware());
    // more middleware
});
```


### SpellCheckMiddleware

This middleware will spell check inbound text using Cognitive Services Spell Check and therefore requires a key. There is a free tier which meets my demo/PoC needs.  You can get more info at https://azure.microsoft.com/en-gb/services/cognitive-services/spell-check/

The implementation is naive at the moment in that it assumes that the suggestions are correct and replaces inbound text automatically. If you have more sophisticated needs please feel free to contribute!

#### Usage

Typically I would place this middleware at the end of the pipeline, but it will work anywhere.  


```
services.AddBot<Bot>((options) => {
    options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);
	
	// more middleware
	options.Middleware.Add(new SpellCheckMiddleware(Configuration));
});
```

Note this requires an instance of `IConfiguration` passing to it.  Use the instance injected into the `Startup.cs` class.  

The configuration can be read from your `appsettings.json` file which needs the following key

```
{
  "SpellCheckKey": "<YOUR SPELL CHECK KEY HERE>"
}
```
