# JamesMann.BotFramework

## JamesMann.BotFramework.Middleware

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

The implementation is naive at the moment in that it assumes that the suggestions are correct and replaces inbound text automatically. If you have more sophisticated needs please feel free to let me know.

####

Usage

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

### AzureAdAuthMiddleware

Needs write up.