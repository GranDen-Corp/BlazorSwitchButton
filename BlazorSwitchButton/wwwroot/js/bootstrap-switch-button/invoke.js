export function createSwitchButton (switchBtnContainer) {
   let input = document.createElement('input');
   input.setAttribute('type', 'checkbox');
   input.checked = true;
   switchBtnContainer.appendChild(input);
   input.switchButton();
}