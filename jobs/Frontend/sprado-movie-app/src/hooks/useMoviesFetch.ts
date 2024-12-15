import React, { useState, useEffect, useCallback } from "react";

type FetchState = {
  data: any | null;
  isLoading: boolean;
  error: Error | null;
};
export const useMoviesFetch = (
  baseUrl: string,
  query: string,
  page: number
) => {
  const [fetchState, setFetchState] = useState<FetchState>({
    data: null,
    isLoading: false,
    error: null,
  });

  const fetchData = useCallback(async () => {
    const url = `${baseUrl}&query=${encodeURIComponent(query)}&page=${page}`;

    if (!url) {
      setFetchState({ data: null, isLoading: false, error: null });
      return;
    }

    try {
      setFetchState((prevValue) => ({ ...prevValue, isLoading: true }));

      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(response.statusText);
      }

      const data = await response.json();
      setFetchState({ data, isLoading: false, error: null });
    } catch (error) {
      setFetchState({ data: null, isLoading: false, error: error as Error });
    }
  }, [baseUrl, query, page]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return {
    data: fetchState.data?.results || [],
    totalPages: fetchState.data?.total_pages || 1,
    isLoading: fetchState.isLoading,
    error: fetchState.error,
  };
};
