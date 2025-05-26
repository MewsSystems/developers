# Mews backend developer task

We are focused on multiple backend frameworks at Mews. Depending on the job position you are applying for, you can choose among the following:

* [.NET](DotNet.md)
* [Ruby on Rails](RoR.md)

Run The App :
In a containerised way (from Backend folder):
Build the image
```sh
docker build -t exchangerate-task -f Task/Dockerfile .
Run the app
docker run -it --rm exchangerate-task
```

On Local Machine: 
```sh
dotnet restore
dotnet run --project Task
```