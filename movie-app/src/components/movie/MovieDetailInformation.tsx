import { Box, Divider, Typography } from "@mui/material";

import { Movie } from "@/hooks/movies/types.ts";

type MovieDetailInformationProps = Pick<
  Movie,
  "genres" | "release_date" | "original_title" | "original_language"
>;
const MovieDetailInformation = ({ ...props }: MovieDetailInformationProps) => {
  const {
    genres,
    original_language: originalLanguage,
    original_title: originalTitle,
    release_date: releaseDate,
  } = props;

  const languageNameInEnglish = new Intl.DisplayNames(["en"], {
    type: "language",
  });

  return (
    <>
      <Box
        sx={{
          display: "flex",
          flexWrap: "wrap",
          alignItems: "center",
          justifyContent: { xs: "center", md: "flex-start" },
          gap: 1,
        }}
      >
        <Divider />
        <Typography variant="body2">
          {genres?.map(({ name }) => name).join(" / ")}
        </Typography>
        <Divider />
        <Typography variant="body2">
          {new Date(releaseDate).getFullYear()}
        </Typography>
        <Divider />
        <Typography variant="body2">
          Origins: {originalTitle} (
          {originalLanguage && languageNameInEnglish.of(originalLanguage)})
        </Typography>
      </Box>
    </>
  );
};

export default MovieDetailInformation;
