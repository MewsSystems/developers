import Link from "next/link";
import Image from "next/image";
import Divider from "@mui/material/Divider";
import { ResultsListItemProps, ResultsListProps } from "./types";
import { MovieSummary } from "@/types";
import {
  Avatar,
  ImageListItem,
  List,
  ListItem,
  ListItemAvatar,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";
import { fallbackImageHandler } from "@/utils/helpers";

import styles from "./searchResults.module.css";

const SearchResultsListItem: React.FC<ResultsListItemProps> = ({ item }) => (
  <>
    <ListItemButton
      alignItems="flex-start"
      key={`item-button-${item.id}`}
      component="a"
      href={`/detail/${item.id}`}
    >
      <ListItemIcon key={`item-icons-${item.id}`}>
        <Image
          key={`item-icons-image-${item.id}`}
          about={item.title}
          src={`${item.posterImage}`}
          alt={item.title}
          loading="lazy"
          width={92}
          height={100}
          className={styles["image-thumbnail"]}
          onError={fallbackImageHandler}
        />
      </ListItemIcon>
      <ListItemText
        key={`item-icons-text-${item.id}`}
        primary={item.title}
        secondary={
          <>
            <Typography
              sx={{ display: "inline" }}
              component="span"
              variant="body2"
              color="text.primary"
            >
              {item.overview}
            </Typography>
          </>
        }
      />
    </ListItemButton>
    <Divider key={`divider-${item.id}`} component="li" flexItem />
  </>
);

export const SearchResults: React.FC<ResultsListProps> = ({ results }) => (
  <List
    sx={{ width: "100%", bgcolor: "background.paper" }}
    key="search-results"
  >
    {results?.movies.map((item: MovieSummary) => (
      <SearchResultsListItem key={item.id} item={item} />
    ))}
  </List>
);
