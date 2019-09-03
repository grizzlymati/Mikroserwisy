# Mikroserwisy
Aplikacja oparta na architekturze mikroserwisów.
Wykorzystane technologie to ASP.NET CORE, MSSQL, RabitMQ.
Oprocz oczywistych rzeczy wymaganych do działania ASP.NET CORE oraz MSSQL,
należy również założyć konto RabitMQ, zainstalować RabitMQ (jeżeli sie chce używać go lokalnie),
a przed tym zainstalowac Erlanga (wymagany do działania RabitMQ). Dodatkowo warto włączyć pluginy
poprzez dostanie się do miejsca w którym rabbitMq jest zainstalowany i wywołania z poziomu wiersza poleceń
komendy: rabbitmq-plugins enable rabbitmq_management - dzieki temu bedziemy mogli podgladac caly ruch w przegladarce.
