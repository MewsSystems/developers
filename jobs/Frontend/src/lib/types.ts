export type Images = {
  secure_base_url: string
  poster_sizes: string[]
}

export type Config = {
  images: Images
}

export type Movie = {
  id: number
  title: string
  overview: string
  vote_average: number
  release_date: string
  poster_path: string
}

export type Movies = {
  page: number
  total_pages: number
  total_results: number
  results: Movie[]
}

export type Genre = { id: number; name: string }

export type Cast = {
  id: string
  job: string
  popularity: number
}

export type CastPerson = {
  id: string
  name: string
  known_for_department: string
  popularity: number
}

export type CrewPerson = {
  id: string
  name: string
  department: string
  popularity: number
}

export type Credits = {
  id: string
  cast: CastPerson[]
  crew: CrewPerson[]
}

export type Video = {
  id: string
  site: string
  key: string
  type: string
}

export type Videos = {
  results: Video[]
}

export type MovieDetails = Movie & {
  runtime: number
  genres: Genre[]
  credits: Credits
  videos: Videos
}

export type ID = {
  id: string
}

export type Filters = {
  query?: string
  page: string
}
