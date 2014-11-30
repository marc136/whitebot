rem SET LIB_DIR=""

rem als User Variable setzen (kann sofort verwendet werden)
SET LIB_DIR="%~dp0libs"

rem als System Variable setzen (Neustart wird benötigt)
SETX LIB_DIR "%~dp0libs"
