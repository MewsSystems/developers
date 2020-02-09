export default class Movie{
    id:number;
    title:string;
    poster_path:string;
    overview:string;
    constructor(id,title,poster_path,overview) {
        this.id=id;
        this.title = title;
        this.poster_path = poster_path;
        this.overview = overview;
    }
}
