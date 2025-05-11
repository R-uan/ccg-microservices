# Player Auth Server
The Player Auth Server is a micro-service responsible for the storage and authentication of a player's information. The information consists of player data: id, nickname, email, password hash, level, experience, wins, loses, banned status and card collection. The card collection is a many-to-one relationship between player and card-references. The reference table contains the card id and the amount obtained by the player.
### What are the server's responsibilities:
- Registration of new players and authentication of the same.
- Generation of authentication token upon authentication.
- Retrieve public and private player profile.
- Verification of the card collection table to confirm ownership of cards.
- Update mutable fields in player's account (email, password).
- Manage player card collection (add and remove cards).
### Some specifications
The server was built up from .NET Web API. The database used is PostgreSQL and the ORM is Entity Framework. The authentication token is in JWT format while the password hashing uses BCrypt.
## Endpoints
### Authentication

#### 🔓 Player Login
    PUBLIC POST /api/auth/login {
        "email": string,
        "password": string
    }

##### Responses
    Ok {
        "token": string
    }

    Unauthorized {}
    

#### 🔓 Player Registration
    PUBLIC POST /api/auth/register {
        "email": string,
        "username": string,
        "password": string
    }

##### Responses
    Ok {
        "player": {
            "guid": string,
            "email": string,
            "username":string
        }
    }

    BadRequest { }

### Player

#### 🔒 Update information (TODO)
    GUARDED PATCH /api/player { }

#### 🔒 Player Profile Information
    GUARDED GET /api/player/profile { }

##### Responses
    Ok {
        "id": string,
        "email": string,
        "username": string,
        "cardCollection": [string],
        "level": int,
        "experience": int,
        "wins": int,
        "losses": int,
        "lastLogin": date,
        "isBanned": bool
    }

    Unauthorized { }

#### 🔓 Partial Player Profile Information
    PUBLIC GET /api/player/partial/{playerId}` { }

##### Responses
    Ok {
        "id": string,
        "email": string,
        "username": string,
        "level": int,
        "wins": int,
        "losses": int,
    }

    NotFound { }

### Card Collection

#### 🔒 Check Player Collection 
    GUARDED GET /api/player/collection/check { 
        cardIds: string[]
    }

##### Responses
    Ok {
        ownedCards: string[],
        unownedCards: string[],
        invalidCards: string[]
    }
    
    Unauthorized { }

#### 🔒 Add new card to Player's collection
    GUARDED POST /api/player/collection { 
        cardId: string,
        amount: int
    }

##### Responses
    Ok {
        playerId: string,
        cardId: string,
        amount: int
        
    }

    Unauthorized { }
