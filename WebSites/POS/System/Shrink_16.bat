start /b /w sqlcmd -S 127.0.0.1 -i E:\System\Shrink_16.sql -o E:\System\Shrink_16.txt
xcopy /S /D /C /Y "E:\Backup\*.*" "\\10.15.20.30\Data-Quyen\Backup_All\16\"
shutdown -r -f -t 0