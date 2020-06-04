cd ~/FT_Retail

git clone https://github.com/JKSProds/FT_Retail.git

cd FT_Retail/FT_Retail

docker build -t aspnetapp .

docker stop FT_Retail
docker rm FT_Retail

cd ..
cd ..

chmod -R 777 FT_Retail

rm -r FT_Retail

docker run -d -p 8080:80 --restart always --name FT_Retail aspnetapp



