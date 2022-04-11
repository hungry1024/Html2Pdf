# Html2Pdf



dotnet publish -c Release src/Dinosaur.Html2Pdf/Dinosaur.Html2Pdf.csproj



cd src/Dinosaur.Html2Pdf/bin/Release/net6.0/publish

docker build -t dinosaur-html2pdf:1.0 .

docker run -d --restart=always --name html2pdf -p 5900:80 -v /opt/docker/doc-transfer/appsettings.json:/app/appsettings.json dinosaur-html2pdf:1.0