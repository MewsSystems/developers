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

It will run on http://localhost:6006/

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
that I would make and certainly in terms of process I would probably work a bit differently if I was doing this for my day job.


### Why use Next.js

I prefer to keep API keys and Access Tokens server side where possible. 

This is more secure and prevents malicious usage and also where services might be rate limited helps to protect our usage of external systems like TMDB.

Next.js has the ability to set up backend api endpoints easily and also gives you the benefit of Next.js caching to optimise performance.

So it made sense to establish BFF for the frontend app.

### Site structure and caching strategy

You can find information about this on the [Site Structure Overview document](../docs/site-structure-and-caching.md)


### SSR Hydration Pattern with Tanstack Query

For the search page we are using the SSR Hydration Pattern with Tanstack Query - you can read more about this in the
[SSR Hydration Pattern with Tanstack Query document](../docs/hydration-pattern-with-tanstack-query.md)

### Styling

I have used frameworks like styled components and emotion on projects before but know from experience that these
frameworks don't always work so nicely with Next.js and the App router. I decided to use Tailwind Css as I have worked a little
with that before and it works well with App router in Next. Also Tailwind Css comes with a pre-made theme that gives some nice options for color palettes
and spacing tokens that are very useful.

### General Tests

Most of the components and pages currently have unit tests, some tests would still be good for some of the utils in our lib folder

### Future improvements

- Additional error handling on the home page
- Add husky to get checks running on push
- Set up a pipeline using github actions and establish a live environment
- Functional tests

### What I would have done differently 

I haven't actually worked much with Tanstack Query hydration in the past. I would probably 
want to do a spike of that before committing to doing it with production code. Because I'm doing this in my free time
I thought that try this new approach would make the challenge a bit more interesting for myself.

I've focused on creating a small subset of components to show that I can work with basic HTML and think about semantic markup, but I would
normally consider adopting a component library such as shadcn/ui for some of the basic components.
