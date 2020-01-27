import React from "react";
import { Switch, Route } from "react-router-dom";

//Pages
import HomePage from "./pages/homepage/homepage.component";
import AboutUs from "./pages/about-us/about-us.component";
import Contact from "./pages/contact/contact.component";
import Page404 from "./pages/page-404/page-404.component";
import Movie from "./pages/movie/movie.component";
import SearchPage from "./pages/search-page/search-page.component";

//Components
import Header from "./components/header/header.component";
import Footer from "./components/footer/footer.component";

function App() {
  return (
    <div>
      <Header />
      <Switch>
        <Route exact path="/" component={HomePage} />
        <Route exact path="/movie-details/:movieId" component={Movie} />
        <Route exact path="/search/:movieName" component={SearchPage} />
        <Route path="/aboutus" component={AboutUs} />
        <Route path="/contact" component={Contact} />
        <Route component={Page404} />
      </Switch>
      <Footer />
    </div>
  );
}

export default App;
