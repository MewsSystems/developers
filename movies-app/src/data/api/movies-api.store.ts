export class MoviesApi {
    init(): Promise<unknown> {
        return fetch('https://api.themoviedb.org/3/movie/11?api_key=03b8572954325680265531140190fd2a');
    }
}