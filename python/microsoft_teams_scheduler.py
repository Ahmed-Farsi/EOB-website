#In deze code moet je user_id vervangen door de ID van de gebruiker die de Teams-vergadering plant en access_token vervangen door de toegangstoken van de gebruiker.

Maak een Microsoft 365-account aan als je er nog geen hebt.

Registreer een nieuwe app in de Microsoft Azure-portal en maak een geheime sleutel aan. Hierdoor krijg je toegang tot de Microsoft Graph API.

Gebruik de Graph API om een Teams-vergadering te plannen en een link te genereren. Je hebt hiervoor de toegangstoken van de gebruiker nodig, dus zorg ervoor dat je de gebruiker hiervoor toestemming hebt gegeven.

Maak een HTML-link aan met de gegenereerde Teams-vergaderlink en plaats deze op je webpagina.

import requests
import json
import datetime

# 1. Verkrijg de toegangstoken van de gebruiker

# 2. Plan de Teams-vergadering
start_time = datetime.datetime.utcnow().replace(microsecond=0).isoformat() + "Z"
end_time = (datetime.datetime.utcnow() + datetime.timedelta(hours=1)).replace(microsecond=0).isoformat() + "Z"
url = "https://graph.microsoft.com/v1.0/users/{user_id}/onlineMeetings"
headers = {"Authorization": "Bearer " + access_token, "Content-Type": "application/json"}
data = {
    "startDateTime": start_time,
    "endDateTime": end_time,
    "subject": "Teams-vergadering met medewerker",
    "participants": {
        "attendees": [
            {
                "emailAddress": {
                    "address": "ahmed.farsi@pro-control.nl",
                    "name": "Ahmed Farsi"
                }
            }
        ]
    },
    "accessLevel": "everyone",
    "autoAdmittedUsers": "everyone",
    "lobbyBypassSettings": "disabled"
}
response = requests.post(url, headers=headers, data=json.dumps(data))
meeting_data = response.json()

# 3. Genereer de Teams-vergaderlink
join_url = meeting_data['joinUrl']

# 4. Maak een HTML-link aan met de gegenereerde Teams-vergaderlink en plaats deze op je webpagina.
html_link = "<a href='" + join_url + "'>Klik hier om de Teams-vergadering te starten</a>"
