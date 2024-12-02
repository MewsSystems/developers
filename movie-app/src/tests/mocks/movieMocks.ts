export const movieSearchResultsMock = [
    {
        id: 1,
        overview: 'Movie overview',
        poster_path: 'poster.jpg',
        title: 'Movie Title',
        vote_average: 7.5,
        vote_count: 100,
        release_date: '2022-01-01',
        original_language: 'en',
        original_title: 'Original Title',
        tagline: 'Movie tagline',
        genres: [
            { id: 1, name: 'Action' },
            { id: 2, name: 'Adventure' },
        ],
    },
    {
        id: 123,
        overview: 'A thrilling adventure awaits in this action-packed movie!',
        poster_path: 'poster_image.jpg',
        title: 'Adventure of a Lifetime',
        vote_average: 8.2,
        vote_count: 350,
        release_date: '2023-05-15',
        original_language: 'en',
        original_title: 'Adventure of a Lifetime',
        tagline: 'The ultimate journey begins here!',
        genres: [{ id: 3, name: 'Thriller' }],
    },
]

export const movieDetailInformationMock = [
    {
        genres: [
            { id: 1, name: 'Action' },
            { id: 2, name: 'Adventure' },
        ],
        original_language: 'en',
        original_title: 'Original Title',
        release_date: '2022-01-01',
    },
    {
        genres: [
            { id: 3, name: 'Action' },
            { id: 4, name: 'Adventure' },
        ],
        original_language: 'fr',
        original_title: 'Titre Original',
        release_date: '2022-01-01',
    },
]
