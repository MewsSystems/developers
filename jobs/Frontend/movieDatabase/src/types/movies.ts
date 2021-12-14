import React from "react";

export type MovieDetails = {
    title: string;
    tagline?: string;
    overview?: string;
    runtime?: number; // in minutes
    status: string;
    poster_path?: string; // relative path to image, starting with /. to be appended to img api "https://image.tmdb.org/t/p/w500"
    [rest:string]: any;
}

export type MovieListItem = {
    title: string;
    id: number;
    release_date: string;
    vote_average: number;
    [rest:string]: any;
}
