#🎥  Movie Database
Very simple webpage that retrieves movies based on your search string and return its details. Needs [API key](https://developers.themoviedb.org/3/getting-started) to work.

##How to run it
1. Open api.ts and add value to movieApi property - for example: 
`export const Keychain = {
                                                                     movieApi: "XXXXaaeeed695f16d0bbd8be21XXXX"
                                                                 }`
2. run `yarn start`
3. in browser, type in name of the movie. From the list, select one and see its detail on the right

##Jobs
1. start - start dev server for demo purpose
2. build - output files to dist directory
3. test - run very basic test jest suite
4. lint - used mainly to output code smell during build

##TODO
0. DO NOT INCLUDE personal API key Keychain object! Store it in the backend
1. make sorting type safe, do not switch by string
2. do not use Date object, use something more useful like MomentJS for date formatting
3. add movie details: reviews, similar movies, popularity count, more images, revenue, categories!
4. use some nicer http client - like axios, to make moviesService better a futureproof
5. refactor dist output - needs to be tested against multiple browsers that it works, etc...
6. more tests... and use enzyme adapter for react 17 as soon as it releases
7. expand movie API response type to cover all properties
8. protect against large responses from backend - add scrollbars and / or pagination with result limit


