// MovieDetail.tsx
import { createPortal } from "react-dom";
import { useEffect } from "react";

import "../CSS/MovieDetail.css";
// Translation of ISO 639-1 languages to ISO 3166-1 flags
// courtesy of user hondnl on GitHub
import langToFlag from "../assets/lang-flag.ts";

interface MovieDetailProps {
  movie: SearchResult;
  isOpen: boolean;
  closeDetail: () => void;
}

const MovieDetail: React.FC<MovieDetailProps> = ({
  movie,
  isOpen,
  closeDetail,
}) => {
  const {
    original_title,
    original_language,
    poster_path,
    release_date,
    vote_average,
    vote_count,
    overview,
  } = movie;

  // Parse release year (if set)
  const setReleaseYear = (release: Date) => {
    const year = new Date(release).getFullYear();

    if (!Number.isNaN(year)) {
      return year;
    } else {
      return "?";
    }
  };

  // Flag API for proper flag display
  const setFlagUrl = (language: string) => {
    const lang: keyof typeof langToFlag = language;
    return (
      "https://flagsapi.com/" + langToFlag[lang].toUpperCase() + "/flat/24.png"
    );
  };

  // Prevent background scroll with open modal
  useEffect(() => {
    if (isOpen) {
      const body = document.body;

      // Check if scollbar is visible
      const scrollbarVisible = window.visualViewport!.width < window.innerWidth;
      body.style.overflow = "hidden";

      if (scrollbarVisible) {
        // Compensate for scrollbar to prevent site skipping right
        body.style.marginRight = "8px";
      }
    }

    if (!isOpen) {
      const body = document.body;
      body.style.overflow = "unset";
      body.style.marginRight = "0";
    }
  }, [isOpen]);

  return (
    <div>
      {isOpen &&
        createPortal(
          <div className="modal-backdrop" onClick={closeDetail} />,
          document.getElementById("backdrop-root")!
        )}

      {isOpen &&
        createPortal(
          <div className="modal">
            {poster_path != null ? (
              <>
                <img
                  className="modal-poster"
                  src={"https://image.tmdb.org/t/p/original/" + poster_path}
                />
                <div className="poster-placeholder" />
              </>
            ) : (
              <div className="poster-placeholder" />
            )}
            <div className="modal-info">
              <h1>{original_title}</h1>
              <div className="modal-info-small">
                <span>
                  <b>Released: </b>
                  <p>{setReleaseYear(release_date)}</p>
                </span>
                <span>
                  <b>Language:</b>
                  <img className="flag" src={setFlagUrl(original_language)} />
                </span>
              </div>
              <div className="modal-info-small">
                <span>
                  <b>Average score:</b>
                  <p>{vote_average.toPrecision(2)}/10</p>
                </span>
                <span>
                  <b>Votes:</b>
                  <p>{vote_count}</p>
                </span>
              </div>
              <div className="modal-overview">
                <span>{overview}</span>
              </div>
            </div>
          </div>,
          document.getElementById("modal-root")!
        )}
    </div>
  );
};

export default MovieDetail;
