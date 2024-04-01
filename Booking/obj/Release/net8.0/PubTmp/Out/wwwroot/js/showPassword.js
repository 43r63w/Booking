document.addEventListener('loadedDom', function () {
	const passwordInput = document.getElementById('passwordInput')
	const showPasswordToggle = document.getElementById('showPasswordToggle')
	const showPasswordIcon = document.getElementById('showPasswordIcon')


	const confirmPasswordInput = document.getElementById('confirmPasswordInput')
	const confirmShowPasswordToggle = document.getElementById('confirmShowPasswordToggle')
	const confirmShowPasswordIcon = document.getElementById('confirmShowPasswordIcon')


	showPasswordToggle.addEventListener('click', showPassword())

	confirmShowPasswordToggle.addEventListener('click', confirmPasswordInput())
})



function showPassword() {
	if (passwordInput.type == 'password') {
		passwordInput.type = 'text'
		showPasswordIcon.classList.remove('bi-eye-slash')
		showPasswordIcon.classList.add('bi-eye')
	} else {
		passwordInput.type = 'password'

		showPasswordIcon.classList.remove('bi-eye')
		showPasswordIcon.classList.add('bi-eye-slash')
	}
}
function confirmShowPassword() {
	if (confirmPasswordInput.type == 'password') {
		confirmPasswordInput.type = 'text'
		confirmShowPasswordIcon.classList.remove('bi-eye-slash')
		confirmShowPasswordIcon.classList.add('bi-eye')
	} else {
		confirmPasswordInput.type = 'password'

		confirmShowPasswordIcon.classList.remove('bi-eye')
		confirmShowPasswordIcon.classList.add('bi-eye-slash')
	}
}