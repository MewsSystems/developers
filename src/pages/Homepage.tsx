import { useState } from "react";
import { useQueryClient } from "react-query";

import { SearchForm } from "../components/SearchForm";
import { MovieGrid } from "../components/MovieGrid";
import { Header } from "../components/Header";
import { Footer } from "../components/Footer";

export const Homepage = () => {
    const queryClient = useQueryClient();
    const [searchTerm, setSearchTerm] = useState(queryClient.getQueryData("searchTerm") || "");
    const [page, setPage] = useState(queryClient.getQueryData("page") || 1);

    const handleSearch = (searchTerm: string) => {
        setSearchTerm(searchTerm);
        setPage(1);
        queryClient.setQueryData("searchTerm", searchTerm);
        queryClient.setQueryData("page", 1);
    };

    return (
        <>
            <Header />
            <SearchForm handleSearch={handleSearch} term={searchTerm} />
            <MovieGrid term={searchTerm} page={page} setPage={setPage} />
            <Footer />
        </>
    );
};
