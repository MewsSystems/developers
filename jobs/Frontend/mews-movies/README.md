## Movie Search App

### Overview

This React TypeScript application provides a simple interface for searching movies using TMDB API. It consists of two main views + Error404 view in case a user types something non existent to the url:

* **Search View:** Displays a search input, paginated list of search results, and pagination controls.
* **Movie Detail View:** Displays detailed information about a selected movie.
* **Error 404 View:** Informs the user that the information/page they were trying to access doesn't exist and provides a button to go back.

### Published App: [Mews Movies](https://frontend-solution--olena-mews-movies.netlify.app/)

### Technical Implementation

**Frontend:**
* **React:** Used for building the user interface and managing component state.
* **TypeScript:** Ensured type safety and improved code maintainability.
* **styled-components:** Utilized for styling components and creating a consistent visual appearance.

**API Integration:**
* **TMDB API:** Integrated to fetch movie data based on search queries and movie IDs.
* **Axios:** Used for making HTTP requests to the API.

**User Experience:**

* **Search functionality:** Implemented debounced search to improve user experience by avoiding unnecessary API calls.
* **Pagination:** Implemented pagination to handle large search result sets efficiently.
* **Error handling:** Displayed user-friendly error messages in case of API failures.
* **User-friendly navigation:** I provided various ways to navigate through the pages and views, accounting for different user preferences for going back or moving through the pages of movies.
* **Mobile-first approach:** I've used reponsive design and applied the mobile-first approach the enable the use of the app on any gadget. 

### Potential Improvements

* **Infinite scrolling:** I considered doing infinite scrolling instead of numbered bar pagination, but I wasn't sure that would be acceptable based on the task, so I decided to stick with the obvious "way to load additional batch". However, I find infinite scrolling more convenient for a user, so I would do that in a real-life project.
* **Testing:** Unfortunatelly, I don't know how to write tests yet. As soon as I learn to do that, I'll be including unit and integration tests in my projects.
* **Advanced filtering:** Also in a larger project I would add a possibility to filter by genres, actors, directors. 
* **Movie recommendations:** Offering a list of recommended similar movies for each movie a user opens the Movie Detail view - I would add that at the botton of the Moview Detail page. 
* **Sorting found movies:** Since there are often hundreds and thousands of movies that appear in the list for a search, I would make sorting available: by most popular, most recent, etc. 
