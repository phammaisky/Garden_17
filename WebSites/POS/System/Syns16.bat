start /b /w sqlcmd -S 127.0.0.1 -i E:\System\HoaDon.sql -o E:\System\HoaDon.txt
start /b /w sqlcmd -S 127.0.0.1 -i E:\System\FC.sql -o E:\System\FC.txt
start /b /w sqlcmd -S 127.0.0.1 -i E:\System\Shop.sql -o E:\System\Shop.txt
start /b /w sqlcmd -S 127.0.0.1 -i E:\System\NhanVien.sql -o E:\System\NhanVien.txt
shutdown -r -f -t 0