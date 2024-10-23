import { imagesApiClient } from './lib/api-client-images.ts';
import { queryOptions, useQuery } from '@tanstack/react-query';
import { QueryConfig } from './lib/react-query-config.ts';
import { notFoundPosterImages } from '../../assets/base64images/movieImages.ts';

export const getImage = ({ imagePath, imageWidth }: { imagePath: string; imageWidth: number }): Promise<string> => {
  if (!imagePath) {
    return Promise.resolve(notFoundPosterImages);
  }
  return imagesApiClient.get(`/w${imageWidth}/${imagePath}`, { responseType: 'arraybuffer' }).then((response) => {
    const image = btoa(new Uint8Array(response.data).reduce((data, byte) => data + String.fromCharCode(byte), ''));
    return `data:${response.headers['content-type'].toLowerCase()};base64,${image}`;
  });
};

export const getImageQueryOptions = (imagePath: string, imageWidth: number) => {
  return queryOptions({
    queryKey: ['discussions', { imagePath, imageWidth }],
    queryFn: () => getImage({ imagePath, imageWidth }),
  });
};

type UseImageOptions = {
  imagePath: string;
  imageWidth: number;
  queryConfig?: QueryConfig<typeof getImageQueryOptions>;
};

export const useImage = ({ imagePath, imageWidth, queryConfig }: UseImageOptions) => {
  return useQuery({
    ...getImageQueryOptions(imagePath, imageWidth),
    ...queryConfig,
  });
};
