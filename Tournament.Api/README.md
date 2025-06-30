# Tournament API Exercise
## Overview
This is an exercise in creating a RESTful API using ASP.NET Core. The API will manage tournaments and games.

## Features
- List all tournaments (using **GET**)
  - Optionally sort by "Title" or "Date", ascending (default) or descending.
  - Optionally filter by "Title"
  - Optionally paginate results with `page` and `pageSize` query parameters.
- Get details of a specific tournament (using **GET**)
- Create a new tournament (using **POST**)
- Modify an existing tournament (using **PUT**)
- Modify an existing tournament (using **PATCH**)
- Delete a tournament (using **DELETE**)

- List all games, regardless of tournament (using **GET**)
- Get details of a specific game (using **GET**)
- Create a new game (using **POST**)
- Modify an existing game (using **PUT**)
- Modify an existing game (using **PATCH**)
- Delete a game (using **DELETE**)

- List all games in a specific tournament (using **GET**)

## Endpoints are
- /api/tournament/
  <ins>optional parameters:</ins>
  - includeGames=true/false
  - Sort=Title/Date
  - Reverse=true/false
  - Filter=\<title text>
  - Page=\<num>
  - PageSize=\<num>
- /api/tournament/\<id>/
  <ins>optional parameter:</ins>
  - includeGames=true/false

- /api/Game/
  <ins>optional parameters:</ins>
  - Title=\<title text>
  - ExactMatch=true/false
- /api/Game/\<id>