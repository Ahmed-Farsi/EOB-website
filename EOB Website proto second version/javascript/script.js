// Een functie om de kleur van de header te veranderen wanneer er gescrold wordt
window.addEventListener('scroll', function() {
	const header = document.querySelector('header');
	header.classList.toggle('scrolled', window.scrollY > 0);
});

// Een functie
