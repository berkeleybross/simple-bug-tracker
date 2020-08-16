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
- run "./run.ps1". This will use docker-compose to build the solution and run postgres, and then use dotnet to create the database. The script should wipe/reset all data before starting.
- the website can be accessed on http://localhost:3001/. There should be links in the left navigation drawer for seeing a list of "bugs" and "users".
- To add a user, navigate to the user list and click the "add" button and enter the users name. Submitting the form will take you back to the list, where you can select a user to edit their name.
- To create a bug, go to the home page/bugs page and click the "new" button. You can enter a short title, a long description (no formatting yet) and select a currently active user. Submitting the form takes you to the list of all bugs, which you can then select to view/edit the details you entered. Viewing the bug also shows the timestamp for when it was created (in UTC)

## Commentary

This was a fun test, thankyou. It took me about 4 hours of working time (spread over about 5 hours).

Choices made:

- I chose to use Vue/Nuxt since I haven't used React in a while. The webserver is written in C#/DotNet core.
- I would have split the nuxt/api in seperate repositories (they can/should be deployed independantly) but for simplicity of building I put them in seperate top level folders.
- Given more time, I'd probably implement storage of bug data using some kind of event sourcing database, so that you can get a full history of how the bug progressed.
- I used vanilla Vuetify to get something that approximated "looks nice". I've never used it before and given more time I'd have focused on padding etc. It would have been nice to get the profile photo of the currently active user next to each bug in the list (I was thinking of using gravatar or similar to get a nice default photo).
- I was planning on implementing a kanban board for the list of bugs, but I realised i was running out of time so just put in a "close" button instead and filtered out closed bugs rather than put them in a column where they can be reopened. I have a "status" column which was supposed to be which column in the kanban board the bug is currently in, but since it was cut short it could probably be a boolean column.I dont think this would be hard to implement given a bit more time - https://codesandbox.io/s/animated-draggable-kanban-board-with-tailwind-and-vue-1ry0p?ref=madewithvuejs.com shows how it could be done in vue.
- I wanted to make the API a bit more robust to transient errors - there are currently no retries/timeouts etc. I also didnt implement data integrity (etags, serializable transactions etc) for speed of implementation. Data input validation isn't very well thought through either - for instance the UI limits some fields to 30 characters but the server side doesnt do any validation (except a few "required"s)
- I'm used to using dapper for database access, but for speed I experimented with using Entity Framework. I think it turned out well, but I did get bitten by a few gotchas around dependent entities not being loaded.
- For speed I skipped all forms of testing. A few integration tests probably would have helped at times (see above about entity framework!).