import { Dialog, DialogTitle, DialogContent } from "@mui/material";
import { Movie } from "../api";

export default function MovieDetails({
  onClose,
  movie,
}: {
  onClose: () => void;
  movie: Movie;
}) {
  return (
    <Dialog open={true} onClose={onClose}>
      <DialogTitle>{movie.title}</DialogTitle>
      <DialogContent>{movie.overview}</DialogContent>
    </Dialog>
  );
}
