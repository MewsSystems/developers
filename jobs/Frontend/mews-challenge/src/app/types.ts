export enum Color {
    "background" = "#333",
    "primary.dark" = '#ccc',
    "primary.light" = 'white',
    "secondary.dark" = 'rgba(255,165,0,0.5)',
    "secondary.light" = 'orange',
    "link" = '#5af',
}

export enum Spacing {
    zero = '0px',
    small = '5px',
    base = '10px',
    big = '20px',
    huge = '50px',
    auto = 'auto',
}

export enum FontSize {
    small = '18px',
    base = '22px',
    big = '28px',
    huge = '40px',
}

export type Movie = {
    title: string,
    vote_average: number,
    vote_count: number,
    release_date: string,
    poster_path: string,
    overview: string
}

export type TMDBResponse = {
    results: Movie[],
    total_pages: number
}

export enum ButtonType {
    link = 'link',
    button = 'button',
}