import { HttpResponse, http } from "msw";
export const fetchMoviesMock = http.get(
  "https://api.themoviedb.org/3/search/movie?query=harry&page=1&api_key=03b8572954325680265531140190fd2a",
  () => {
    return HttpResponse.json({
      results: [
        {
          id: 671,
          poster_path: "/hziiv14OpD73u9gAak4XDDfBKa2.jpg",
          original_title: "Harry Potter and the Philosopher's Stone",
          release_date: "2001-11-16",
          vote_average: 7.9,
          overview:
            "Harry Potter has lived under the stairs at his aunt and uncle's house his whole life. But on his 11th birthday, he learns he's a powerful wizard—with a place waiting for him at the Hogwarts School of Witchcraft and Wizardry. As he learns to harness his newfound powers with the help of the school's kindly headmaster, Harry uncovers the truth about his parents' deaths—and about the villain who's to blame.",
        },
      ],
    });
  }
);
export const handlers = [fetchMoviesMock];
