import { THUMBNAIL_URL, DEFAULT_IMG_URL } from '../../api/endpoints';

export const getThumbnailUrl = (thumbnail?: string) => {
  return thumbnail ? THUMBNAIL_URL + thumbnail : DEFAULT_IMG_URL;
};
