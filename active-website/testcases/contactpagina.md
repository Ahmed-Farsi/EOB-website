Testrapport: Contactpagina

Testdatum: 7 april 2023

Tester: Ahmed Farsi

Doel van de test:
Het doel van deze test is om te controleren of het contactformulier op de website correct functioneert en of de gebruiker snel en gemakkelijk antwoord kan krijgen op zijn vraag.

Testscenario's:
Er zijn vier testscenario's uitgevoerd om te controleren of het contactformulier correct functioneert. Hieronder vindt u de resultaten van elke test:


User Story
Als een websitebezoeker, wil ik het contactformulier op de website kunnen gebruiken om een vraag te stellen, zodat ik snel en gemakkelijk antwoord kan krijgen op mijn vraag.

Test Scenario's

Scenario 1: Versturen van een leeg contactformulier
1 Ga naar de contactpagina.
2 Laat alle velden leeg.
3 Klik op de verzendknop.
4 Controleer of er een foutmelding wordt weergegeven.

Conclusie: Het contactformulier kan niet worden verzonden zonder dat alle verplichte velden zijn ingevuld.

Scenario 2: Versturen van een volledig ingevuld contactformulier
1 Ga naar de contactpagina.
2 Vul alle velden in met juiste gegevens.
3 Klik op de verzendknop.
4 Controleer of er een bevestegingsmail wordt verzonden.

Conclusie: Het contactformulier kan worden verzonden met juiste gegevens en er wordt een bevestegingsmail verstuurd.

Scenario 3: Versturen van een contactformulier met onjuiste gegevens
1 Ga naar de contactpagina.
2 Vul één of meerdere velden in met onjuiste gegevens.
3 Klik op de verzendknop.
4 Controleer of er een foutmelding wordt weergegeven.

Conclusie: Het contactformulier kan niet worden verzonden met onjuiste gegevens.

Scenario 4: Versturen van een contactformulier met alternatief pad
1 Ga naar de contactpagina.
2 Vul alle velden in met juiste gegevens.
3 Schakel de internetverbinding uit.
4 Klik op de verzendknop.
5 Controleer of er een foutmelding wordt weergegeven.
6 Schakel de internetverbinding weer in.
7 Controleer of het contactformulier alsnog wordt verzonden.

Conclusie: Het contactformulier kan worden verzonden met juiste gegevens, zelfs als er tijdelijk geen internetverbinding is.

Testdata
Naam: John Doe
E-mail: Ahmedfarsi49@gmail.com
Telefoonnummer: 0123456789
Onderwerp: Vraag over product
Bericht: Dit is een testbericht.

Gewenste Resultaten
Scenario 1: Een foutmelding wordt weergegeven en het contactformulier kan niet worden verzonden.
Scenario 2: Een bevestegingsmail wordt verstuurd en het contactformulier is verzonden.
Scenario 3: Een foutmelding wordt weergegeven en het contactformulier kan niet worden verzonden.
Scenario 4: Het contactformulier wordt verzonden zodra de internetverbinding is hersteld.

Alternatieve paden
1 Bij het verzenden van het contactformulier kan de server down zijn. In dit geval wordt er een foutmelding weergegeven en moet de gebruiker het later opnieuw proberen.
2 Als er onjuiste gegevens zijn ingevuld, wordt er een foutmelding weergegeven en moet de gebruiker de juiste gegevens invullen om het formulier te kunnen verzenden. Als het verzenden van het formulier succesvol is, wordt de gebruiker doorgestuurd naar een bedankpagina waarop staat dat het bericht is ontvangen en er zo spoedig mogelijk contact zal worden opgenomen. Een bevestigingsmail wordt niet verstuurd naar de gebruiker, maar het bericht wordt wel automatisch doorgestuurd naar het bedrijfs-e-mailadres voor verdere afhandeling.

Verbeteringen voorstellen:

1 Implementeer validatie voor elk veld in het contactformulier, zodat gebruikers niet in staat zijn om het formulier te verzenden met ontbrekende of onjuiste gegevens.

2 Voeg een captcha toe aan het contactformulier om spam te voorkomen en ervoor te zorgen dat alleen echte gebruikers het formulier kunnen verzenden.

3 Stuur een bevestigingsmail naar de gebruiker wanneer het contactformulier succesvol is verzonden, zodat de gebruiker weet dat het bericht is ontvangen en dat er zo spoedig mogelijk contact zal worden opgenomen.

4 Implementeer een systeem dat automatisch berichten doorstuurt naar de juiste afdeling op basis van het onderwerp dat is gekozen door de gebruiker.

5 Voeg een optie toe aan het contactformulier waarmee gebruikers kunnen kiezen of ze een kopie van hun bericht willen ontvangen.

6 Implementeer een automatische ontvangstbevestiging voor de gebruiker, zodat deze weet dat het bericht is ontvangen en er zo spoedig mogelijk contact zal worden opgenomen. Dit kan de gebruikerservaring verbeteren en bijdragen aan het vertrouwen van de gebruiker in het bedrijf.

Deze verbetervoorstellen hebben betrekking op technische en functionele oplossingen die de samengestelde functionaliteit van het contactformulier verbeteren. Ze zijn gebaseerd op de resultaten van de tests en de conclusies en ze verbeteren de functionaliteit en het gebruik van de ontwikkelde software. Ze hebben ook een duidelijke relatie met de opdracht en de user stories.