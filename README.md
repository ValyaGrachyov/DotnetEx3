Чтобы запустить бэк в докере:
1. Экспортировать dev сертификат aspnetapp.pfx в папку ./TicTacToe_Backend/certificates/https
	dotnet dev-certs https -ep <ПУТЬ ДО РЕПОЗИТОРИЯ>\TicTacToe_Backend\certificates\https\aspnetapp.pfx -p password12345 --trust
2. Перейти в директорию с .yml прописать
	docker-compose up -d
