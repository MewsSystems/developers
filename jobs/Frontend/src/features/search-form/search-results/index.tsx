import { Link } from "react-router-dom"
import { Props } from "./model"
import { DateHelper } from "../../../helpers/date-helper";
import styled from "styled-components";

const yearInfo = (date: string) =>
    date?.length > 0
        ? `(${DateHelper.getYear(date)})`
        : '';

export const SearchResults = (props: Props) => {
    if (props.movies.length === 0) return (
        <p>No results found</p>
    )

    return (
        <>
            {props.movies.map((movie) => (
                <StyledLink to={`/movie/${movie.id}`} key={movie.id}>
                    <StyledSearchResultItem>
                        {movie.title} {yearInfo(movie.release_date)}
                    </StyledSearchResultItem>
                </StyledLink>
            ))}
        </>
    )
}

const StyledLink = styled(Link)`
    text-decoration: none;
    color: black;
`;

const StyledSearchResultItem = styled.div`
    list-style-type: none;
    padding: 1px 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    margin-bottom: 1px;
    background-color: rgba(241,241,241,0.5);
    box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.75);
    &:hover {
        background-color: #f1f1f1;
    }
`;