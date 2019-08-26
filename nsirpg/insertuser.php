<?php
// Server Login variables
	$server_name = "localhost";
	$server_username = "root";
	$server_password = "";
	$database_name = "nsirpg";

// user variables
$username = $_POST["username"];
$password = $_POST["password"];
$email = $_POST["email"];

// check connection
	$conn = new mysqli($server_name, $server_username, $server_password, $database_name);
	if(!$conn)
	{
		die("Connection Failed.".mysql_connect_error());
	}
	

// check users exist
$namecheckquery = "SELECT username FROM users WHERE username = '".$username."'";
$namecheck = mysqli_query($conn, $namecheckquery);
IF(mysqli_num_rows($namecheck)>0)
{
	echo "Username Already Exists";	
	exit();
}

// check email exist
$emailcheckquery = "SELECT email FROM users WHERE email = '".$email."'";
$emailcheck = mysqli_query($conn, $emailcheckquery);
IF(mysqli_num_rows($emailcheck)>0)
{
	echo "Email Already Exists";
	exit();
}

// create user
	$salt = "\$5\$round = 5000\$"."YeahNahBoomNoolGernim".$username."\$";
	$hash = crypt($password, $salt);
	$insertuserquery = "INSERT INTO users (username,email,hash,salt) VALUE ('".$username."','".$email."','".$hash."','".$salt."');";
	mysqli_query($conn,$insertuserquery) or die("error insert failed");
	echo "Success";	
?>