import { createSlice } from "@reduxjs/toolkit"

const initialState = {
  page: 1,
  results: [
    {
      id: 671,
      poster_path: "/wuMc08IPKEatf9rnMNXvIDxqP4W.jpg",
      title: "Harry Potter and the Philosopher's Stone",
    },
    {
      id: 672,
      poster_path: "/sdEOH0992YZ0QSxgXNIGLq1ToUi.jpg",
      title: "Harry Potter and the Chamber of Secrets",
    },
    {
      id: 674,
      poster_path: "/fECBtHlr0RB3foNHDiCBXeg9Bv9.jpg",
      title: "Harry Potter and the Goblet of Fire",
    },
    {
      id: 673,
      poster_path: "/aWxwnYoe8p2d2fcxOqtvAtJ72Rw.jpg",
      title: "Harry Potter and the Prisoner of Azkaban",
    },
    {
      id: 12444,
      poster_path: "/iGoXIpQb7Pot00EEdwpwPajheZ5.jpg",
      title: "Harry Potter and the Deathly Hallows: Part 1",
    },
    {
      id: 12445,
      poster_path: "/c54HpQmuwXjHq2C9wmoACjxoom3.jpg",
      title: "Harry Potter and the Deathly Hallows: Part 2",
    },
    {
      id: 675,
      poster_path: "/5aOyriWkPec0zUDxmHFP9qMmBaj.jpg",
      title: "Harry Potter and the Order of the Phoenix",
    },
    {
      id: 767,
      poster_path: "/z7uo9zmQdQwU5ZJHFpv2Upl30i1.jpg",
      title: "Harry Potter and the Half-Blood Prince",
    },
    {
      id: 25941,
      poster_path: "/lk9qVzvPHuL2vX9ixNut9n0KfYD.jpg",
      title: "Harry Brown",
    },
    {
      id: 47943,
      poster_path: "/u6OzWGLlzGXDJJGGWbb6AWKxmoj.jpg",
      title: "With a Friend Like Harry...",
    },
    {
      id: 899082,
      poster_path: "/jntLBq0MLR3hrwKaTQswxACRPMs.jpg",
      title: "Harry Potter 20th Anniversary: Return to Hogwarts",
    },
    {
      id: 441906,
      poster_path: "/rWX9ZPcKH6qId169zm8GdFlRTdy.jpg",
      title: "Jab Harry Met Sejal",
    },
    {
      id: 639,
      poster_path: "/3wkbKeowUp1Zpkw1KkBqMWbt0P9.jpg",
      title: "When Harry Met Sally...",
    },
    {
      id: 483898,
      poster_path: "/g1xiBoLD6v3ZaXPa4QtuXiQeYKW.jpg",
      title: "50 Greatest Harry Potter Moments",
    },
    {
      id: 584192,
      poster_path: "/a0ymZ8WfhjarZMAHieJFCcC8bPa.jpg",
      title: "Who Framed Harry Lund?",
    },
    {
      id: 984,
      poster_path: "/UHxxkYe9tRdiPu0JFgcEL5hmQ4.jpg",
      title: "Dirty Harry",
    },
    {
      id: 10152,
      poster_path: "/qxD6P3UXASFCdbrVavceZzgOfI9.jpg",
      title: "Dumb and Dumberer: When Harry Met Lloyd",
    },
    {
      id: 24693,
      poster_path: "/uOqUwsozWzPhAwvbcEIRFiiqitw.jpg",
      title: "The Olsen Gang and Data-Harry Blows Up The World Bank",
    },
    {
      id: 482408,
      poster_path: "/kkzDng2S8w2hT1gap7MdMdx7uUE.jpg",
      title: "Harry Potter: A History Of Magic",
    },
    {
      id: 1003152,
      poster_path: "/9DGZNooSu4LwxjT3nfeo0IJP42.jpg",
      title: "Harry Belafonte: Between Calypso and Justice",
    },
  ],
  total_pages: 20,
  total_results: 386,
}

const moviesSlice = createSlice({
  name: "movies",
  initialState,
  reducers: {},
})

export default moviesSlice.reducer
