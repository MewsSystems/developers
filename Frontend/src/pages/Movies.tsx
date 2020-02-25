import React from 'react'
import { Switch, Route } from 'react-router-dom'
import MovieIndex from './movie-index/MovieIndex'
import MovieInfo from './movie-info/MovieInfo'

const Movies = () => {
  return (
    <Switch>
      <Route path="/movies" exact>
        <MovieIndex />
      </Route>
      <Route path="/movies/:id">
        <MovieInfo />
      </Route>
    </Switch>
  )
}

export default Movies
