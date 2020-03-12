
http://localhost:5001/locations/92d21196-dbd0-42c0-8b35-4bba3713defb
{
	"ID":"92d21196-dbd0-42c0-8b35-4bba3713defb",
	"Latitude":12.56,
	"Longitude":23.54,
	"Altitude":1200,
	"Timestamp":12,
	"MemberID":"87e21196-dbd0-42c0-8b35-4bba3713defc"
}

//

{"ID":"92d21196-dbd0-42c0-8b35-4bba3713defb","Latitude":1.2,"Longitude":23.54,"Altitude":2.2,"Timestamp":33,"MemberID":"92d21196-dbd0-42c0-8b35-4bba3713defb"}

docker run -p 5432:5432 --name some-postgres -e POSTGRES_PASSWORD=inteword -e POSTGRES_USER=integrator -e POSTGRES_DB=locationservice -d postgres

docker run -it --rm --link some-postgres:postgres postgres psql -h postgres -U integrator -d locationservice

docker run -it --rm --link some-postgres:postgres -p 5000:5003 locationservice
