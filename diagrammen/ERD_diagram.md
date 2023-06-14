+------------------+                   +------------------+
|   Abonnement    |                   |    Gebruiker     |
+------------------+                   +------------------+
| id        (PK)  | (int)             | id        (PK)   | (int)
| naam            | (string)          | voornaam         | (string)
| prijs           | (float)           | achternaam       | (string)
| geldigheid_start| (date)            | email            | (string)
| geldigheid_eind | (date)            | wachtwoord       | (string)
+------------------+                   +------------------+

      |                                         |
      | 1                                     N |
      |                                         |
+------------------+                   +------------------+
|    Betaling      |                   |     Toegang      |
+------------------+                   +------------------+
| id         (PK)  | (int)             | id        (PK)   | (int)
| datum            | (date)            | datum            | (date)
| bedrag           | (float)           | gebruiker_id (FK)| (int)
| abonnement_id (FK)|--------->| abonnement_id (FK)| (int)
+------------------+                   +------------------+
Uitleg:

Een abonnement kan worden gekoppeld aan meerdere gebruikers, maar elke gebruiker heeft slechts één abonnement. Dit wordt weergegeven door de foreign key "abonnement_id" in de Gebruiker-tabel die verwijst naar de primaire sleutel "id" van de Abonnement-tabel.

Een abonnement kan meerdere betalingen hebben, maar elke betaling hoort bij één abonnement. Dit wordt weergegeven door de foreign key "abonnement_id" in de Betaling-tabel die verwijst naar de primaire sleutel "id" van de Abonnement-tabel.

Elk abonnement kan meerdere toegangen hebben, maar elke toegang hoort bij één abonnement. Dit wordt weergegeven door de foreign key "abonnement_id" in de Toegang-tabel die verwijst naar de primaire sleutel "id" van de Abonnement-tabel.

De pijlen geven de richting van de foreign key-relaties aan, waarbij de relatie van "1" naar "N" wordt weergegeven. Dit betekent dat aan de ene kant van de relatie (het "1"-uiteinde) zich één record bevindt, terwijl aan de andere kant van de relatie (het "N"-uiteinde) meerdere records kunnen zijn.