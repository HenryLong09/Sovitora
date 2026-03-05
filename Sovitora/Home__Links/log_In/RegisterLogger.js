document.getElementById("SubmitForm").onclick = async function() {
    let emailEntered = document.getElementById("email").value.trim();
    const passwordEntered = document.getElementById("password").value.trim();
    const confirmPassword_Entered = document.getElementById("confirmPassword").value.trim();

    // Check passwords match
    if (passwordEntered !== confirmPassword_Entered) {
        alert("Passwords do not match!");
        return;
    }

    // Send the data to your API
    try {
        const response = await fetch("http://localhost:5100/api/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: emailEntered,
                password: passwordEntered
            })
        });

        if (response.ok) {
            alert("Registration successful!");
            document.getElementById("registerForm").reset(); // clear the form
        } else {
            const errorText = await response.text();
            alert("Registration failed: " + errorText);
        }
    } catch (err) {
        console.error(err);
        alert("Could not connect to the server.");
    }
};
