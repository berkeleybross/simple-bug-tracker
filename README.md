# simple-bug-tracker

## Goal

- It should be possible to view the list of open bugs, including their titles
- It should be possible to view the detail of individual bugs, including the title the full
  description, and when it was opened
- It should be possible to create bugs
- It should be possible to close bugs
- It should be possible to assign bugs to people
- It should be possible to add people to the system
- It should be possible to change people’s names
- The web application should look nice
- The web application should expose some sort of API
- The data should be stored in some sort of database

The test deliberately leaves many decisions up to you. We encourage you to use techniques
and technologies that you feel reflect good practice, but don’t be afraid to make a choice
based on personal preference, especially if you feel it will work well for this task.

You can work in an agile way. It doesn’t matter if you are not able to implement all the
features in a reasonable amount of time. We’re far more interested in how you approach
problems, and what your strengths are, than just getting to the end of the requirements.

Please document how you build, run and test the application, then either zip it up, or send us
a link to a GitHub or Bitbucket repository. Feel free to include commentary on the choices you
made, and any difficulties you encountered.

## How to build

Requires: powershell core (v7), dotnet 3.1.300, docker, docker-compose

- Open powershell at the root of the solution
- run "./run.ps1". This will use docker-compose to build the solution and run postgres, and then use dotnet to create the database.
- the website can be accessed on http://localhost:3001/
