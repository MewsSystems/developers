# Movie App Next.js

This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

### Setup your env file

You will need to obtain an access token from TMDB - https://www.themoviedb.org/settings/api. 

You can sign up and create an account if you don't already have one.

You will need your API Read Access Token. Then create a file in the root folder of this project called `.env.local`. 

It should look like the following. Add your token

```bash
# api code
MOVIE_DB_ACCESS_TOKEN=YOUR-TOKEN-HERE
MOVIES_SEARCH_REVALIDATE=300
MOVIES_DETAIL_REVALIDATE=3600
MOVIES_CONFIGURATION_REVALIDATE=7200

# server side page component code
SEARCH_REVALIDATE_TIME=300
MOVIE_REVALIDATE_TIME=3600

# client side code
NEXT_PUBLIC_CLIENT_SIDE_SEARCH_REVALIDATE_TIME=300
NEXT_PUBLIC_CLIENT_SIDE_MOVIE_REVALIDATE_TIME=3600

# current domain
NEXT_PUBLIC_SITE_URL=http://localhost:3000
```

The project uses npm. Run the follow to get set up.

```
npm i
```

### Production build of the app

To see the app running optimally you can do a production build of the app and see that working locally.

```bash
npm run build
npm run start
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

I would advise testing with the production build and using an incognito browser window.

### Local development run

To benefit from hot reloading while working on the app you can run..

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

There is also a Storybook set up

```bash
npm run storybook
```

This will allow you to look at and work on specific components in isolation

### Dev scripts

See the scripts section of the package.json for scripts

```bash
npm run lint
npm run test
npm run typecheck
```
### Code formating
The project uses eslint and prettier to manage code format

## Technical Decisions

### General approach

The basic idea here was to first build a working skeleton for the whole app and then be able to drill in to
parts of the code later to make improvements and enhance and improve the experience. I have been very constrained
in terms of time available to work on this so I aimed to get something that I could demo. There are lots of improvements
that I would make and certainly in terms of process I would probably work a bit differently ifI was doing this for me day job.


### Why use Next.js

I prefer to keep API keys and Access Tokens server side where possible. 

This is more secure and prevents malicious usage and also where services might be rate limited helps to protect our usage of external systems like TMDB.

Next.js has the ability to set up backend api endpoints easily and also gives you the benefit of Next.js caching to optimise performance.

So it made sense to establish BFF for the frontend app.

### data API

We have two data api endpoints

 - The search `/api/movies?search=cars&page=3` 
   - The code for which is in `src/app/api/movies/route.ts`
 - The movie details - `/api/movies/21192`
   - The code for which is in `src/app/api/movies/[movieId]/route.ts`

The search uses 
- TMDB's search endpoint `https://api.themoviedb.org/3/search/movie?query=car&page=1`
- and also the TMDB's configuration endpoint `https://api.themoviedb.org/3/configuration`

The movie details uses 
- TMDB's movie endpoint `https://api.themoviedb.org/3/movie/11` 
- and also the TMDB's configuration endpoint `https://api.themoviedb.org/3/configuration`

We assume that the data here will not change very often and take full advantage of the in memory caching 
feature that has been added to fetch for Next.js. We establish cache times like the following
- 5 mins for search results - many movies so might change more frequently that the other data
- 1 hour for movie details - unlikely to change often
- 2 hours for configuration - might rarely change

Env variables are also established to allow these times to be controlled and adjusted

Configuration is currently used to build the full image urls on both our endpoints. 
The configuration response from TMDB is cached in memory - while cached it can be accessed relatively quickly with 
benefits to all users of the endpoint. 

Our endpoints build and aggregate data for the frontend to consume data from multiple sources but using a single request.

### Client code

I am using the App router here rather than the Page router in Next.

We have two frontend pages  

- The search page `/` - http://localhost:3000/?search=Cars&page=9
    - The code for which starts in `src/app/page.tsx`
- The movie details page `/movies/:movieId` - `http://localhost:3000/movies/1726-iron-man`
    - The code for which starts in `src/app/movies/[movieId]/page.tsx`

They use a share layout that is defined in `src/app/layout.tsx`

Both the pages mentioned are serverside rendered they did a fetch to get teh data for the pages initial load
and pass that data to the frontend for hydration - which again can take advantage of Next.js in memory caching. 
Cache times can be configured in the env file.

### Tanstack Query hydration

I have also decided to use Tanstack Query hydration as a pattern.
This allows the serverside render not only hydrate React and quickly present the HTML but also means the 
Tanstack Query cache can be hydrated. Once loaded and configured on the frontend Tanstack Query can take 
over the data loading and give the users benefits of frontend cache data while they go backwards 
and forwards in the search page. 

I have also used this on the movie details page. You could argue here that the frontend cache is not as useful 
as caching on the search page. I also decide to be consistent across the two pages in how their data is initialised and hydrated.
So we have a standard data loading pattern. 

Currently the Next.js Link component isn't always able to provide "Soft navigation", 
where routing is solely via the frontend JSON fetch rather instead a full component page load - but once this problem is resolved the page could take full
advantage of cache. In the meantime the page can be cached by setting the revalidate time of page.


### Styling

I have used frameworks like styled components and emotion on projects before but know from experience that these
frameworks don't always work so nicely with Next.js and the App router. I decided to use Tailwind Css as I have worked a little
with that before and it works well with App router in Next. Also Tailwind Css comes with a pre-made theme that gives some nice options for color palettes
and spacing tokens that are very useful.

### General Tests

I have written some tests around the endpoints 

- `src/app/api/movies/route.test.ts`
- `src/app/api/movies/[movieId]/route.test.ts`

And some tests around the search page that are integration tests in style

- `src/features/home/HomeSearchSection.test.tsx`

I would normally write more tests. There are other tests that I could write for the HomeSearchSection,
I would write more tests for the MovieDetailsSection, test the hydration handling on the serverside page.tsx as 
well as test some of the components in isolation. I just didn't have the time here to do what I would have liked to do.

### Future improvements

I had limited time so here are a few other things I would look at..

- More tests
- Manual testing around accessibility
- Better display for small screens - I would love to put more time into this, I think the search page could be optimised quite nicely for smaller screens
  - You could even add additional image sizes to the api endpoint and use different image sizes for different media queries
- Optimise our endpoints but reducing the payload size - there is currently data we send to the frontend that isn't used
- It's not the prettiest web app (I'm not really a designer) again with a bit more time it would be good to put more polish on it terms of looks
- Add husky to get checks running on push
- Start to move the components into their own folders and using barrel files. The folders would contain tests, storybook files and other sub-components
- There is still some opportunity for code reuse in the endpoints and some of the components (such as MovieListItem and MovieDetailsView)
- Set up a pipeline using github actions and establish a live environment

### What I would have done differently 

I haven't actually worked much with Tanstack Query hydration in the past and in my real day to day job I would probably 
want to do a spike of that before committing to doing it with production code. Because I'm doing this in my free time
I thought that try this new approach would make the challenge a bit more interesting for myself.
