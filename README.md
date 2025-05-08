# CCG Microservices
Welcome to the microservices dump of my project. If you're looking for the exciting stuff like the game server, game client, Synapse Net, or the match-making server—yeah, those live in their own fancy little repos. What you'll find here is mostly the CRUD workhorses doing their boring but necessary jobs. Don't expect fireworks, just solid backend grunt work.

## Player Auth Server
As the name suggests, this microservice handles player authentication—creating accounts and logging players in using email and password. It uses JWTs for auth (because they’re dead simple), and those tokens are passed around like candy across the rest of the system. The Auth Server is completely independent; it doesn’t know or care about the other services, even if they constantly come knocking. It’s written in C# (.NET) and backed by PostgreSQL. (This also contains a player's owned cards table)

> Tags: C#, .NET, Postgres, Authentication, JWT, Entity Framework

## Card Catalog
This service stores all the card details. It’s built with C# (.NET) and uses MongoDB for storage. Right now, it doesn’t require authentication—because it’s just serving up public card data. Eventually, when I get around to adding an admin system, only authenticated users with the right permissions will be allowed to create or update cards.

> Tags: C#, .NET, MongoDB Driver

## Deck Collection
This is where players' decks are stored. Built with C# (.NET) and backed by MongoDB. All endpoints require user authentication, and it calls the Player Auth Service to verify card ownership when a player tries to create a new deck—because no one’s sneaking in cards they don’t own.

> Tags: C#, .NET, MongoDB Driver
