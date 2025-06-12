export type MovieParams = {
    id: number
    title?: string
    overview?: string
    releaseDate?: string
    voteAverage?: number
    posterPath?: string | null
    backdropPath?: string | null
    originalLanguage?: string
    tagline?: string
    popularity?: number
}

export default class Movie {
    public id: number
    public title?: string
    public overview?: string
    public releaseDate?: string
    public voteAverage?: number
    public posterPath?: string
    public backdropPath?: string
    public originalLanguage?: string
    public tagline?: string
    public popularity?: number

    constructor(movie: MovieParams) {
        this.id = movie.id
        this.title = movie.title
        this.overview = movie.overview
        this.releaseDate = movie.releaseDate
        this.voteAverage = movie.voteAverage
        this.posterPath = movie.posterPath ?? undefined
        this.backdropPath = movie.backdropPath ?? undefined
        this.originalLanguage = movie.originalLanguage
        this.tagline = movie.tagline
        this.popularity = movie.popularity
    }

    public update(updateInfo: MovieParams) {
        ;(Object.keys(updateInfo) as Array<keyof MovieParams>).forEach(
            (key) => {
                if (updateInfo[key] !== undefined) {
                    // @ts-expect-error - We know these properties match between Movie and MovieParams
                    this[key] = updateInfo[key]
                }
            },
        )
    }

    public clone(): Movie {
        return new Movie({
            id: this.id,
            title: this.title,
            overview: this.overview,
            releaseDate: this.releaseDate,
            voteAverage: this.voteAverage,
            posterPath: this.posterPath,
            backdropPath: this.backdropPath,
            originalLanguage: this.originalLanguage,
            tagline: this.tagline,
            popularity: this.popularity,
        })
    }

    public cloneAndOverride(updateInfo: MovieParams): Movie {
        const clonedMovie = this.clone()
        clonedMovie.update(updateInfo)

        return clonedMovie
    }
}
