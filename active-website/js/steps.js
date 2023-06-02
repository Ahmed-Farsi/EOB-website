let currentStep = 0;
const steps = document.querySelectorAll('.step');
const progressItems = document.querySelectorAll('.step-wizard-item');
const stepLinks = document.querySelectorAll('.step-link');
const delay = 20000; // Aanpassen naar de gewenste vertraging (in milliseconden)

function showStep(index) {
  steps[currentStep].classList.remove('current');
  steps[currentStep].classList.add('previous');
  progressItems[currentStep].classList.remove('current-item');

  currentStep = index;

  steps[currentStep].classList.remove('next');
  steps[currentStep].classList.add('current');
  progressItems[currentStep].classList.add('current-item');

  if (currentStep < steps.length - 1) {
    steps[currentStep + 1].classList.add('next');
  } else {
    steps[0].classList.add('next');
  }
}

function handleStepLinkClick(event) {
  event.preventDefault(); // Prevent default link behavior
  const clickedStepIndex = parseInt(this.getAttribute('data-step-index'));

  showStep(clickedStepIndex);
}

// Add event listeners to the links
stepLinks.forEach((link) => {
  link.addEventListener('click', handleStepLinkClick);
});

// Continue automatic progression
function autoProgress() {
  showStep((currentStep + 1) % steps.length);
  setTimeout(autoProgress, delay);
}

autoProgress();
