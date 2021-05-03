import moment from "moment";
import {Genres} from "./types";

export const getGenres = (arr: Genres[]): string => {
    const result: string[] = [];
    arr.map((genre) => result.push(genre.name));
    return result.join(" | ");
};

export const getCategory = (adult: boolean | undefined): string =>
    !!adult ? "18+" : "pro všechny";

export const getReleaseDate = (date: string | undefined): string => {
    const format = "YYYY-MM-DD";
    return moment(date, format).format("MMMM DD, YYYY");
};

export const getRevenue = (revenue: number | undefined )=>{
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits : 0,
    })
    return revenue ? formatter.format(revenue) : `-`
}

export const getCeilScore = (score:number )=> Math.ceil(score)