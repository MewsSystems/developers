import BaseCard from "../BaseCard";
import Typography from "../Typography";
import Button from "../Button";
import { theme } from "../../theme";

interface MovieDetailsCardProps {
	title: string;
	posterPath: string | null;
	releaseDate: string;
	overview: string;
	genres: { name: string }[];
	rating: number;
	onGoBack: () => void;
}

const MovieDetailsCard = ({
	title,
	posterPath,
	releaseDate,
	overview,
	genres,
	rating,
	onGoBack,
}: MovieDetailsCardProps) => {
	return (
		<BaseCard size={3}>
			<BaseCard.Body $layout="row">
				<BaseCard.Poster posterPath={posterPath} alt={`${title} Poster`} />
				<BaseCard.Content>
					<Typography variant="h3">{title}</Typography>
					<Typography variant="h6">Release Date: {releaseDate}</Typography>
					<Typography variant="h6">
						Genres: {genres.map((genre) => genre.name).join(", ")}
					</Typography>
					<Typography variant="body" textAlign="justify">
						{overview || "No overview available."}
					</Typography>
					<Typography variant="h6" color={theme.palette.primary.main}>
						Rating: {rating || 0}/10
					</Typography>
				</BaseCard.Content>
			</BaseCard.Body>
			<BaseCard.Footer>
				<Button onClick={onGoBack}>Go back!</Button>
			</BaseCard.Footer>
		</BaseCard>
	);
};

export default MovieDetailsCard;
