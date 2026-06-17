import React, { memo, useCallback, useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import MovieDetailModal from "../MovieDetailModal/MovieDetailModal";
import { StyledModal } from "../Modal/Modal.styles";

/**
 * If the URL query string contains a movie ID, the modal will movie detail modal will open.
 */
const MovieDetailModalOpener = () => {
  const [isOpen, setIsOpen] = useState(false);

  const navigate = useNavigate();
  const { search } = useLocation();

  const query = useMemo(() => new URLSearchParams(search), [search]);
  const id = query.get("id");

  useEffect(() => {
    if (id) {
      setIsOpen(true);
    }
  }, [id]);

  const handleCloseModal = useCallback(() => {
    setIsOpen(false);

    const newSearch = new URLSearchParams(query);
    newSearch.delete("id");
    navigate(`?${newSearch.toString()}`, { replace: true });
  }, [query, navigate]);

  if (!id) {
    return null;
  }

  return (
    <StyledModal
      isOpen={isOpen}
      onBackgroundClick={handleCloseModal}
      onEscapeKeydown={handleCloseModal}
    >
      <MovieDetailModal movieId={id} onCloseModal={handleCloseModal} />
    </StyledModal>
  );
};

export default memo(MovieDetailModalOpener);
