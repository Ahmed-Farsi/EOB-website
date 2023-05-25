<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $name = $_POST['name'];
    $phone = $_POST['phone'];
    $email = $_POST['email'];
    $subject = $_POST['subject'];
    $message = $_POST['message'];
    
    // Verstuur de e-mail
    $to = "ahmed.farsi@pro-control.nl";
    $headers = "From: $email\r\n";
    $headers .= "Reply-To: $email\r\n";
    $message = "Naam: $name\r\nTelefoon: $phone\r\nE-mail: $email\r\nOnderwerp: $subject\r\nBericht: $message\r\n";
    
    if (mail($to, $subject, $message, $headers)) {
        echo "Bedankt voor je bericht! We zullen zo snel mogelijk contact met je opnemen.";
    } else {
        echo "Er is een probleem opgetreden bij het verzenden van het bericht. Probeer het later opnieuw.";
    }
}
?>
<!-- contact@engineeroutofthebox.nl -->