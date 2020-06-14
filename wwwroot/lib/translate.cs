<?php
$q = $_REQUEST["q"];
if ($q !=="") {
	$q = strtolower($q);
}
echo $q;

?>