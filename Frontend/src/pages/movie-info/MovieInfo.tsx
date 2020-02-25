import React, { useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { connect } from 'react-redux'
import Container from '../../layout/Container'
import Page from '../../layout/Page'
import styled from '../../utils/styled'
import {
  MovieInfobox,
  MovieInfoboxBlurBackground,
  MovieInfoboxInner,
  MovieInfoboxImage,
  MovieInfoboxHeading,
  MovieName,
  MovieRoles,
  MovieReview
} from './MovieInfoHeader'
import { API_ENDPOINT_IMAGE } from '../../utils/api'
import { MovieStatsInner, MovieStats, StatAttribute, Bullet } from './MovieInfoStats'
import { MovieDetails, MovieDetailsColumn, MovieDetailsRow, MovieDetailsAttrName } from './MovieInfoDetails'
import { Loading } from '../../layout/Loading'
import { ApplicationState } from '../../store'
import { fetchInfoRequest } from '../../store/movie-info/actions'
import { MovieInfoState } from '../../store/movie-info/types'

// We can use `typeof` here to map our dispatch types to the props, like so.
type PropsFromDispatch = {
  fetchRequest: typeof fetchInfoRequest
}

type AllProps = MovieInfoState & PropsFromDispatch

const MovieInfo = ({ loading, data: movie, fetchRequest }: AllProps) => {
  const { id } = useParams()

  useEffect(() => {
    if (id) {
      const idInt: number = parseInt(id, 10)
      fetchRequest(idInt)
    }
  }, [id])

  return (
    <Page>
      <Container>
        <Wrapper>
          <Loading loading={loading} />
          {movie && (
            <>
              <MovieInfobox>
                <MovieInfoboxBlurBackground src={`${API_ENDPOINT_IMAGE}/w500${movie.poster_path}`} />
                <MovieInfoboxInner>
                  <MovieInfoboxImage src={`${API_ENDPOINT_IMAGE}/w500${movie.poster_path}`} />
                  <MovieInfoboxHeading>
                    <MovieName>{movie.title}</MovieName>
                    <MovieRoles>
                      genres: <span>{movie.genres.map(g => g.name).join(', ')}</span>
                    </MovieRoles>
                    <MovieReview>{movie.overview}</MovieReview>
                  </MovieInfoboxHeading>
                  <MovieStats>
                    <MovieStatsInner>
                      <StatAttribute attr="str" isPrimaryAttr={movie.primary_attr === 'str'}>
                        <Bullet attr="str" /> {movie.vote_count || 0}
                      </StatAttribute>
                      <StatAttribute attr="agi" isPrimaryAttr={movie.primary_attr === 'agi'}>
                        <Bullet attr="agi" /> {movie.vote_average || 0}
                      </StatAttribute>
                      <StatAttribute attr="int" isPrimaryAttr={movie.primary_attr === 'int'}>
                        <Bullet attr="int" /> {movie.popularity || 0}
                      </StatAttribute>
                    </MovieStatsInner>
                  </MovieStats>
                </MovieInfoboxInner>
              </MovieInfobox>
              <MovieDetails>
                <MovieDetailsColumn>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Release Date:</MovieDetailsAttrName> {movie.release_date || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Budget:</MovieDetailsAttrName> {movie.budget || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Revenue:</MovieDetailsAttrName> {movie.revenue || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Runtime:</MovieDetailsAttrName> {movie.runtime || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Spoken Languages:</MovieDetailsAttrName>{' '}
                    {movie.spoken_languages?.map(l => l.name).join(', ') || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Original Language:</MovieDetailsAttrName> {movie.original_language || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Original Title:</MovieDetailsAttrName> {movie.original_title || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Tagline:</MovieDetailsAttrName> {movie.tagline || '-'}
                  </MovieDetailsRow>
                </MovieDetailsColumn>
                <MovieDetailsColumn>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Production Companies:</MovieDetailsAttrName>
                    {movie.production_companies
                      ?.map(pc => pc.name)
                      .slice(0, 2)
                      .join(', ') || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Production Countries:</MovieDetailsAttrName>
                    {movie.production_countries
                      .map(pc => pc.name)
                      .slice(0, 2)
                      .join(', ') || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Homepage:</MovieDetailsAttrName> {movie.homepage || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>IMDB id:</MovieDetailsAttrName> {movie.imdb_id || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Status:</MovieDetailsAttrName> {movie.status || '-'}
                  </MovieDetailsRow>
                  <MovieDetailsRow>
                    <MovieDetailsAttrName>Video:</MovieDetailsAttrName> {movie.video ? 'yes' : 'no'}
                  </MovieDetailsRow>
                </MovieDetailsColumn>
              </MovieDetails>
            </>
          )}
        </Wrapper>
      </Container>
    </Page>
  )
}

// It's usually good practice to only include one context at a time in a connected component.
// Although if necessary, you can always include multiple contexts. Just make sure to
// separate them from each other to prevent prop conflicts.
const mapStateToProps = ({ movieInfo }: ApplicationState) => ({
  loading: movieInfo.loading,
  errors: movieInfo.errors,
  data: movieInfo.data
})

// mapDispatchToProps is especially useful for constraining our actions to the connected component.
// You can access these via `this.props`.
const mapDispatchToProps = {
  fetchRequest: fetchInfoRequest
}

// Now let's connect our component!
// With redux v4's improved typings, we can finally omit generics here.
export default connect(mapStateToProps, mapDispatchToProps)(MovieInfo)

const Wrapper = styled('div')`
  position: relative;
`
