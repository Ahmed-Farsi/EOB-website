// const currentStep = 0;
// const steps = document.querySelectorAll('.step');
// const progressLabels = document.querySelectorAll('.progress-label');

// function showStep(index) {
//   steps[currentStep].classList.remove('current');
//   steps[currentStep].classList.add('previous');
//   progressLabels[currentStep].parentElement.classList.remove('current-item');

//   currentStep = index;

//   steps[currentStep].classList.remove('next');
//   steps[currentStep].classList.add('current');
//   progressLabels[currentStep].parentElement.classList.add('current-item');

//   if (currentStep < steps.length - 1) {
//     steps[currentStep + 1].classList.add('next');
//   } else {
//     steps[0].classList.add('next');
//   }

//   setTimeout(() => {
//     showStep((currentStep + 1) % steps.length);
//   }, 5000);
// }

// showStep(0);

