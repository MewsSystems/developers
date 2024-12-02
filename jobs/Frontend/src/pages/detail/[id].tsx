import { movieService } from "@/services/movieService";
import { Movie } from "@/types";
import { fallbackImageHandler } from "@/utils/helpers";
import {
  Grid,
  Link,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
} from "@mui/material";
import { GetServerSideProps } from "next";
import Image from "next/image";
import styles from "./Detail.module.css";

export const getServerSideProps = (async (context) => {
  if (!context.params?.id) {
    throw new Error("Missing id");
  }

  const id = context?.params.id as unknown as number;
  const movie = await movieService.getById(id);
  return { props: { ...movie } };
}) satisfies GetServerSideProps<Movie>;

export default function Detail({
  id,
  title,
  posterImage,
  overview,
  homepage,
  genres,
  spokenLanguages,
}: Movie) {
  return (
    <Grid container justifyContent="center" spacing={2}>
      <Grid item xs={8} marginTop={9}>
        <Image
          key={`item-icons-image-${id}`}
          about={title}
          src={`${posterImage}`}
          alt={title}
          loading="lazy"
          width={185}
          height={100}
          className={styles["image-hero"]}
          onError={fallbackImageHandler}
        />
      </Grid>
      <Grid item xs={8} marginTop={9}>
        <TableContainer component={Paper}>
          <Table sx={{ minWidth: 650 }} aria-label="simple table">
            <TableBody>
              <TableRow>
                <TableCell>Title</TableCell>
                <TableCell>{title}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Overview</TableCell>
                <TableCell>{overview}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Homepage</TableCell>
                <TableCell>
                  <Link target="_blank" href={homepage}>
                    {homepage}
                  </Link>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Genres</TableCell>
                <TableCell>{genres.join(", ")}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Spoken languages</TableCell>
                <TableCell>{spokenLanguages.join(", ")}</TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
      </Grid>
    </Grid>
  );
}
