          +------------------+   +------------------+
          |     Abonnement   |   |     Gebruiker     |
          +------------------+   +------------------+
          | id               |   | id               |
          | naam             |   | voornaam         |
          | prijs            |   | achternaam       |
          | geldigheid_start |   | email            |
          | geldigheid_eind  |   | wachtwoord       |
          |                  |   |                  |
          | 1--->N           |   | 1--->N           |
          +------------------+   +------------------+
                   |                        |
                   |                        |
          +------------------+   +------------------+
          |      Betaling    |   |     Toegang       |
          +------------------+   +------------------+
          | id               |   | id               |
          | datum            |   | datum            |
          | bedrag           |   | gebruiker_id     |
          | abonnement_id    |   | abonnement_id    |
          |                  |   |                  |
          | N<---1           |   | N<---1           |
          +------------------+   +------------------+

Dit diagram laat zien hoe onze website werkt. Er zijn verschillende onderdelen, zoals Abonnement, Gebruiker, Betaling en Toegang. Een abonnement kan worden gekoppeld aan meerdere gebruikers, maar elke gebruiker heeft slechts één abonnement. Ook kan een abonnement meerdere betalingen hebben, maar elke betaling hoort bij één abonnement. En tot slot kan elk abonnement meerdere toegangen hebben, maar elke toegang hoort bij één abonnement. Zo zorgen we ervoor dat alles netjes georganiseerd is en onze gebruikers de juiste toegang hebben tot onze tool.