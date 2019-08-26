<?php
// Server Login variables
	$server_name = "localhost";
	$server_username = "root";
	$server_password = "";
	$database_name = "nsirpg";

// user variables
$username = $_POST["username"];
$password = $_POST["password"];

// check connection
	$conn = new mysqli($server_name, $server_username, $server_password, $database_name);
	if(!$conn)
	{
		die("Connection Failed.".mysql_connect_error());
	}	
//http://localhost/nsirpg/Login.php
	
// Check users exist
$namecheckquery = "SELECT username, salt, hash FROM users WHERE username = '".$username."';";
$namecheck = mysqli_query($conn, $namecheckquery);
if(mysqli_num_rows($namecheck) != 1)
{
	echo "Incorrect Username";
	exit();
}

// get login from query	
$existinginfo = mysqli_fetch_assoc($namecheck);
$salt = $existinginfo["salt"];
$hash = $existinginfo["hash"];

$loginhash = crypt($password,$salt);
if($hash != $loginhash)
{
	echo "Incorrect Password";
	exit();	
}
else
{
	echo "Login Successful";
	exit();
}
?>